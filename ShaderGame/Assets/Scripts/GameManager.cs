using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;

    //used for configuration settings
    private string[] levelOrder = {"Level1","Level2","Level3","Level4"};
    private string[] shaderOrder = { "None", "Outline", "Shadow", "Both"};
    private int CurrentLevel = 0;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    //Loads a scene given a scene name
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Used for moving through the 4 levels of the game
    public void LoadNextLevel()
    {
        if (CurrentLevel == 4)
        {
            CurrentLevel = 0;
            LoadLevel("GameOver");
        }
        else
        {
            CurrentLevel++;
            
            LoadLevel(levelOrder[CurrentLevel - 1]);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public string[] GetLevelOrder()
    {
        return levelOrder;
    }

    public string[] GetShaderOrder()
    {
        return shaderOrder;
    }

    public void SetLevelOrder(string[] levels)
    {
        for(int i = 0; i<4; i ++)
        {
            levelOrder[i] = levels[i];
        }
    }

    public void SetShaderOrder(string[] shaders)
    {
        for( int i = 0; i<4; i++)
        {
            shaderOrder[i] = shaders[i];
        }
    }

    //Return current level
    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    //Return current shader settings
    public string GetCurrentShader()
    {
        return shaderOrder[CurrentLevel - 1];
    }
}
