using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public bool gameStarted = false,isFinished=false,gameFail=false;
    public static GameController instance;
    private void Awake()
    {
        instance = this;        
}    
}
