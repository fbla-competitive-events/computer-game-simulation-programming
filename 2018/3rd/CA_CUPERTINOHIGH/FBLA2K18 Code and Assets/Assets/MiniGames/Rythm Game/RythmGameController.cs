using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RythmGameController : MonoBehaviour {
    public static float Score = 0;
    public static bool Pause = true;
    public static int MissedBeats = 0;
    public int MaxScore;
    public int MaxMissedBeats;
    
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject PauseMenu;
    public GameObject HUD;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MissedText;
    public static string ScoreTextT = "Score: 0 / ";
    public static string MissedTextT = "Missed Beats: 0 / ";

    public Button startB;
    public Button PauseB;
    public Button WinB;
    public Button LoseB;

    public static int MScore;
    public static int MBeats;
    // Use this for initialization
    void Start () {
        startB.Select();
        MScore = MaxScore;
        MBeats = MaxMissedBeats;
        
    }

    public void QuitMission()
    {
        LosePanel.SetActive(false);
        Reset();
        GameObject.Find("Game Controller").GetComponent<GameAndPlayerManager>().CancelMission();
    }

    public void FinishMission()
    {
        WinPanel.SetActive(false);
        Reset();
        GameObject.Find("Game Controller").GetComponent<GameAndPlayerManager>().FinishMission();
    }
    static bool w = true;
    static bool l = true;
    public static void Reset()
    {
        Score = 0;
        MissedBeats = 0;
        ScoreTextT = "Score: " + Score + " / " + MScore;
        MissedTextT = "Missed Beats: " + MissedBeats + " / " + MBeats;
        w = true;
        l = true;
    }

    public void Resume() {
        ScoreText.gameObject.SetActive(true);
        MissedText.gameObject.SetActive(true);
        Pause = false;
    }

    

    // Update is called once per frame
    void Update () {
        ScoreTextT = "Score: " + Score + " / " + MScore;
        MissedTextT = "Missed Beats: " + MissedBeats + " / " + MBeats;
        ScoreText.text = ScoreTextT;
        MissedText.text = MissedTextT;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //menu 
            Pause = !Pause;
            if (Pause)
            {
                HUD.SetActive(false);
                ScoreText.gameObject.SetActive(false);
                MissedText.gameObject.SetActive(false);
                PauseMenu.SetActive(true);
                PauseB.Select();
            } else
            {
                HUD.SetActive(true);
                ScoreText.gameObject.SetActive(true);
                MissedText.gameObject.SetActive(true);
                PauseMenu.SetActive(false);
            }
        }

        if (Score >= MaxScore)
        {
            HUD.SetActive(false);
            ScoreText.gameObject.SetActive(false);
            MissedText.gameObject.SetActive(false);
            Pause = true;
            WinPanel.SetActive(true);
            if (w)
            {
                w = false;
                WinB.Select();
            }
        }
        if (MissedBeats >= MaxMissedBeats)
        {
            HUD.SetActive(false);
            ScoreText.gameObject.SetActive(false);
            MissedText.gameObject.SetActive(false);
            Pause = true;
            LosePanel.SetActive(true);
            
            if (l)
            {
                l = false;
                LoseB.Select();
            }
        }
	}
    
}
