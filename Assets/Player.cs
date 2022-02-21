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

    private const float inputSamplingDelay = 0.20f;

    public const string SupportedAlphabet = "qwertyuiopasdfghjklzxcvbnm";

    private Hashtable keyPresses;
    
    private KeyCode[] acceptedAlphabetKeys;

    public Renderer cursor, cursorMine;

    public long startOnFrame = 0;

    private void Awake()
    {
        sync = GetComponent<CoherenceSync>();
        
        fixedUpdateInput = sync.MonoBridge.FixedUpdateInput;
        input = sync.Input;
        grid = FindObjectOfType<Grid>();

        keyPresses = new Hashtable();
        
        keyPresses[KeyCode.UpArrow] = 0f;
        keyPresses[KeyCode.DownArrow] = 0f;
        keyPresses[KeyCode.LeftArrow] = 0f;
        keyPresses[KeyCode.RightArrow] = 0f;
        
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
        foreach (var key in acceptedAlphabetKeys)
        {
            if (Input.GetKey(key))
            {
                if (CanTypeKey(key))
                {
                    input.SetButtonRangeState("key", (float)key);
                    RecordKeyTime(key);
                }
                else
                {
                    input.SetButtonRangeState("key", 0f);
                }
            }
        }
    }

    void TryTypeKey(KeyCode key, int x0, int y0, ref int x, ref int y)
    {
        if (Input.GetKey(key)) 
        {
            if(CanTypeKey(key))
            {
                x = x0;
                y = y0;
                RecordKeyTime(key);
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