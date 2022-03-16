using System.Collections;
using System.Collections.Generic;
using Coherence.Toolkit;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player player;

    private Vector3 targetPosition;

    void Awake()
    {
        targetPosition = transform.position;
    }

    void LateUpdate()
    {
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
            targetPosition = playerPos;
            
            if (Vector3.SqrMagnitude(transform.position - targetPosition) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 0.2f);
            }
        }
        
        
    }
}
