using System.Collections;
using Coherence.Toolkit;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CoherenceSync))]
[RequireComponent(typeof(CoherenceInput))]
public class Player : MonoBehaviour
{
    private FixedUpdateInput fixedUpdateInput;

    public string playerName = "Player";
    
    public Vector2Int gridPosition = new Vector2Int(25, 25);

    public int score = 0;
    public int clientID = 0;
    
    private CoherenceInput input;
    private CoherenceSync sync;
    private Grid grid;

    public float inputSamplingDelay = 0.10f;

    public const string SupportedAlphabet = "qwertyuiopasdfghjklzxcvbnm";

    private Hashtable keyPresses;
    
    private KeyCode[] acceptedAlphabetKeys;

    public Renderer cursor, cursorMine;
    public long startOnFrame = 0;
    
    ArrayList tmpKeyRecorder = new ArrayList();
    
    KeyCode[] demoKeys = new KeyCode[] {KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.D,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.J,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.Y,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.M,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.A,KeyCode.S,KeyCode.RightArrow,KeyCode.A,KeyCode.LeftArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.G,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.O,KeyCode.RightArrow,KeyCode.W,KeyCode.RightArrow,KeyCode.J,KeyCode.K,KeyCode.L,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.R,KeyCode.DownArrow,KeyCode.O,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.G,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.U,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.S,KeyCode.RightArrow,KeyCode.O,KeyCode.RightArrow,KeyCode.M,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.H,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.W,KeyCode.O,KeyCode.DownArrow,KeyCode.W,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.E,KeyCode.UpArrow,KeyCode.M,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.C,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.K,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.C,KeyCode.DownArrow,KeyCode.H,KeyCode.DownArrow,KeyCode.O,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.O,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.Y,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.S,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.G,KeyCode.RightArrow,KeyCode.N,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.V,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.Y,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.C,KeyCode.RightArrow,KeyCode.Q,KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.K,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.LeftArrow,};
    private int demoKeysCurrentIndex = 0;
    private bool demoKeysReplaying = false;
    private float lastDemoKeyTime = 0;
    public float timeBetweenDemoKeys = 0.2f;

    private bool IsNextDemoKey(KeyCode key)
    {
        if (Time.time - lastDemoKeyTime < timeBetweenDemoKeys) return false;
        
        var demokey = GetCurrentDemoKey(false);

        return demoKeysReplaying && demokey != null && demokey == key;
    }
    
    private KeyCode? GetCurrentDemoKey(bool advance = true)
    {
        if (demoKeysCurrentIndex < 0 || demoKeysCurrentIndex >= demoKeys.Length)
        {
            demoKeysReplaying = false;
            return null;
        }

        var ret = demoKeys[demoKeysCurrentIndex];
        if (advance)
        {
            demoKeysCurrentIndex++;
            lastDemoKeyTime = Time.time;
        }
        return ret;
    }
    
    private void Awake()
    {
        sync = GetComponent<CoherenceSync>();
        
        fixedUpdateInput = sync.MonoBridge.FixedUpdateInput;
        input = sync.Input;
        grid = FindObjectOfType<Grid>();

        keyPresses = new Hashtable {[KeyCode.UpArrow] = 0f, [KeyCode.DownArrow] = 0f, [KeyCode.LeftArrow] = 0f, [KeyCode.RightArrow] = 0f};

        InitAlphabet();
    }
    
    private void InitAlphabet()
    {
        acceptedAlphabetKeys = new KeyCode[SupportedAlphabet.Length];
        for (var i = 0; i < SupportedAlphabet.Length; i++)
        {
            var key = (KeyCode) System.Enum.Parse(typeof(KeyCode), SupportedAlphabet[i].ToString().ToUpper());
            acceptedAlphabetKeys[i] = key;
            keyPresses[key] = 0f;
        }
    }

    private bool CanTypeKey(KeyCode key)
    {
        var keyTime = keyPresses[key];
        if (keyTime == null) return true;
        return Time.time - (float) keyTime > inputSamplingDelay;
    }

    private void RecordKeyTime(KeyCode key)
    {
        keyPresses[key] = Time.time;
    }

    void Update()
    {
        transform.position = grid.GetGlobalPositionFromGrid(gridPosition.x, gridPosition.y);

        if (sync.isSimulated)
        {
            playerName = grid.playerName;

            cursorMine.enabled = true;
            cursor.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            string output = "KeyCode[] demoKeys = new KeyCode[] {";

            foreach (var code in tmpKeyRecorder)
            {
                output += $"KeyCode.{code.ToString()},";
            }

            output += "};";

            Debug.Log(output);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            demoKeysCurrentIndex = 0;
            demoKeysReplaying = true;
        }
    }

    public Vector2 GetNetworkInputMovement(long frame)
    {
        return input.GetAxisState("Mov", frame);
    }

    public float GetNetworkInputString(long frame)
    {
        return input.GetButtonRangeState("key", frame);
    }

    public void ApplyLocalInputs()
    {
        ApplyMovement();
        ApplyAlphabet();
    }

    private void ApplyAlphabet()
    {
        bool canType = false;
        
        foreach (var key in acceptedAlphabetKeys)
        {
            if (demoKeysReplaying)
            {
                if (IsNextDemoKey(key))
                {
                    if (CanTypeKey(key))
                    {
                        input.SetButtonRangeState("key", (float)key);
                        GetCurrentDemoKey(true); // just advance
                        canType = true;
                        RecordKeyTime(key);
                    }
                    else
                    {
                       //TG input.SetButtonRangeState("key", 0f);
                    }
                }
            }
            else
            if (fixedUpdateInput.GetKeyDown(key))
            {
                if (CanTypeKey(key))
                {
                    input.SetButtonRangeState("key", (float)key);
                    RecordKeyTime(key);
                    canType = true;
                    tmpKeyRecorder.Add(key);
                }
                else
                {
                   //TG input.SetButtonRangeState("key", 0f);
                }
            }
        }

        if (!canType)
        {
            //TG input.SetButtonRangeState("key", 0f);
        }
    }

    void TryTypeKey(KeyCode key, int x0, int y0, ref int x, ref int y)
    {
        if (demoKeysReplaying)
        {
            if (IsNextDemoKey(key))
            {
                if (CanTypeKey(key))
                {
                    x = x0;
                    y = y0;
                    GetCurrentDemoKey(true); // just advance
                    RecordKeyTime(key);
                }
                else
                {
                    if (x0 != 0) x = 0;
                    if (y0 != 0) y = 0;
                }
            }
        }
        else
        if (fixedUpdateInput.GetKeyDown(key)) 
        {
            if(CanTypeKey(key))
            {
                x = x0;
                y = y0;
                RecordKeyTime(key);
                
                tmpKeyRecorder.Add(key);
            }
            else
            {
                if (x0 != 0) x = 0;
                if (y0 != 0) y = 0;
            }
        }
    }
    
    private void ApplyMovement()
    {
        int y = 0;
        int x = 0;

        TryTypeKey(KeyCode.UpArrow, 0, 1, ref x, ref y);
        TryTypeKey(KeyCode.DownArrow, 0, -1, ref x, ref y);
        TryTypeKey(KeyCode.RightArrow, 1, 0, ref x, ref y);
        TryTypeKey(KeyCode.LeftArrow, -1, 0, ref x, ref y);
        
        var movement = new Vector2(x, y).normalized;
        input.SetAxisState("Mov", movement);
    }
}