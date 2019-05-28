using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class IntroScreen : MonoBehaviour
{
    //Screens
    public GameObject intro;
    public GameObject StartScreen;
    public GameObject Controls;
    public GameObject QuitMenu;

    public FirstPersonController Player;

	// Use this for initialization
	void Start ()
    {
        Player = Player.GetComponent<FirstPersonController>();
        Player.enabled = false;
        Time.timeScale = 0;

        intro.SetActive(true);
	}

    public void ToStart()
    {
        intro.SetActive(false);
        StartScreen.SetActive(true);
        Controls.SetActive(false);
        QuitMenu.SetActive(false);
    }

    public void PlayGame()
    {
        StartScreen.SetActive(false);
        Player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void ToControls()
    {
        Controls.SetActive(true);
        StartScreen.SetActive(false);
    }

    public void ToQuitMenu()
    {
        QuitMenu.SetActive(true);
        StartScreen.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Pause()
    {
        if (StartScreen.activeInHierarchy == false)
        {
            StartScreen.SetActive(true);
            Player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }
}
