using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    bool canMove = false;
    [SerializeField]
    float speed;
    void Update()
    {
        if (canMove)//If player collision to helicopter then helicopter will move
        {
            transform.position+=new Vector3(Time.deltaTime*speed,0,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-15, transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = true;            
        }
    }
}
