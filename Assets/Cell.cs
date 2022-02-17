using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string content;
    public int owner;
    public Vector2Int gridPosition = new Vector2Int(25, 25);
    public Grid grid;
    public TMPro.TMP_Text textControl;
    public Renderer solidBackground, tentativeBackground;
    public bool isSolid = false;
    
    public void Initialise(Grid grid, int x, int y)
    {
        this.grid = grid;
        gridPosition = new Vector2Int(x, y);
        isSolid = false;
        content = null;
    }

    public void SetSolidState(bool isSolid)
    {
        this.isSolid = isSolid;
    }

    public void SetState(string content, int owner, bool? isSolid = null)
    {
        this.content = content;
        this.owner = owner;
        
        if (isSolid != null)
        {
            this.isSolid = (bool)isSolid;
        }
    }
    
    void Update()
    {
        transform.position = grid.GetGlobalPositionFromGrid(gridPosition.x, gridPosition.y);
        textControl.text = content;
        solidBackground.enabled = isSolid && !string.IsNullOrEmpty(content);
        tentativeBackground.enabled = !isSolid && !string.IsNullOrEmpty(content);
    }
}
