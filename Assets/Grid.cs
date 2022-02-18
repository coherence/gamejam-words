using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Coherence.Toolkit;
using Coherence.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Network = Coherence.Network;

public class Grid : MonoBehaviour
{
    public const int tiling = 50;
    public const int minWordLength = 4;
    public Transform cursor;
    public int cursorPosX = 25;
    public int cursorPoxY = 25;
    public float LocalRadius = 5f;
    public float posAdjustment = -0.1f;

    public string playerName = "";

    public long MaxFramesForTempLetter = 380;
    
    public Transform TempObjectToTrack;

    public Cell[] cells;

    public Transform cellPrefab;

    private ArrayList wordDictionary, wordsAlreadyUsed;

    private long currentSimulationFrame = 0;

    public NetworkDialog networkDialog;

    public Transform playerScoreParent;
    public Transform playerScoreUIPrefab;
    public float playerScoreStartX = 30;
    public float playerScoreStartY = 30;
    public float playerScoreDistanceX = 170;
    public float playerScoreLastX = 30;

    private CoherenceClientConnection tempClientconnection;
    
    class PlayerData
    {
        public CoherenceClientConnection clientConnection;
        public CoherenceSync sync;
        public string name;
        public int score;
        public int clientID;
        public PlayerDataUI ui;
        
        public void Update()
        {
            ui.playerName.text = name;
            ui.playerScore.text = score.ToString();
        }
    }

    private Hashtable players;

    void UpdatePlayerData()
    {
        foreach (DictionaryEntry s in players)
        {
            var player = (PlayerData)s.Value;
            player.name = player.sync.GetComponent<Player>().playerName;
            player.Update();
        }
    }
    
    public void AddPlayer(CoherenceClientConnection client)
    {
        tempClientconnection = client;
        
        var uigo = Instantiate(playerScoreUIPrefab);
        uigo.transform.parent = playerScoreParent.transform;
        uigo.transform.position = new Vector3(playerScoreLastX, playerScoreStartY, 0);
        
        playerScoreLastX += playerScoreDistanceX;
        
        var player = new PlayerData()
        {
            clientConnection = client,
            sync = client.Sync,
            name = "",
            score = 0,
            clientID = client.ClientId,
            ui = uigo.GetComponent<PlayerDataUI>()
        };

        players[client.ClientId] = player;
        
        UpdatePlayerUIControls();
    }

    public void RemovePlayer(CoherenceClientConnection client)
    {
        var player = (PlayerData) players[client.ClientId];
        Destroy(player.ui.gameObject);
        players.Remove(client.ClientId);
        UpdatePlayerUIControls();
    }

    void UpdatePlayerUIControls()
    {
        playerScoreLastX = playerScoreStartX;
        
        foreach (DictionaryEntry s in players)
        {
            var player = (PlayerData) s.Value;

            player.ui.transform.position = new Vector3(playerScoreLastX, playerScoreStartY, 0);
            playerScoreLastX += playerScoreDistanceX;
        }
    }

    private void LoadDictionary()
    {
        wordsAlreadyUsed = new ArrayList();
        wordDictionary = new ArrayList();
        
        TextAsset mytxtData=(TextAsset)Resources.Load("words_alpha");
        string txt=mytxtData.text;

        string[] lines = txt.Split('\n');
        
        foreach(var line in lines)    
        {
            var trimLine = line.Trim();
            if (trimLine.Length >= 4)
            {
                wordDictionary.Add(trimLine);
            }
        }

        var contains = wordDictionary.Contains("make");
        Debug.Log($"Dictionary test: make {contains}");
    }

    public long GetSimulationFrame()
    {
        return currentSimulationFrame;
    }
    
    public void UpdateSimulationFrame(long frame)
    {
        currentSimulationFrame = frame;

        foreach (var cell in cells)
        {
            if (!cell.IsSolid && !string.IsNullOrEmpty(cell.Content))
            {
                long age = currentSimulationFrame - cell.frameWhenEntered;

                if (age > MaxFramesForTempLetter)
                {
                    cell.SetState(null, -1, false);
                }
            }
        }
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

        players = new Hashtable();
        
        LoadDictionary();

        Network.OnConnected += () =>
        {
            playerName = networkDialog.nameInput.text;
        };
    }

