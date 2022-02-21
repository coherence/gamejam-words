using System;
using System.Collections;
using Coherence.Toolkit;
using Unity.VisualScripting;
using UnityEngine;

public class Simulation : CoherenceInputSimulation<SimulationState>
{
    public Grid grid;
    
    const int InternalMinClientNumberToPlay = 1; 
    protected override void SetInputs(CoherenceClientConnection client)
    {
        var player = client.GameObject.GetComponent<Player>();
        player.ApplyLocalInputs();
    }

    protected override void Simulate(long simulationFrame)
    {
        grid.UpdateSimulationFrame(simulationFrame);

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
            
            return; // TODO
        }
        
        foreach (CoherenceClientConnection client in AllClients)
        {
            var player = client.GameObject.GetComponent<Player>();
            var movement = (Vector3)player.GetNetworkInputMovement(simulationFrame);
            var alphabetInt = Mathf.Ceil(player.GetNetworkInputString(simulationFrame));

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
                if (player.gridPosition.x < 0) player.gridPosition.x = Grid.tiling - 1;
                if (player.gridPosition.y < 0) player.gridPosition.y = Grid.tiling - 1;
                if (player.gridPosition.x >= Grid.tiling) player.gridPosition.x = 0;
                if (player.gridPosition.y >= Grid.tiling) player.gridPosition.y = 0;

                //grid.UpdatePlayerPositionAndClearNonSolidCells(client.ClientId, player.gridPosition.x, player.gridPosition.y);
            }

            if (alphabetInt != 0)
            {
                var key = (KeyCode) alphabetInt;
                grid.SetCellContentAndCheckWord(player.gridPosition.x, player.gridPosition.y, key.ToString(), client.ClientId, simulationFrame);
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
        
        return simulationState;
    }

    protected override void OnClientJoined(CoherenceClientConnection client)
    {
        SimulationEnabled = AllClients.Count >= InternalMinClientNumberToPlay;
        if (SimulationEnabled)
        {
            // Lets us rejoin the same simulation without restarting the app.
            StateStore.Clear();
        }

        grid.AddPlayer(client);
    }

    protected override void OnClientLeft(CoherenceClientConnection client)
    {
        grid.RemovePlayer(client);
        
        SimulationEnabled = AllClients.Count >= InternalMinClientNumberToPlay;
    }
    
    protected override void OnPauseChange(bool isPaused)
    {
        //grid.TogglePaused(isPaused);
    }
}