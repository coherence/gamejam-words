using System;
using System.Collections;
using Coherence.Toolkit;
using UnityEngine;

public struct DemoKey
{
    public KeyCode Key;
    public long Frame;

    public DemoKey(KeyCode key, long frame)
    {
        Key = key;
        Frame = frame;
    }
}

[RequireComponent(typeof(CoherenceSync))]
[RequireComponent(typeof(CoherenceInput))]
public class Player : MonoBehaviour
{
    private FixedUpdateInput fixedUpdateInput;

    public string playerName = "Player";
    
    public Vector2Int gridPosition = new Vector2Int(40, 25);

    public int score;
    public int clientID;
    
    private CoherenceInput input;
    private CoherenceSync sync;
    private Grid grid;

    public float inputSamplingDelay = 0.10f;

    public const string SupportedAlphabet = "qwertyuiopasdfghjklzxcvbnm";

    private Hashtable keyPresses;
    
    private KeyCode[] acceptedAlphabetKeys;

    public Renderer cursor, cursorMine, cursorDirRight, cursorDirDown;
    public long startOnFrame;
    
    ArrayList tmpKeyRecorder = new ArrayList();

    private static KeyCode[] demoKeys = {KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.D,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.J,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.Y,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.M,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.A,KeyCode.S,KeyCode.RightArrow,KeyCode.A,KeyCode.LeftArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.G,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.O,KeyCode.RightArrow,KeyCode.W,KeyCode.RightArrow,KeyCode.J,KeyCode.K,KeyCode.L,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.R,KeyCode.DownArrow,KeyCode.O,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.G,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.U,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.S,KeyCode.RightArrow,KeyCode.O,KeyCode.RightArrow,KeyCode.M,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.H,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.W,KeyCode.O,KeyCode.DownArrow,KeyCode.W,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.E,KeyCode.UpArrow,KeyCode.M,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.C,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.R,KeyCode.RightArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.K,KeyCode.RightArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.C,KeyCode.DownArrow,KeyCode.H,KeyCode.DownArrow,KeyCode.O,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.E,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.O,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.D,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.N,KeyCode.DownArrow,KeyCode.Y,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.UpArrow,KeyCode.RightArrow,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.UpArrow,KeyCode.S,KeyCode.RightArrow,KeyCode.I,KeyCode.RightArrow,KeyCode.G,KeyCode.RightArrow,KeyCode.N,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.L,KeyCode.RightArrow,KeyCode.S,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.E,KeyCode.DownArrow,KeyCode.V,KeyCode.DownArrow,KeyCode.I,KeyCode.DownArrow,KeyCode.T,KeyCode.DownArrow,KeyCode.Y,KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.T,KeyCode.RightArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.RightArrow,KeyCode.C,KeyCode.RightArrow,KeyCode.Q,KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.K,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.LeftArrow,};
    private DemoKey[] demo = BuildDemo(demoKeys, 6); 
    private int demoKeysCurrentIndex;
    private bool demoKeysReplaying;
    private long demoStartFrame;
    private long demoPauseFrame;

    public bool lastTypedWasLetter = false;
    public TypingDirection lastTypingDirection = TypingDirection.NONE;
    public enum TypingDirection
    {
        RIGHT = 0,
        DOWN = 1,
        UP = 2,
        LEFT = 3,
        NONE = 100
    };

    public enum TypingMode
    {
        AUTOMATIC = 0,
        ALWAYS_RIGHT = 1,
        ALWAYS_DOWN = 2,
        MANUAL = 3
    }

    public TypingMode typingMode = TypingMode.AUTOMATIC;

    private static DemoKey[] BuildDemo(KeyCode[] keys, int delay)
    {
        DemoKey[] demo = new DemoKey[keys.Length];
        long frame = 0;
        for (int i = 0; i < keys.Length; i++)
        {
            demo[i] = new DemoKey(keys[i], frame);
            frame += delay;
        }
        return demo;
    }

    private bool IsNextDemoKey(KeyCode key)
    {
        var demokey = GetCurrentDemoKey(false);
        
        if (!demokey.HasValue || (demokey.Value.Frame + demoStartFrame) > input.CurrentSimulationFrame) return false;

        return demoKeysReplaying && demokey.Value.Key == key;
    }
    
    private DemoKey? GetCurrentDemoKey(bool advance = true)
    {
        if (demoKeysCurrentIndex < 0 || demoKeysCurrentIndex >= demo.Length)
        {
            demoKeysReplaying = false;
            return null;
        }

        var ret = demo[demoKeysCurrentIndex];
        if (advance)
        {
            demoKeysCurrentIndex++;
        }
        return ret;
    }
    
