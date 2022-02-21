using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Coherence.Toolkit;
using Coherence.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Network = Coherence.Network;

public class Grid : MonoBehaviour
{
    public bool allowCompletingEachOthersWords = true;
    
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
    public float playerScoreStartX = 13.4f;
    public float playerScoreStartY = 13f;
    public float playerScoreDistanceX = 170;
    public float playerScoreLastX = 30;

    private CoherenceClientConnection tempClientconnection;

    public Transform letterScoreUIParent;
    public Transform letterScoreUIPrefab;

    public SystemMessage systemMessage;

    public long startFrame = 0;
    
    public bool IsPaused
    {
        get;
        private set;
    }
    
    public void TogglePaused(bool p)
    {
        IsPaused = p;
        
        systemMessage.DisplayMessage(p? "PAUSED" : null);
    }
    
    //qwertyuiopasdfghjklzxcvbnm
    //https://www3.nd.edu/~busiforc/handouts/cryptography/letterfrequencies.html
    float[] letterFrequencies = new float[]
    {
        0.1962f,    //q
        1.2899f,    //w
        11.1607f,   //e
        7.5809f,    //r
        6.9509f,    //t
        1.7779f,    //y
        3.6308f,    //u
        7.5448f,    //i
        7.1635f,    //o
        3.1671f,    //p
        8.4966f,    //a
        5.7351f,    //s
        3.3844f,    //d
        1.8121f,    //f
        2.4705f,    //g
        3.0034f,    //h
        0.1965f,    //j
        1.1016f,    //k
        5.4893f,    //l
        0.2722f,    //z
        0.2902f,    //x
        4.5388f,    //c
        1.0074f,    //v
        2.0720f,    //b
        6.6544f,    //n
        3.0129f,    //m
    };

    int GetLetterScoreFromFrequency(float freq)
    {
        int score = Mathf.CeilToInt(1 / freq * 10f);

        if (score > 16)
        {
            score = 16;
        }
        
        return score;
    }
    
    public class PlayerData
    {
        public CoherenceClientConnection clientConnection;
        public CoherenceSync sync;
        public int clientID;
        public Player player;
        public PlayerDataUI ui;
        
        public void Update()
        {
            ui.playerName.text = player.playerName;
            ui.playerScore.text = player.score.ToString();
        }
    }

    private Hashtable players;

    void UpdatePlayerData()
    {
        foreach (DictionaryEntry s in players)
        {
            var player = (PlayerData)s.Value;
            player.Update();
        }
    }
    
    public void AddPlayer(CoherenceClientConnection client)
    {
        tempClientconnection = client;
        
        var uigo = CreatePlayerScoreUIElement();

        var player = new PlayerData()
        {
            clientConnection = client,
            sync = client.Sync,
            clientID = client.ClientId,
            ui = uigo,
            player = client.Sync.GetComponent<Player>()
        };

        players[client.ClientId] = player;
        
        UpdatePlayerUIControls();
    }

    private PlayerDataUI CreatePlayerScoreUIElement()
    {
        var uigo = Instantiate(playerScoreUIPrefab);
        uigo.SetParent(playerScoreParent.transform);
        uigo.transform.position = new Vector3(playerScoreLastX, playerScoreStartY, 0);
        playerScoreLastX += playerScoreDistanceX;
        return uigo.GetComponent<PlayerDataUI>();
    }

    public void RemovePlayer(CoherenceClientConnection client)
    {
        var player = (PlayerData) players[client.ClientId];
        Destroy(player.ui.gameObject);
        players.Remove(client.ClientId);
        UpdatePlayerUIControls();
    }
    
    public class PlayerDataComparer : IComparer
    {
        public int Compare(object _x, object _y)
        {
            var a = (PlayerData) _x;
            var b = (PlayerData) _y;
            return b.player.score.CompareTo(a.player.score);
        }
    }

