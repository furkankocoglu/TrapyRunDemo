using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrap : MonoBehaviour
{
    Rigidbody cubeRigidbody;
    Renderer cubeRenderer;
    bool isCollisionEnter = false;
    private void Start()
    {
        cubeRigidbody = GetComponent<Rigidbody>();
        cubeRenderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (isCollisionEnter)//if player collision to cube then change color to red smooth
        {
            cubeRenderer.material.color = Color.Lerp(cubeRenderer.material.color, Color.red, 0.05f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameController.instance.gameStarted && !GameController.instance.gameFail)//if game started and game not fail then aktive cube physic and destroy after 5 sec.
            {
                cubeRigidbody.useGravity = true;
                cubeRigidbody.isKinematic = false;
                isCollisionEnter = true;
                Destroy(gameObject, 5f);
            }
           
        }        
        
    }
   
}
