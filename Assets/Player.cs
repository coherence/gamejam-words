using System.Collections;
using Coherence.Toolkit;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CoherenceSync))]
[RequireComponent(typeof(CoherenceInput))]
public class Player : MonoBehaviour
{
    private FixedUpdateInput fixedUpdateInput;

    public Vector2Int gridPosition = new Vector2Int(25, 25);
   
    private CoherenceInput input;
    private Grid grid;

    private const float inputSamplingDelay = 0.25f;

    private const string InternalAlphabet = "qwertyuiopasdfghjklzxcvbnm";

    struct KeyPress
    {
        public KeyCode keyCode;
        public float timestamp;
    }

    private Hashtable keyPresses;
    
    private KeyCode[] acceptedAlphabetKeys;

    private void Awake()
    {
        var coherenceSync = GetComponent<CoherenceSync>();
        fixedUpdateInput = coherenceSync.MonoBridge.FixedUpdateInput;
        input = coherenceSync.Input;
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
        acceptedAlphabetKeys = new KeyCode[InternalAlphabet.Length];
        for (var i = 0; i < InternalAlphabet.Length; i++)
        {
            var key = (KeyCode) System.Enum.Parse(typeof(KeyCode), InternalAlphabet[i].ToString().ToUpper());
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
    }

    public Vector2 GetNetworkInputMovement(long frame)
    {
        return input.GetAxisState("Mov", frame);
    }

    public string GetNetworkInputString(long frame)
    {
        return input.GetStringState("key", frame);
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
                    input.SetStringState("key", key.ToString());
                    RecordKeyTime(key);
                }
                else
                {
                    input.SetStringState("key", null);
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

        Vector2 movement = new Vector2(x, y).normalized;
        input.SetAxisState("Mov", movement);
    }
}