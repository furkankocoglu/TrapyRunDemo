using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverPanel,levelCompletedPanel;
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    public void EnableGameOverPanel()//enable game over panel
    {
        gameOverPanel.SetActive(true);
    }
    public void RestartLevel()// restart current level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void EnableLevelCompletedPanel()// enable level completed panel
    {
        levelCompletedPanel.SetActive(true);
    }
    public void NextLevel()//continue to next level
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings-1>=nextLevelIndex)
        {            
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
    }
}
