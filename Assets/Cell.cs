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
    
    public void Initialise(Grid grid, int x, int y)
    {
        this.grid = grid;
        gridPosition = new Vector2Int(x, y);
    }

    public void SetState(string content, int owner)
    {
        this.content = content;
        this.owner = owner;
    }
    
    void Update()
    {
        transform.position = grid.GetGlobalPositionFromGrid(gridPosition.x, gridPosition.y);
        textControl.text = content;
    }
}
