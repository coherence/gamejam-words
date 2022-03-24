using System.Collections;
using System.Collections.Generic;
using Coherence.Log;
using Coherence.Toolkit;
using UnityEngine;
using Logger = Coherence.Log.Logger;

public class Simulation : CoherenceInputSimulation<SimulationState>
{
    const int InternalMinClientNumberToPlay = 1;
    
    public Grid grid;
    public string hash;
    public long Frame => CurrentSimulationFrame;

    private Logger logger = Log.GetLogger<Simulation>();
    private List<(string, object)> logCache = new List<(string, object)>();

    private void PrintSnapshot(string context)
    {
        logCache.Clear();
        logCache.Add(("Context", context));
        logCache.Add(("FrameDiff", monoBridge.NetworkTime.ClientSimulationFrame.Frame - monoBridge.NetworkTime.ServerSimulationFrame.Frame));
        logCache.Add(("AckFrame", inputManager.AcknowledgedFrame));
        logCache.Add(("SimFrame", CurrentSimulationFrame));
        logCache.Add(("RecvFrame", inputManager.CommonReceivedFrame));
        logCache.Add(("Pause", inputManager.ShouldPause));
        foreach (ICoherenceInput input in inputManager.AllInputs)
        {
            logCache.Add((input.Owner.ToString(),
                $"Ack: {input.LastAcknowledgedFrame}, Recv: {input.LastReceivedFrame}, Sent: {input.LastSentFrame}"));
        }

        logger.Debug($"Input snapshot", logCache.ToArray());
    }

    protected override void SetInputs(CoherenceClientConnection client)
    {
        var player = client.GameObject.GetComponent<Player>();
        player.ApplyLocalInputs();
    }

    protected override void OnStart()
    {
        SimulationEnabled = false;
        monoBridge.OnTimeReset += () => PrintSnapshot("TimeReset");
    }

    protected override void OnDisconnected()
    {
        PrintSnapshot("OnDisconnected");
    }

    protected override void OnBeforeSimulate()
    {
        if (grid.IsPaused)
        {
            long startFrame = 0;
            foreach (CoherenceClientConnection client in AllClients)
            {
                var player = client.Sync.GetComponent<Player>();
                if (player.startOnFrame != 0)
                {
                    if (player.startOnFrame > startFrame)
                    {
                        startFrame = player.startOnFrame;
                    }
                }
            }

            grid.startFrame = startFrame;
            if (startFrame > 0 && CurrentSimulationFrame >= grid.startFrame)
            {
                SimulationEnabled = true;
            }
            
            return; // TODO
        }
    }

    protected override void Simulate(long simulationFrame)
    {
        grid.UpdateSimulationFrame(simulationFrame);
        
        foreach (CoherenceClientConnection client in AllClients)
        {
            var player = client.GameObject.GetComponent<Player>();
            var alphabetInt = Mathf.Ceil(player.GetNetworkInputString(simulationFrame));
            var movement = (Vector3)player.GetNetworkInputMovement(simulationFrame);
            
            if (alphabetInt != 0)
            {
                var key = (KeyCode) alphabetInt;
                grid.SetCellContentAndCheckWord(player.gridPosition.x, player.gridPosition.y, key.ToString(), client.ClientId, simulationFrame);
            }
            
            var hasMovement = false;
            
            if (movement.x > 0)
            {
                hasMovement = true; 
                player.gridPosition.x += 1;
            }
            
            if (movement.x < 0)
            {
                hasMovement = true; 
                player.gridPosition.x -= 1;
            }
            
            if (movement.y > 0)
            {
                hasMovement = true; 
                player.gridPosition.y += 1;
            }

            if (movement.y < 0)
            {
                hasMovement = true;
                player.gridPosition.y -= 1;
            }
            
            if (hasMovement)
            {
                if (player.gridPosition.x < 0) player.gridPosition.x = Grid.tilesX - 1;
                if (player.gridPosition.y < 0) player.gridPosition.y = Grid.tilesY - 1;
                if (player.gridPosition.x >= Grid.tilesX) player.gridPosition.x = 0;
                if (player.gridPosition.y >= Grid.tilesY) player.gridPosition.y = 0;

                //grid.UpdatePlayerPositionAndClearNonSolidCells(client.ClientId, player.gridPosition.x, player.gridPosition.y);
            }

           
        }
    }

    protected override void Rollback(long toFrame, SimulationState state)
    {
        for (var i = 0; i < AllClients.Count; i++)
        {
            var player = AllClients[i].GameObject.GetComponent<Player>();
            player.gridPosition = state.PlayerPositions[i];
        }

        grid.SetSimulationState(state);
    }

    protected override SimulationState CreateState()
    {
        var simulationState = new SimulationState { PlayerPositions = new Vector2Int[AllClients.Count]};
        for (var i = 0; i < AllClients.Count; i++)
        {
            var player = AllClients[i].GameObject.GetComponent<Player>();
            simulationState.PlayerPositions[i] = player.gridPosition;
        }

        simulationState.CellStates = grid.GetSimulationState(out ArrayList wordsUsed, out Hashtable playerScores);
        
        simulationState.wordsUsed = wordsUsed;

        simulationState.PlayerScores = playerScores;

        hash = simulationState.ComputeHash().ToString();
        
        return simulationState;
    }

    protected override void OnClientJoined(CoherenceClientConnection client)
    {
        grid.AddPlayer(client);
        PrintSnapshot("ClientJoined");
    }

    protected override void OnClientLeft(CoherenceClientConnection client)
    {
        grid.RemovePlayer(client);
        PrintSnapshot("ClientLeft");
    }
    
    protected override void OnPauseChange(bool isPaused)
    {
        PrintSnapshot($"Pause:{isPaused}");
        //Debug.Log($"[{CurrentSimulationFrame}] Pause: {isPaused}");
        //grid.TogglePaused(isPaused);
    }
}