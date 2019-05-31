using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameAndPlayerManager : MonoBehaviour {
    //resources:
    /*
     * https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
     * */
    public static GameObject Player;
    //use this value to set player's position when loading a new scene
    public static Vector3 PlayerPosition;

    //talk to CurrentMisionObject and get/set values
    public static CurrentMissionObject MissionScript;

    public static int Involvement = 0;
    public static int Charisma = 0;
    public static int Knowledge = 0;

    public static int AddInvolvement = 0;
    public static int AddCharisma = 0;
    public static int AddKnowledge = 0;

    public static bool IsFBLAMember = false;
    public bool FirstTime = true;



    //save player stats in file
    public void SaveAndQuit()
    {
        Debug.Log("QUITing...");
        Application.Quit();
    }


    
    float guiAlpha = 1;
    bool moveText = false;
    bool turnText = false;
    bool sprintText = false;
    bool interactText = false;
    bool profileText = false;
    bool optionsText = false;
    bool letsStart = false;
    bool ShowMovement = false;
    bool interactBoard = false;
    int fontText = Screen.width / 10;

    public float Sound = 0.5f;
    public void ChangeSound(Slider slide)
    { 
        Sound = slide.value;
        gameObject.GetComponent<AudioSource>().volume = Sound;
    }


    private void OnGUI()
    {
        if (ShowMovement) { 
            string MoveText = "WASD To Move";
            string TurnText = "Arrow Keys To Turn";
            string SprintText = "SHIFT To Toggle Sprint";
            string InteractText = "E To Interact";
            string ProfileText = "TAB To Open Player Profile";
            string OptionsText = "ESC To Open Menu";
            string LetsStart = "Let's START!";
            string InteractWithBoard = "Walk Forward and View This Board";
            Color b = new Color(255, 87, 40);
            b.a = guiAlpha;
            guiAlpha -= Time.deltaTime / 4;

            GUI.color = b;
            GUIStyle st = new GUIStyle();
            st.fontSize = fontText;

            if (moveText) GUI.Box(new Rect(Screen.width / 2 - Screen.width/3, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), MoveText, st);
            if (turnText) GUI.Box(new Rect(Screen.width / 5, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), TurnText, st);
            if (sprintText) GUI.Box(new Rect(Screen.width / 8, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), SprintText, st);
            if (interactText) GUI.Box(new Rect(Screen.width / 2 - Screen.width / 5, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), InteractText, st);
            if (profileText) GUI.Box(new Rect(Screen.width / 8, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), ProfileText, st);
            if (optionsText) GUI.Box(new Rect(Screen.width / 5, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), OptionsText, st);
            if (letsStart) GUI.Box(new Rect(Screen.width / 3, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), LetsStart, st);
            if (interactBoard) GUI.Box(new Rect(Screen.width / 5 - Screen.width/10, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), InteractWithBoard, st);


            if (guiAlpha <= 0f)
            {
                guiAlpha = 1f;
                if (moveText)
                {
                    moveText = false;
                    turnText = true;
                    fontText = Screen.width / 13;
                }
                else if (turnText)
                {
                    turnText = false;
                    sprintText = true;
                }
                else if (sprintText)
                {
                    sprintText = false;
                    interactText = true;
                }
                else if (interactText)
                {
                    interactText = false;
                    profileText = true;
                    fontText = Screen.width / 15;
                }
                else if (profileText)
                {
                    profileText = false;
                    optionsText = true;
                    
                }
                else if (optionsText)
                {
                    optionsText = false;
                    letsStart = true;
                } else if (letsStart)
                {
                    letsStart = false;
                    
                    BetaGameOptions.pause = false;
                    GameObject.Find("Key Listener").GetComponent<KeyListener>().KeyActive = true;
                    interactBoard = true;
                    fontText = Screen.width / 20;
                } else if (interactBoard)
                {
                    ShowMovement = false;
                    interactBoard = false;
                }
            }
        }
    }

    private void Awake()
    {

        if (FirstTime && IsFBLAMember == false)
        {
            Debug.Log(BetaGameOptions.pause);
            BetaGameOptions.pause = true;
            GameObject.Find("Key Listener").GetComponent<KeyListener>().KeyActive = false;
            moveText = true;
            ShowMovement = true;
        }
        FirstTime = false;
        GameObject[] g = GameObject.FindGameObjectsWithTag("GameController");
        if (g.Length > 1)
        {
            GameObject.Destroy(g[0]);
           if (g.Length > 2)
           {
                Destroy(g[1]);
           }
        }
        DontDestroyOnLoad(this);
    }



    //use to keep track of Mission Rewards once completed
    public static void SetupMissionRewards(int i, int c, int k)
    {
        AddInvolvement = i;
        AddCharisma = c;
        AddKnowledge = k;
    }

    public void FinishMission()
    {
        AddRewards();
        SceneManager.LoadScene("v.0.5 BT");
        Player.transform.position = PlayerPosition + new Vector3(0, 1000f);
        Debug.Log(PlayerPosition);
    }

    private static void AddRewards()
    {
        Involvement += AddInvolvement;
        Charisma += AddCharisma;
        Knowledge += AddKnowledge;
    }

    public void CancelMission()
    {
        //ask confirmation
        //'Are you sure you want to leave? Progress will not be saved'
        SceneManager.LoadScene("v.0.5 BT");
        Player.transform.position = PlayerPosition + new Vector3(0, 1000f);
    }

    private void Start()
    {
        Player = GameObject.Find("Main Player");
        DontDestroyOnLoad(Player);
        SavePlayerPosition();
    }

    public static void SavePlayerPosition()
    {
        PlayerPosition = Player.transform.position;
    }


    public static void StartMiddleLevel()
    {
        SavePlayerPosition();
        Debug.Log(PlayerPosition);
        SceneManager.LoadScene("Rythm Game");
        
    }
    
    public static void StartStudyingCompetition()
    {
        SavePlayerPosition();
        SceneManager.LoadScene("MemoryGame");
    }
    
    public static void StartRehearsingSpeech()
    {
        SavePlayerPosition();
        SceneManager.LoadScene("Rythm Game");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
}
