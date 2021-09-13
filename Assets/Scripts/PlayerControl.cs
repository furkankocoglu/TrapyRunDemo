using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    float speed;
    float firstTouchPositionX;
    bool isJumped = false;
    Vector3 direction;
    Rigidbody playerRigidbody;
    Animator playerAnimator;
    [SerializeField]
    Transform helicopterTransform,endZoneTransform;
    Rigidbody[] ragdollRigidbodies;
    Collider[] ragdollColliders;
    bool isRagdollEnabled = false;
    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        playerRigidbody = ragdollRigidbodies[0];
        playerAnimator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        TouchControl();
        if (GameController.instance.gameStarted)
        {
            if (!GameController.instance.gameFail)// if game started and game not fail then control character.
            {
                PlayerMovement(direction);
            }
            else//otherwise enable ragdoll physic
            {                
                if (!isRagdollEnabled)
                {
                    EnableRagdoll();
                }
                
            }
                        
        }
        
    }
    //enable ragdoll physic
    void EnableRagdoll()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = true;
        ragdollColliders[0].enabled = false;
        playerAnimator.enabled = false;
        for (int i = 1; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
            ragdollColliders[i].enabled = true;

        }
        isRagdollEnabled = true;
    }
    //check touch to control character.
    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            if (!GameController.instance.gameStarted)// if game not started then start the game
            {
                GameController.instance.gameStarted = true;
                playerAnimator.SetBool("Running",true);
            }
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)//first touch
            {
                firstTouchPositionX = touch.position.x;
                direction = new Vector3(0, 0, 1);
            }
            else if (touch.phase == TouchPhase.Moved)//still touching
            {               
                if (touch.position.x > firstTouchPositionX)
                {
                    direction = new Vector3(1,0,1);                    
                }
                else
                {
                    direction = new Vector3(-1, 0, 1);
                }
            }
            else if (touch.phase == TouchPhase.Ended)//touch ended
            {
                direction = new Vector3(0, 0, 1);
            }
        }
    }
    //Move Player to direction
    void PlayerMovement(Vector3 direction)
    {
        if (!GameController.instance.isFinished)// if game not finished then control player with touch control
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,direction.x*45f,0),Time.deltaTime*2f);  
            direction *= Time.fixedDeltaTime * speed;
            playerRigidbody.velocity = new Vector3(direction.x, playerRigidbody.velocity.y, direction.z);
            if (playerRigidbody.velocity.y < -5f)
            {
                playerAnimator.SetBool("Falling", true);
            }
        }
        else//otherwise if player did not jump to helicopter, go to jump position and jump to helicopter.
        {
            
            if (!isJumped)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2f);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(helicopterTransform.position.x,transform.position.y,endZoneTransform.position.z), Time.deltaTime * 5f);
                if (Vector3.Distance(transform.position, endZoneTransform.position) < 1)
                {
                    playerAnimator.SetTrigger("Jump");
                    transform.position = new Vector3(helicopterTransform.position.x + 1.5f, transform.position.y, transform.position.z);
                    playerRigidbody.velocity = new Vector3(0, (helicopterTransform.position.y - transform.position.y) + Physics.gravity.y * -1, (helicopterTransform.position.z - transform.position.z) * 0.45f);
                    isJumped = true;

                }
            }
           
            
            
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("finish"))
        {
            GameController.instance.isFinished = true;            
            
        }
        if (collision.gameObject.CompareTag("helicopter"))
        {
            UIController.instance.EnableLevelCompletedPanel();
            playerRigidbody.velocity = Vector3.zero;
            transform.SetParent(collision.transform);
        }
    }
}
