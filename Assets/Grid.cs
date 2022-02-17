using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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

    private ArrayList wordDictionary;

    private void LoadDictionary()
    {
        wordDictionary = new ArrayList();
        using (StreamReader reader = new StreamReader("Assets/Resources/words_alpha.txt"))
        {
            var line = reader.ReadLine().Trim();

            if (line.Length > 2)
            {
                wordDictionary.Add(line);
                Debug.Log(line);
            }
        }

        var contains = wordDictionary.Contains("make");
        Debug.Log($"Dictionary test: make {contains}");
    }
    
    private Vector2Int GetGridPosFromLocal(float x, float z)
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

        LoadDictionary();
    }

    public bool IsWordInDictionary(string word)
    {
        return wordDictionary.Contains(word);
    }

    public void SetCellContentAndCheckWord(int x, int y, string content, int owner)
    {
        var cell = GetCellAtXY(x, y);

        if (cell.isSolid) return; // a word already exists intersecting this cell
        
        cell.SetState(content, owner, false);

        int score = 0;

        score += EvaluateWordInDirection(x, y, 1, 0, owner);
        score += EvaluateWordInDirection(x, y, -1, 0, owner);
        score += EvaluateWordInDirection(x, y, 0, 1, owner);
        score += EvaluateWordInDirection(x, y, 0, -1, owner);

        if (score > 0)
        {
            Debug.Log($"TODO: GIVE Player {owner} score of {score}");
        }
    }

    private Cell GetCellAtXY(int x, int y)
    {
        return cells[x + tiling * y];
    }
    
    int EvaluateWordInDirection(int ox, int oy, int xdir, int ydir, int clientID)
    {
        int score = 0;
        string content = "";
        int x = ox;
        int y = oy;
        
        var currentCell = GetCellAtXY(x, y);
        
        while (currentCell != null)
        {
            var character = currentCell.content;
            if (string.IsNullOrEmpty(character))
            {
                break;
            }

            content += character.ToLower();
            x += xdir;
            y += ydir;

            if (x < 0) break;
            if (y < 0) break;
            if (x >= tiling) break;
            if (y >= tiling) break;
            
            currentCell = GetCellAtXY(x, y);
        }

        var reversedContent = Reverse(content);
        
        Debug.Log($"Checking word {content} and {reversedContent}...");
        
        if (wordDictionary.Contains(reversedContent) || wordDictionary.Contains(content))
        {
            x = ox;
            y = oy;
            
            for (int i = 0; i < content.Length; i++)
            {
                var cell = GetCellAtXY(x, y);
                
                if(cell.owner != -1) cell.owner = clientID;

                cell.isSolid = true;
                
                x += xdir;
                y += ydir;
            }
            
            return content.Length; // TODO: calculate score based on letter weights
        }
        else
        {
            Debug.Log("Not found");
        }

        return score;
    }
    
    public static string Reverse( string s )
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse( charArray );
        return new string( charArray );
    }

    public void SetCellSolidState(int x, int y, bool isSolid)
    {
        var cell = cells[x + tiling * y];
        cell.SetSolidState(isSolid);
    }
    
    public void SetSimulationState(SimulationState.Cell[] state)
    {
        for (var i = 0; i < tiling * tiling; i++)
        {
            cells[i].SetState(state[i].content, state[i].owner, state[i].isSolid);
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
                owner = cells[i].owner,
                isSolid = cells[i].isSolid
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