    public bool IsWordInDictionary(string word)
    {
        return wordDictionary.Contains(word);
    }

    public void UpdatePlayerPositionAndClearNonSolidCells(int clientID, int xp, int yp)
    {
        for (var y = 0; y < tiling; y++)
        {
            for (var x = 0; x < tiling; x++)
            {
                var cell = GetCellAtXY(x, y);

                if (cell.Owner == clientID && !cell.IsSolid)
                {
                    var diffX = xp - x;
                    var diffY = yp - y;

                    if (Mathf.Abs(diffX) > 2 || Mathf.Abs(diffY) > 2)
                    {
                        cell.SetContent(null);
                        cell.SetOwner(-1);
                    }
                }
            }
        }
    }
    
    public void SetCellContentAndCheckWord(int x, int y, string content, int clientID, long frameID)
    {
        var cell = GetCellAtXY(x, y);

        if (cell.IsSolid) return; // a word already exists intersecting this cell
        
        cell.SetState(content, clientID, false);
        cell.SetFrame(frameID);

        int score = 0;

        score += EvaluateWordInDirection(x, y, 1, 0, clientID);
        score += EvaluateWordInDirection(x, y, 0, -1, clientID);

        if (score > 0)
        {
            var player = (PlayerData)players[clientID];
            player.score += score;
            player.Update();
        }
    }

    private Cell GetCellAtXY(int x, int y)
    {
        if (x < 0) return null;
        if (y < 0) return null;
        if (x >= tiling) return null;
        if (y >= tiling) return null;
        return cells[x + tiling * y];
    }

    int GetLetterScore(string letter)
    {
        // TODO evaluate letters
        return 1;
    }

    int GetWordBeginningX(int x, int y, int clientID)
    {
        x--;
        
        int beginning = x;

        Cell cell = null;
        
        while ((cell = GetCellAtXY(x, y)) != null)
        {
            if (!cell.IsSolid && cell.Owner != clientID) break;
            x--;
        }

        beginning = x+1;

        return beginning;
    }
    
    int GetWordBeginningY(int x, int y, int clientID)
    {
        y++;
        
        int beginning = y;

        Cell cell = null;
        
        while ((cell = GetCellAtXY(x, y)) != null)
        {
            if (!cell.IsSolid && cell.Owner != clientID) break;
            y++;
        }

        beginning = y-1;

        return beginning;
    }
    
    int EvaluateWordInDirection(int ox, int oy, int xdir, int ydir, int clientID)
    {
        int score = 0;
        string content = "";

        bool horizontal = xdir != 0;
        bool vertical = !horizontal;
        
        if (xdir != 0)
        {
            ox = GetWordBeginningX(ox, oy, clientID);
        }

        if (ydir != 0)
        {
            oy = GetWordBeginningY(ox, oy, clientID);
        }
        
        int x = ox;
        int y = oy;
        
        content = GetWordInDirection(xdir, ydir, x, y, clientID);

        if (DoesWordExistInDictionaryAndIsUnused(content))
        {
            // Do cross checks as well

            var crossChecksDetectedIssues = PerformCrossChecks(ox, oy, xdir, ydir, clientID, content, horizontal);

            if (crossChecksDetectedIssues) return 0;
            
            x = ox;
            y = oy;
            
            Debug.Log($"Word {content} found!");

            wordsAlreadyUsed.Add(content);
            
            EvaluateAddedWord(xdir, ydir, clientID, content, x, y, score);
            
            return content.Length; // TODO: calculate score based on letter weights
        }
        
        return score;
    }

    private void EvaluateAddedWord(int xdir, int ydir, int clientID, string content, int x, int y, int score)
    {
        for (int i = 0; i < content.Length; i++)
        {
            var cell = GetCellAtXY(x, y);

            if (cell.Owner != -1) cell.SetOwner(clientID);

            if (cell.IsSolid)
            {
            }
            else
            {
                score += GetLetterScore(cell.Content);
                cell.SetSolid(true);
            }

            x += xdir;
            y += ydir;
        }
    }

