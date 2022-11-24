using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string Content { get; private set;  }
    public uint Owner { get; private set; }
    public bool IsSolid { get; private set; }

    public Vector2Int gridPosition = new Vector2Int(40, 25);
    public Grid grid;
    public TMPro.TMP_Text textControl, debugText;
    public Renderer solidBackground, tentativeBackground;

    public Renderer tentativeTimerUI; 
    public long frameWhenEntered;
    
    public void Initialise(Grid grid, int x, int y)
    {
        this.grid = grid;
        gridPosition = new Vector2Int(x, y);
        
        transform.position = grid.GetGlobalPositionFromGrid(gridPosition.x, gridPosition.y);
        
        textControl.text = Content;
    }

    public void SetFrame(long frame)
    {
        frameWhenEntered = frame;
    }

    public void SetContent(string content)
    {
        this.Content = content;
        
        textControl.text = Content;
    }

    public void SetOwner(uint owner)
    {
        this.Owner = owner;
    }

    public void SetSolid(bool solid)
    {
        this.IsSolid = solid;
    }
    
    public void SetState(string content, uint owner, bool? isSolid = null)
    {
        this.Content = content;
        this.Owner = owner;
        
        if (isSolid != null)
        {
            this.IsSolid = (bool)isSolid;
        }
        
        textControl.text = Content;
    }

    private int frame = 0;
    
    void Update()
    {
        if (frame++ % 10 != 0) return;
        
        solidBackground.enabled = IsSolid && !string.IsNullOrEmpty(Content);
        tentativeBackground.enabled = !IsSolid && !string.IsNullOrEmpty(Content);
        tentativeTimerUI.enabled = !IsSolid && !string.IsNullOrEmpty(Content);
        
        tentativeTimerUI.transform.localScale = new Vector3(0.08f * (grid.MaxFramesForTempLetter-(grid.GetSimulationFrame() - frameWhenEntered))/grid.MaxFramesForTempLetter, tentativeTimerUI.transform.localScale.y,
            tentativeTimerUI.transform.localScale.z);
    }
}
