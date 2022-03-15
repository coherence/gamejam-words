using System.Collections;
using System.Collections.Generic;
using Coherence.Toolkit;
using UnityEngine;

public struct SimulationState : IHashable
{
    public Vector2Int[] PlayerPositions;

    public Hashtable PlayerScores;

    public List<Cell> CellStates;

    public ArrayList wordsUsed;
    
    public struct Cell
    {
        public int x;
        public int y;
        public string content;
        public int owner;
        public bool isSolid;
        public long frameWhenEntered;
    }

    public Hash128 ComputeHash()
    {
        Hash128 h = new Hash128();
        if (PlayerPositions != null)
        {
            foreach (Vector2Int t in PlayerPositions)
            {
                h.Append(t.x);
                h.Append(t.y);
            }
        }

        if (CellStates != null)
        {
            foreach (var cs in CellStates)
            {
                h.Append(cs.x);
                h.Append(cs.y);
                h.Append(cs.frameWhenEntered);
                h.Append(cs.content);
                h.Append(cs.owner);
                h.Append(cs.isSolid ? 1 : 0);
            }
        }

        if (wordsUsed != null)
        {
            foreach (string w in wordsUsed)
            {
                h.Append(w);
            }
        }

        return h;
    }
}