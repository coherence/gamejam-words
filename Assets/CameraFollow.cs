using System.Collections;
using System.Collections.Generic;
using Coherence.Toolkit;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player player;

    private Vector3 targetPosition;

    public bool cameraNear = true;
    public float nearSize = 10f, farSize = 20f;

    private Vector3 originalCameraPos;
    
    void Awake()
    {
        originalCameraPos = targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraNear = !cameraNear;

            Camera.main.orthographicSize = cameraNear ? nearSize : farSize;
        }
    }

    void LateUpdate()
    {
        if (cameraNear == false)
        {
            transform.position = originalCameraPos;
            Camera.main.orthographicSize = cameraNear ? nearSize : farSize;
            return;
        }
        
        if (player == null)
        {
            var objs = FindObjectsOfType<Player>();
            foreach (var obj in objs)
            {
                var sync = obj.GetComponent<CoherenceSync>();
                if (sync)
                {
                    if (sync.isSimulated)
                    {
                        player = obj;
                    }
                }
            }
        }
        else
        {
            var playerPos = player.transform.position;
            playerPos.z = transform.position.z;
            targetPosition = Vector3.Lerp(targetPosition, playerPos, Time.deltaTime * 0.6f);
            
            if (Vector3.SqrMagnitude(transform.position - targetPosition) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 0.5f);
            }
        }
        
        
    }
}