    private bool PerformCrossChecks(int ox, int oy, int xdir, int ydir, int clientID, string content, bool horizontal)
    {
        int x;
        int y;
        bool crossChecksDetectedIssues = false;

        x = ox;
        y = oy;

        for (int i = 0; i < content.Length; i++)
        {
            var cell = GetCellAtXY(x, y);

            if (cell.IsSolid)
            {
            }
            else
            {
                // check cross connections - are we creating non-existing words somewhere?
                if (horizontal)
                {
                    int beginningY = GetWordBeginningY(x, y, clientID);
                    var w = GetWordInDirection(0, -1, x, beginningY, clientID);

                    Debug.Log($"Checking vertically as well...{beginningY} {w} {w.Length}");

                    if (w.Length > 1 && !DoesWordExistInDictionaryAndIsUnused(w))
                    {
                        crossChecksDetectedIssues = true;
                        ClearNonSolidLettersInDirection(0, -1, x, beginningY, clientID);
                        ClearNonSolidLettersInDirection(1, 0, ox, oy, clientID);
                        Debug.Log($"Failed vertical xcheck on letter {cell.Content}");
                        //break;
                    }
                }
                else
                {
                    int beginningX = GetWordBeginningX(x, y, clientID);
                    var w = GetWordInDirection(1, 0, beginningX, y, clientID);

                    Debug.Log($"Checking horizontally as well...{beginningX} {w} {w.Length}");

                    if (w.Length > 1 && !DoesWordExistInDictionaryAndIsUnused(w))
                    {
                        crossChecksDetectedIssues = true;
                        ClearNonSolidLettersInDirection(1, 0, beginningX, y, clientID);
                        ClearNonSolidLettersInDirection(0, -1, ox, oy, clientID);
                        Debug.Log($"Failed horizontal xcheck on letter {cell.Content}");
                        //break;
                    }
                }
            }

            x += xdir;
            y += ydir;
        }

        return crossChecksDetectedIssues;
    }

    private string GetWordInDirection(int xdir, int ydir, int x, int y, int clientID)
    {
        string content = "";
        
        var currentCell = GetCellAtXY(x, y);

        while (currentCell != null)
        {
            var character = currentCell.Content;
            if (string.IsNullOrEmpty(character))
            {
                break;
            }

            if (!currentCell.IsSolid && currentCell.Owner != clientID) break;

            content += character.ToLower();
            x += xdir;
            y += ydir;

            if (x < 0) break;
            if (y < 0) break;
            if (x >= tiling) break;
            if (y >= tiling) break;

            currentCell = GetCellAtXY(x, y);
        }

        return content;
    }
    
    private void ClearNonSolidLettersInDirection(int xdir, int ydir, int x, int y, int clientID)
    {
        var currentCell = GetCellAtXY(x, y);
        
        while (currentCell != null)
        {
            if (!currentCell.IsSolid)
            {
                Debug.Log($"Clearing letter {currentCell.Content}");
                currentCell.SetState(null, -1);
            }
            
            var character = currentCell.Content;
            if (string.IsNullOrEmpty(character))
            {
                break;
            }

            x += xdir;
            y += ydir;

            if (x < 0) break;
            if (y < 0) break;
            if (x >= tiling) break;
            if (y >= tiling) break;

            currentCell = GetCellAtXY(x, y);
        }
    }

    bool DoesWordExistInDictionaryAndIsUnused(string content)
    {
        return wordDictionary.Contains(content) && !wordsAlreadyUsed.Contains(content);
    }
    
    public static string Reverse( string s )
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse( charArray );
        return new string( charArray );
    }

    public void SetSimulationState(SimulationState.Cell[] state)
    {
        for (var i = 0; i < tiling * tiling; i++)
        {
            cells[i].SetState(state[i].content, state[i].owner, state[i].isSolid);
            cells[i].SetFrame(state[i].frameWhenEntered);
        }
    }
    
    public SimulationState.Cell[] GetSimulationState()
    {
        SimulationState.Cell[] cellStates = new SimulationState.Cell[tiling * tiling];
        for (var i = 0; i < tiling * tiling; i++)
        {
            cellStates[i] = new SimulationState.Cell()
            {
                content = cells[i].Content,
                owner = cells[i].Owner,
                isSolid = cells[i].IsSolid,
                frameWhenEntered = cells[i].frameWhenEntered
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
        UpdatePlayerData();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddPlayer(tempClientconnection);
        }
    }
}
