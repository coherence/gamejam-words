using System.Collections;
using UnityEngine;

public struct SimulationState
{
    public Vector2Int[] PlayerPositions;

    public Hashtable PlayerScores;

    public Cell[] CellStates;

    public ArrayList wordsUsed;
    
    public struct Cell
    {
        public string content;
        public int owner;
        public bool isSolid;
        public long frameWhenEntered;
    }
}