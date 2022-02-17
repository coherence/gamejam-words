using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string Content { get; private set;  }
    public int Owner { get; private set; }
    public bool IsSolid { get; private set; }

    public Vector2Int gridPosition = new Vector2Int(25, 25);
    public Grid grid;
    public TMPro.TMP_Text textControl, debugText;
    public Renderer solidBackground, tentativeBackground;
    
    public long frameWhenEntered;
    
    public void Initialise(Grid grid, int x, int y)
    {
        this.grid = grid;
        gridPosition = new Vector2Int(x, y);
    }

    public void SetFrame(long frame)
    {
        frameWhenEntered = frame;
    }

    public void SetContent(string content)
    {
        this.Content = content;
    }

    public void SetOwner(int owner)
    {
        this.Owner = owner;
    }

    public void SetSolid(bool solid)
    {
        this.IsSolid = solid;
    }
    
    public void SetState(string content, int owner, bool? isSolid = null)
    {
        this.Content = content;
        this.Owner = owner;
        
        if (isSolid != null)
        {
            this.IsSolid = (bool)isSolid;
        }
    }
    
    void Update()
    {
        transform.position = grid.GetGlobalPositionFromGrid(gridPosition.x, gridPosition.y);
        textControl.text = Content;
        solidBackground.enabled = IsSolid && !string.IsNullOrEmpty(Content);
        tentativeBackground.enabled = !IsSolid && !string.IsNullOrEmpty(Content);

        debugText.text = (!IsSolid && !string.IsNullOrEmpty(Content)) ? $"{grid.MaxFramesForTempLetter - (grid.GetSimulationFrame()-frameWhenEntered)}" : "";
    }
}
