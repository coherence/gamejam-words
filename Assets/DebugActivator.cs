using System;
using UnityEngine;

public class DebugActivator : MonoBehaviour
{
    [Serializable]
    public struct Entry
    {
        public KeyCode ToggleKey;
        public GameObject Object;
        public MonoBehaviour Component;
    }

    public Entry[] entries = Array.Empty<Entry>();

    private void Update()
    {
        foreach (var entry in entries)
        {
            if (Input.GetKeyDown(entry.ToggleKey))
            {
                if (entry.Object)
                {
                    entry.Object.SetActive(!entry.Object.activeSelf);
                }

                if (entry.Component)
                {
                    entry.Component.enabled = !entry.Component.enabled;
                }
            }
        }
    }
}