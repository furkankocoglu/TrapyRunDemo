using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Rigidbody[] ragdollRigidbodies;
    Rigidbody enemyRigidbody;
    Collider[] ragdollColliders;
    Animator enemyAnimator;
    GameObject player;
    [SerializeField]
    float followDistance,moveSpeed,force;
    bool isRagdollEnabled = false;
    NavMeshAgent enemeyAgent;
    float randomXPos;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = ragdollRigidbodies[0];
        enemeyAgent = GetComponent<NavMeshAgent>();
        randomXPos = Random.Range(-3.5f, 3.5f);    
        enemeyAgent.speed = moveSpeed;
    }
    
    void FixedUpdate()
    {
        if (GameController.instance.gameStarted)// If game is started then enemy will follow to player.
        {
            FollowPlayer();
        }
        
    }
    //Enemy will active or disable to AI , if enemy on base path it will move with physic otherwise it will move with AI
    void FollowPlayer()
    {        
        if (Vector3.Distance(transform.position,player.transform.position)>followDistance && enemeyAgent.enabled)
        {            
            enemyAnimator.SetBool("Running", true);
            if (GameController.instance.gameFail || GameController.instance.isFinished || (transform.position.x>-4 && transform.position.x<4))
            {
                enemeyAgent.enabled = false;
            }
            else
            {               
                enemeyAgent.SetDestination(new Vector3(randomXPos, 0, player.transform.position.z));
                transform.LookAt(enemeyAgent.destination);
            }            
            
        }
        else
        {
            if (transform.position.z >= player.transform.position.z && !GameController.instance.gameFail && !GameController.instance.isFinished)
            {
                enemyRigidbody.velocity = Vector3.zero;
            }
            else
            {
                enemyRigidbody.velocity = new Vector3(0, enemyRigidbody.velocity.y, Time.fixedDeltaTime * moveSpeed * 50f);
            }
            transform.LookAt(transform.position+Vector3.forward);

        }
        if(Vector3.Distance(transform.position, player.transform.position) <= followDistance)
        {
            if (!isRagdollEnabled)
            {
                EnableRagdoll(true);                
            }
           
        }
       
        if (!isRagdollEnabled)
        {            
            checkGround();
        }        
    }
    //Enemy checking ground with raycast.If ray do not hit any object then it will active ragdoll phsyic.
    void checkGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down,out hit,Mathf.Infinity))
        {
            
            if (hit.point.y<-1)
            {                
                EnableRagdoll(false);
                Destroy(gameObject,5f);
            }
        }
        else
        {
            EnableRagdoll(false);
            Destroy(gameObject, 5f);
        }
    }
    //When needed to active ragdoll call this method.If enemy close to player , it will active ragdoll and will jump to on player.
    void EnableRagdoll(bool onGround)
    {
        enemeyAgent.enabled = false;        
        enemyRigidbody.isKinematic = true;
        ragdollColliders[0].enabled = false;
        enemyAnimator.enabled = false;
        for (int i = 1; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
            ragdollColliders[i].enabled = true;
            if (onGround)
            {
                Vector3 referansVector = (player.transform.position - transform.position);
                Vector3 forceDirection = new Vector3(Mathf.Abs(referansVector.x)/referansVector.x, Mathf.Abs(referansVector.y) / referansVector.y, Mathf.Abs(referansVector.z) / referansVector.z);
                ragdollRigidbodies[i].velocity = forceDirection*force;
                GameController.instance.gameFail = true;
                UIController.instance.EnableGameOverPanel();
            }
            

        }
        isRagdollEnabled = true;
    }
}
