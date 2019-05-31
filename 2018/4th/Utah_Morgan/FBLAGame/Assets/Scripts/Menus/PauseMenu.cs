using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : UIBox
{
    /// <summary>
    /// Single ton instance of this type
    /// </summary>
    public static PauseMenu Instance { get { return FindObjectOfType<PauseMenu>(); } }

    /// <summary>
    /// If the game is paused or not
    /// </summary>
    public static bool IsGamePaused = false;

    /// <summary>
    /// The main menu scene
    /// </summary>
    [SerializeField]
    string MainMenuScene;

    /// <summary>
    /// Resumes the game by setting the time scale back to normal
    /// </summary>
    public void Resume()
    {        
        UIManager.Instance.OpenClose(this);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause()
    {
        UIManager.Instance.OpenClose(this);        
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    /// <summary>
    /// Loads the menu scene
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
