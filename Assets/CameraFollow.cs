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

    public float lerpSpeed = 2;
    
    private Vector3 originalCameraPos;

    float distance = 0;

    public float snapDistance = 10;
    
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
                    if (sync.HasStateAuthority)
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
            targetPosition = playerPos;// Vector3.Lerp(targetPosition, playerPos, Time.deltaTime * 0.6f);

            if (Time.frameCount % 20 == 0)
            {
                distance = Vector3.SqrMagnitude(transform.position - targetPosition);
            }
            
            if (distance > snapDistance)
            {
                transform.position = targetPosition;
                distance = Vector3.SqrMagnitude(transform.position - targetPosition);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            }
        
        }
        
        
    }
}