    private void Awake()
    {
        sync = GetComponent<CoherenceSync>();
        
        fixedUpdateInput = sync.MonoBridge.FixedUpdateInput;
        input = sync.Input;
        grid = FindObjectOfType<Grid>();

        keyPresses = new Hashtable {[KeyCode.UpArrow] = 0f, [KeyCode.DownArrow] = 0f, [KeyCode.LeftArrow] = 0f, [KeyCode.RightArrow] = 0f, [KeyCode.Backspace] = 0f};

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
            if(!grid.IsPaused)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    int tdi = (int)typingMode;
                    tdi++;
                    if (tdi >= Enum.GetNames(typeof(TypingMode)).Length)
                    {
                        tdi = 0;
                    }

                    typingMode = (TypingMode) tdi;
                    ChangeTypingDirectionBasedOnMode();
                }
            }

            if (grid)
            {
                grid.typingModeCaption.text = $"Direction: {typingMode.ToString().Replace("_"," ")}";
            }
            
            playerName = grid.playerName;

            cursorMine.enabled = true;
            cursor.enabled = false;

            cursorDirDown.enabled = lastTypingDirection == TypingDirection.DOWN;
            cursorDirRight.enabled = lastTypingDirection == TypingDirection.RIGHT;
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
            if (!demoKeysReplaying)
            {
                demoKeysReplaying = true;
                
                if (demoKeysCurrentIndex == 0)
                {
                    demoStartFrame = input.CurrentSimulationFrame;
                    Debug.Log($"[{demoStartFrame}] Starting demo");
                }
                else if (demoKeysCurrentIndex >= demo.Length)
                {
                    demoKeysCurrentIndex = 0;
                    demoStartFrame = input.CurrentSimulationFrame;
                    Debug.Log($"[{demoStartFrame}] Restarting demo");
                }
                else
                {
                    demoStartFrame += (input.CurrentSimulationFrame - demoPauseFrame);
                    Debug.Log($"[{demoStartFrame}] Continuing demo");
                }
            }
            else
            {
                demoKeysReplaying = false;
                demoPauseFrame = input.CurrentSimulationFrame;
            }
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
        ApplyAlphabet();
        ApplyMovement();
    }

    private void ApplyAlphabet()
    {
        bool canType = false;
        
        foreach (KeyCode key in acceptedAlphabetKeys)
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
                        break;
                    }
                }
            }
            else if (fixedUpdateInput.GetKeyDown(key))
            {
                input.SetButtonRangeState("key", (float)key);
                RecordKeyTime(key);
                canType = true;
                tmpKeyRecorder.Add(key);
                lastTypedWasLetter = true;
                break;
            }
        }

        if (!canType)
        {
            input.SetButtonRangeState("key", 0f);
        }
    }

    bool TryTypeKey(KeyCode key, int x0, int y0, ref int x, ref int y)
    {
        bool pressed = false;
        
        if (demoKeysReplaying)
        {
            if (IsNextDemoKey(key))
            {
                if (CanTypeKey(key))
                {
                    x = x0;
                    y = y0;
                    pressed = true;
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
                pressed = true;
                RecordKeyTime(key);
                
                tmpKeyRecorder.Add(key);
            }
            else
            {
                if (x0 != 0) x = 0;
                if (y0 != 0) y = 0;
            }
        }

        return pressed;
    }
    
    private void ApplyMovement()
    {
        int y = 0;
        int x = 0;

        TryTypeKey(KeyCode.UpArrow, 0, 1, ref x, ref y);
        TryTypeKey(KeyCode.DownArrow, 0, -1, ref x, ref y);
        TryTypeKey(KeyCode.RightArrow, 1, 0, ref x, ref y);
        TryTypeKey(KeyCode.LeftArrow, -1, 0, ref x, ref y);

        bool backspace = false;
        
        if (lastTypingDirection == TypingDirection.RIGHT)
        {
            backspace = TryTypeKey(KeyCode.Backspace, -1, 0, ref x, ref y);
        }
        
        if (lastTypingDirection == TypingDirection.DOWN)
        {
            backspace = TryTypeKey(KeyCode.Backspace, 0, 1, ref x, ref y);
        }
        
        if (x == 0 && y == 0)
        {
            if (lastTypedWasLetter)
            {
                if (lastTypingDirection != TypingDirection.NONE)
                {
                    x = lastTypingDirection == TypingDirection.RIGHT ? 1 : 0;
                    y = lastTypingDirection == TypingDirection.DOWN ? -1 : 0;

                    lastTypedWasLetter = false;
                }
            }
        }
        else
        {
            if (typingMode == TypingMode.AUTOMATIC)
            {
                DetectAutomaticTypingDirection(x, y, backspace);
            }
            else
            {
                ChangeTypingDirectionBasedOnMode();
            }

            lastTypedWasLetter = false;
        }
        
        var movement = new Vector2(x, y).normalized;
        input.SetAxisState("Mov", movement);
    }

    private void ChangeTypingDirectionBasedOnMode()
    {
        switch (typingMode)
        {
            case TypingMode.AUTOMATIC:
                break;
                
            case TypingMode.MANUAL:
                lastTypingDirection = TypingDirection.NONE;
                break;
                
            case TypingMode.ALWAYS_DOWN:
                lastTypingDirection = TypingDirection.DOWN;
                break;
                
            case TypingMode.ALWAYS_RIGHT:
                lastTypingDirection = TypingDirection.RIGHT;
                break;
        }
    }

    private void DetectAutomaticTypingDirection(int x, int y, bool backspace)
    {
        if (lastTypedWasLetter)
        {
            if (x > 0)
            {
                lastTypingDirection = TypingDirection.RIGHT;
            }
            else if (y < 0)
            {
                lastTypingDirection = TypingDirection.DOWN;
            }
            else
            {
                if (!backspace)
                {
                    lastTypingDirection = TypingDirection.NONE;
                }
            }
        }
        else
        {
            lastTypingDirection = TypingDirection.NONE;
        }
    }
}