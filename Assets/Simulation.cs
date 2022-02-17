using System;
using Coherence.Toolkit;
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
        foreach (CoherenceClientConnection client in AllClients)
        {
            var player = client.GameObject.GetComponent<Player>();
            var movement = (Vector3)player.GetNetworkInputMovement(simulationFrame);
            var alphabetInt = Mathf.Ceil(player.GetNetworkInputString(simulationFrame));
            
            if (movement.x > 0) player.gridPosition.x += 1;
            if (movement.x < 0) player.gridPosition.x -= 1;
            if (movement.y > 0) player.gridPosition.y += 1;
            if (movement.y < 0) player.gridPosition.y -= 1;

            if (player.gridPosition.x < 0) player.gridPosition.x = Grid.tiling - 1;
            if (player.gridPosition.y < 0) player.gridPosition.y = Grid.tiling - 1;
            if (player.gridPosition.x >= Grid.tiling) player.gridPosition.x = 0;
            if (player.gridPosition.y >= Grid.tiling) player.gridPosition.y = 0;

            if (alphabetInt != 0)
            {
                var key = (KeyCode) alphabetInt;
                
                //Debug.Log($"GetButtonRangeState {alphabetInt} {key} {key.ToString()}");
                
                grid.SetCellContentAndCheckWord(player.gridPosition.x, player.gridPosition.y, key.ToString(), client.ClientId);
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

        grid.SetSimulationState(state.CellStates);
    }

    protected override SimulationState CreateState()
    {
        var simulationState = new SimulationState { PlayerPositions = new Vector2Int[AllClients.Count]};
        for (var i = 0; i < AllClients.Count; i++)
        {
            var player = AllClients[i].GameObject.GetComponent<Player>();
            simulationState.PlayerPositions[i] = player.gridPosition;
        }

        simulationState.CellStates = grid.GetSimulationState();
        
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
    }

    protected override void OnClientLeft(CoherenceClientConnection client)
    {
        SimulationEnabled = AllClients.Count >= InternalMinClientNumberToPlay;
    }
    
    protected override void OnPauseChange(bool isPaused)
    {
        //PauseScreen.SetActive(isPaused);
    }
}