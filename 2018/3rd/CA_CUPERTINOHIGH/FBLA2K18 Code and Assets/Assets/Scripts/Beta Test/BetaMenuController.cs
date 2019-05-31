using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BetaMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject IntroMenu;
    public Button playB;
    public Button playB2;
    public void Start()
    {
        playB.Select();
    }
    public void LoadIntro()
    {
        MainMenu.SetActive(false);
        IntroMenu.SetActive(true);
        playB2.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Application.Quit();
        }*/
    }
}