    void UpdatePlayerUIControls()
    {
        playerScoreLastX = playerScoreStartX;

        ArrayList order = new ArrayList();

        foreach (DictionaryEntry s in players)
        {
            order.Add((PlayerData) s.Value);
        }
        PlayerDataComparer comparer = new PlayerDataComparer();
        order.Sort(comparer);

        foreach (Transform tr in playerScoreParent)
        {
            Destroy(tr.gameObject);
        }
        
        foreach (PlayerData player in order)
        {
            player.ui = CreatePlayerScoreUIElement();
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
        IsPaused = true;
        systemMessage.DisplayMessage(null);
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
        
        var alphabet = Player.SupportedAlphabet;
        int foundLetter = -1;

        float startX = -13.4f;
        float startY = -8f;
        
        for (int i = 0; i < alphabet.Length; i++)
        {
            var go = Instantiate(letterScoreUIPrefab);
            go.SetParent(letterScoreUIParent.transform);
            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(startX, startY);
            
            float freq = letterFrequencies[i];
            
            var ui = go.GetComponent<LetterScoreUI>();
            ui.letter.text = alphabet[i].ToString().ToUpper();
            ui.score.text = GetLetterScoreFromFrequency(freq).ToString();
            
            startY -= 31f;
        }
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

        score += EvaluateWordInDirection(x, y, 1, 0, clientID, allowCompletingEachOthersWords);
        score += EvaluateWordInDirection(x, y, 0, -1, clientID, allowCompletingEachOthersWords);

        if (score > 0)
        {
            AddPlayerScore(clientID, score);
        }
    }

    private void AddPlayerScore(int clientID, int score)
    {
        var player = (PlayerData) players[clientID];
        player.player.score += score;
        player.Update();
        UpdatePlayerUIControls();
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
        var alphabet = Player.SupportedAlphabet;
        int foundLetter = -1;
        letter = letter.ToLower();

        for (int i = 0; i < alphabet.Length; i++)
        {
            if (alphabet[i] == letter[0])
            {
                foundLetter = i;
                break;
            }
        }

        if (foundLetter == -1) return 0;

        float freq = letterFrequencies[foundLetter];
        
        return GetLetterScoreFromFrequency(freq);
    }

    int GetWordBeginningX(int x, int y, int clientID, bool allowCompleteOthers)
    {
        x--;
        
        int beginning = x;

        Cell cell = null;
        
        while ((cell = GetCellAtXY(x, y)) != null)
        {
            if (!allowCompleteOthers)
            {
                if (!cell.IsSolid && cell.Owner != clientID) break;
            }

            x--;
        }

        beginning = x+1;

        return beginning;
    }
    
    int GetWordBeginningY(int x, int y, int clientID, bool allowCompleteOthers)
    {
        y++;
        
        int beginning = y;

        Cell cell = null;
        
        while ((cell = GetCellAtXY(x, y)) != null)
        {
            if (!allowCompleteOthers)
            {
                if (!cell.IsSolid && cell.Owner != clientID) break;
            }

            y++;
        }

        beginning = y-1;

        return beginning;
    }
    
    int EvaluateWordInDirection(int ox, int oy, int xdir, int ydir, int clientID, bool allowCompleteOthers)
    {
        int score = 0;
        string content = "";

        bool horizontal = xdir != 0;
        bool vertical = !horizontal;
        
        if (xdir != 0)
        {
            ox = GetWordBeginningX(ox, oy, clientID, allowCompleteOthers);
        }

        if (ydir != 0)
        {
            oy = GetWordBeginningY(ox, oy, clientID, allowCompleteOthers);
        }
        
        int x = ox;
        int y = oy;
        
        content = GetWordInDirection(xdir, ydir, x, y, clientID, allowCompleteOthers);

        if (DoesWordExistInDictionaryAndIsUnused(content))
        {
            // Do cross checks as well

            var crossChecksDetectedIssues = PerformCrossChecks(ox, oy, xdir, ydir, clientID, content, horizontal, allowCompleteOthers);

            if (crossChecksDetectedIssues) return 0;
            
            x = ox;
            y = oy;
            
            Debug.Log($"Word {content} found!");

            wordsAlreadyUsed.Add(content);
            
            score = EvaluateAddedWord(xdir, ydir, clientID, content, x, y);

            return score;
        }
        
        return score;
    }

    private int EvaluateAddedWord(int xdir, int ydir, int clientID, string content, int x, int y)
    {
        int score = 0;
        
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

        return score;
    }

    private bool PerformCrossChecks(int ox, int oy, int xdir, int ydir, int clientID, string content, bool horizontal, bool allowCompleteOthers)
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
                    int beginningY = GetWordBeginningY(x, y, clientID, false);
                    var w = GetWordInDirection(0, -1, x, beginningY, clientID, false);

                    //Debug.Log($"Checking vertically as well...{beginningY} {w} {w.Length}");

                    if (w.Length > 1 && !DoesWordExistInDictionaryAndIsUnused(w))
                    {
                        crossChecksDetectedIssues = true;
                        ClearNonSolidLettersInDirection(0, -1, x, beginningY, clientID);
                        ClearNonSolidLettersInDirection(1, 0, ox, oy, clientID);
                        //Debug.Log($"Failed vertical xcheck on letter {cell.Content}");
                        //break;
                    }
                }
                else
                {
                    int beginningX = GetWordBeginningX(x, y, clientID, false);
                    var w = GetWordInDirection(1, 0, beginningX, y, clientID, false);

                    //Debug.Log($"Checking horizontally as well...{beginningX} {w} {w.Length}");

                    if (w.Length > 1 && !DoesWordExistInDictionaryAndIsUnused(w))
                    {
                        crossChecksDetectedIssues = true;
                        ClearNonSolidLettersInDirection(1, 0, beginningX, y, clientID);
                        ClearNonSolidLettersInDirection(0, -1, ox, oy, clientID);
                        //Debug.Log($"Failed horizontal xcheck on letter {cell.Content}");
                        //break;
                    }
                }
            }

            x += xdir;
            y += ydir;
        }

        return crossChecksDetectedIssues;
    }

    
    
    private string GetWordInDirection(int xdir, int ydir, int x, int y, int clientID, bool allowCompleteOthers)
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

            if (!allowCompleteOthers)
            {
                if (!currentCell.IsSolid && currentCell.Owner != clientID) break;
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

    public void SetSimulationState(SimulationState state)
    {
        var c = state.CellStates;
        
        for (var i = 0; i < tiling * tiling; i++)
        {
            cells[i].SetState(c[i].content, c[i].owner, c[i].isSolid);
            cells[i].SetFrame(c[i].frameWhenEntered);
        }

        wordsAlreadyUsed.Clear();
        foreach (var w in state.wordsUsed)
        {
            wordsAlreadyUsed.Add(w);
        }
        
        foreach (DictionaryEntry s in players)
        {
            var pl = (PlayerData)s.Value;
            pl.player.score = (int) state.PlayerScores[pl.clientID];
        }
    }
    
    public SimulationState.Cell[] GetSimulationState(out ArrayList wordsUsed, out Hashtable playerScores)
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

        wordsUsed = new ArrayList();
        foreach (var w in wordsAlreadyUsed)
        {
            wordsUsed.Add(w);
        }

        playerScores = new Hashtable();

        foreach (DictionaryEntry s in players)
        {
            playerScores[(int) ((PlayerData)s.Value).clientID] = ((PlayerData) s.Value).player.score;
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
        playerName = networkDialog.nameInput.text;
        UpdatePlayerData();

        if (IsPaused && Network.IsConnected)
        {
            if (startFrame != 0)
            {
                if (currentSimulationFrame-startFrame > 60)
                {
                    systemMessage.DisplayMessage("Sorry, game already started. Please use another room.");
                }
                else
                {
                    if (currentSimulationFrame > startFrame)
                    {
                        IsPaused = false;
                        systemMessage.DisplayMessage(null);
                    }
                    else
                    {
                        systemMessage.DisplayMessage($"Starting in... {startFrame-currentSimulationFrame}");
                    }
                }
            }
            else
            {
                systemMessage.DisplayMessage("Waiting for the game to start [one player should press SPACE when ready]");
            }
        }
        else
        {
            systemMessage.DisplayMessage(null);
        }
        
        systemMessage.UpdateFrame(currentSimulationFrame);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsPaused && startFrame == 0)
            {
                var pr = FindObjectsOfType<Player>();

                foreach (var p in pr)
                {
                    p.startOnFrame = currentSimulationFrame + 350;
                }
            }
        }
    }
}
