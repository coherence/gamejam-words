using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Grid : MonoBehaviour
{
    public const int tiling = 50;
    public Transform cursor;
    public int cursorPosX = 25;
    public int cursorPoxY = 25;
    public float LocalRadius = 5f;
    public float posAdjustment = -0.1f;

    public Transform TempObjectToTrack;

    public Cell[] cells;

    public Transform cellPrefab;
    
    Vector2Int GetGridPosFromLocal(float x, float z)
    {
        float t = (float) tiling;
        float adjustedX = (x + LocalRadius - posAdjustment) / LocalRadius;
        float adjustedY = (z + LocalRadius - posAdjustment) / LocalRadius;

        adjustedX *= 0.5f * tiling;
        adjustedY *= 0.5f * tiling;
        
        return new Vector2Int((int)adjustedX-1, (int)adjustedY-1);
    }

    public Vector3 GetGlobalPositionFromGrid(int x, int y)
    {
        x += 1;
        y += 1;
        float originX = -LocalRadius + posAdjustment;
        float originY = -LocalRadius + posAdjustment;
        float t = tiling;
        float d = 1f / t * LocalRadius * 2f;
        float localX = originX + (float) x * d;
        float localY = originY + (float) y * d;
        Vector3 localPos = new Vector3(localX, 0, localY);
        return transform.TransformPoint(localPos);
    }
    
    
    void Awake()
    {
        cells = new Cell[tiling * tiling];

        for (var y = 0; y < tiling; y++)
        {
            for (var x = 0; x < tiling; x++)
            {
                var cellGO = Instantiate(cellPrefab);
                var cell = cellGO.GetComponent<Cell>();
                cell.Initialise(this, x, y);
                cells[x + tiling * y] = cell;
                cellGO.parent = transform;
            }
        }
    }

    public void SetCellContent(int x, int y, string content, int owner)
    {
        var cell = cells[x + tiling * y];
        cell.SetState(content, owner);
    }
    
    public void SetSimulationState(SimulationState.Cell[] state)
    {
        for (var i = 0; i < tiling * tiling; i++)
        {
            cells[i].SetState(state[i].content, state[i].owner);
        }
    }
    
    public SimulationState.Cell[] GetSimulationState()
    {
        SimulationState.Cell[] cellStates = new SimulationState.Cell[tiling * tiling];
        for (var i = 0; i < tiling * tiling; i++)
        {
            cellStates[i] = new SimulationState.Cell()
            {
                content = cells[i].content,
                owner = cells[i].owner
            };
        }

        return cellStates;
    }

    IEnumerator TestProc()
    {
        while (true)
        {
            cursorPosX++;
            if (cursorPosX >= 50) cursorPosX = 0;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        return;
        
        if (TempObjectToTrack == null)
        {
            var player = GameObject.FindObjectOfType<Player>();
            if (player == null) return;
            TempObjectToTrack = player.transform;
            return;
        }

        TempObjectToTrack.position = GetGlobalPositionFromGrid(cursorPosX, cursorPoxY);
        
        var pos = transform.InverseTransformPoint(TempObjectToTrack.position);
        var gridPos = GetGridPosFromLocal(pos.x, pos.z);
        //Debug.Log($"Cursor pos: {cursorPosX},{cursorPoxY}, Grid position: {gridPos}");
    }
}
