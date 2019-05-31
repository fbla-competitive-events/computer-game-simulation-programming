using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : UIBox
{
    /// <summary>
    /// Opens the Main Menu on start
    /// </summary>
    private void Start()
    {
        UIManager.Instance.OpenClose(this);
    }

    /// <summary>
    /// Loads the base scene and the outside scene
    /// </summary>
    public void PlayGame()
    {        
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        async.completed += (AsyncOperation a) =>
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        };
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }	
}
