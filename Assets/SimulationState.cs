using UnityEngine;

public struct SimulationState
{
    public Vector2Int[] PlayerPositions;

    public Cell[] CellStates;
    
    public struct Cell
    {
        public string content;
        public int owner;
        public bool isSolid;
        public long frameWhenEntered;
    }
}