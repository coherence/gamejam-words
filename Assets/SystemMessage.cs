using TMPro;
using UnityEngine;

public class SystemMessage : MonoBehaviour
{
    public TMP_Text messageText;
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
}