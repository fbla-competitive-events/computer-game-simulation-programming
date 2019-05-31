using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MemoryGameController : MonoBehaviour {

    public static int Score = 0;
    public static int Wrong = 0;
    public static bool Pause = false;
    public static bool PlayingRythm = false;
    public int MaxScore = 10;
    public int MaxWrong = 3;

    public GameObject LosePanel;
    public GameObject WinPanel;

    public GameObject PlayButton;

    public GameObject PauseMenu;

    public Button IntroB;
    public Button PauseB;
    public Button WinB;
    public Button LoseB;

    public GameObject HUD;
    public TextMeshProUGUI Correct;
    public TextMeshProUGUI Incorrect;
    public TextMeshProUGUI Instruct;
	// Use this for initialization
	void Start () {
        Score = 0;
        Wrong = 0;
        Pause = true;
        IntroB.Select();
    }

    public void Resume()
    {
        HUD.SetActive(true);
        Instruct.gameObject.SetActive(true);
        Pause = false;
    }
	
	// Update is called once per frame
	void Update () {
        Correct.text = "Correct: " + Score + " / " + MaxScore;
        Incorrect.text = "Incorrect: " + Wrong + " / " + MaxWrong;
        if (Input.GetKeyDown(KeyCode.Backspace) && !PlayingRythm)
        {
            Pause = !Pause;
            if (Pause)
            {
                HUD.SetActive(false);
                PauseMenu.SetActive(true);
                PauseB.Select();
                Instruct.gameObject.SetActive(false);
            } else
            {

                PauseMenu.SetActive(false);
                PlayButton.GetComponent<Button>().Select();
                HUD.SetActive(true);
                Instruct.gameObject.SetActive(true);
            }
        }
		if (Score >= MaxScore)
        {
            HUD.SetActive(false);
            Instruct.gameObject.SetActive(false);
            PlayButton.SetActive(false);
            Pause = true;
            WinPanel.SetActive(true);
            WinB.Select();
            Score = 0;
            Wrong = 0;
        } 
        if (Wrong >= MaxWrong)
        {
            Instruct.gameObject.SetActive(false);
            HUD.SetActive(false);
            PlayButton.SetActive(false);
            Pause = true;
            LosePanel.SetActive(true);
            LoseB.Select();
            Score = 0;
            Wrong = 0;
        }
	}

    public void FinishMission()
    {
        GameObject.Find("Game Controller").GetComponent<GameAndPlayerManager>().FinishMission();
    }

    public void QuitMission()
    {
        GameObject.Find("Game Controller").GetComponent<GameAndPlayerManager>().CancelMission();

    }
}
