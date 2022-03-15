using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemMessage : MonoBehaviour
{
    public TMP_Text messageText, frame, serverFrame, timeScale, hashLabel;
    public Transform panel;

    public void DisplayMessage(string msg)
    {
        if (string.IsNullOrEmpty(msg))
        {
            panel.gameObject.SetActive(false);
            messageText.text = "";
            return;
        }
        
        panel.gameObject.SetActive(true);
        messageText.text = msg;
    }

    public void UpdateFrame(long localFixedFrame, long localFrame, long serverFrame, string hash)
    {
        frame.text = localFixedFrame.ToString();
        this.serverFrame.text = $"{localFrame} / {serverFrame} ({localFrame - serverFrame})";
        timeScale.text = Time.timeScale.ToString("F6");
        hashLabel.text = hash;
    }
}
