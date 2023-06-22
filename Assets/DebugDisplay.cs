using TMPro;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    public float updateRate = 5.0f;
    
    public TMP_Text pingText;

    public TMP_Text simHashText;
    public TMP_Text fixedFrameText;
    public TMP_Text localFrameText;
    public TMP_Text serverFrameText;
    public TMP_Text deltaFrameText;
    public TMP_Text timeScaleText;
    
    public TMP_Text inputAckFrameText;
    public TMP_Text inputMispredictionFrameText;
    public TMP_Text inputCommonFrameText;
    public TMP_Text inputShouldPauseText;

    private float lastUpdate;
    
    public void UpdateInfo(
        int latencyMs,
        long fixedFrame,
        long localFrame,
        long serverFrame,
        string hash, 
        long inputAckFrame, 
        long? inputMispredictionFrame,
        long? inputCommonFrame, 
        bool inputShouldPause)
    {
        float updateInterval = 1f / updateRate;
        if (lastUpdate + updateInterval > Time.unscaledTime)
        {
            return;
        }
        
        lastUpdate = Time.unscaledTime;

        pingText.text = $"{latencyMs}ms";

        fixedFrameText.text = fixedFrame.ToString();
        simHashText.text = hash;
        serverFrameText.text = serverFrame.ToString();
        localFrameText.text = localFrame.ToString();
        deltaFrameText.text = (localFrame - serverFrame).ToString();
        timeScaleText.text = Time.timeScale.ToString("F4");

        inputAckFrameText.text = inputAckFrame.ToString();
        inputMispredictionFrameText.text = inputMispredictionFrame.ToString();
        inputCommonFrameText.text = inputCommonFrame.ToString();
        inputShouldPauseText.text = inputShouldPause.ToString();
    }
}