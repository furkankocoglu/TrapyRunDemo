using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    Vector3 offset;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject enemyPrefab;
    bool invokeStarted = false;
    [SerializeField]
    float createDelay, createTime;
    void Start()
    {
        offset = transform.position-player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameController.instance.gameStarted && !invokeStarted)//if game started and invoke not stared then CreateEnemy method will repeat per create time
        {
            InvokeRepeating("CreateEnemy",createDelay,createTime);
            invokeStarted = true;
        }
        if ((GameController.instance.gameFail||GameController.instance.isFinished) && invokeStarted)//if game fail or level finished and invoke started then cancel invoke and destroy this
        {
            CancelInvoke();
            Destroy(gameObject);
        }
        else//otherwise follow player with offset
        {
            Vector3 targetPos = new Vector3(transform.position.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
            transform.position = targetPos;

        }
    }

    //Creating 8 enemy at random position
    void CreateEnemy()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(transform.position.x-4,transform.position.x+4),transform.position.y,transform.position.z-(Random.Range(0f,2f)));
            Instantiate(enemyPrefab,randomPosition,Quaternion.identity);
        }
    }
}
