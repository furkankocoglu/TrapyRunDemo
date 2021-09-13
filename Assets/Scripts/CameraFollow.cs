using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform,helicopter;
    Vector3 offset,helicopterPosition;
    void Start()
    {
        offset = new Vector3(0,transform.position.y-playerTransform.transform.position.y,transform.position.z-playerTransform.transform.position.z);
        helicopterPosition = helicopter.position;
    }
    
    void Update()
    {
        if (GameController.instance.isFinished)//if level finished then look helicopter smooth
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(helicopterPosition-transform.position),Time.deltaTime);
        }
        else//else follow player with offset
        {
            transform.position = playerTransform.position + offset;
        }
        
    }
}
