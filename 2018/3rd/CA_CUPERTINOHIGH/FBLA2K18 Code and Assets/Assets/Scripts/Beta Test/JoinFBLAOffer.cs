using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JoinFBLAOffer : MonoBehaviour {
    public BetaGameOptions PlayerScript;
    public GameObject MissionBoard;
    public void DeclineOffer()
    {
        Debug.Log("ERROR!");
        BetaGameOptions.pause = false;
        CanvasPanel.SetActive(false);
        viewingMenu = false;
        Minimap.SetActive(true);
        //corruption
    }

    public void AcceptOffer()
    {
        GameAndPlayerManager.IsFBLAMember = true;
        Debug.Log("Congrats!");
        BetaGameOptions.pause = false;
        CanvasPanel.SetActive(false);
        viewingMenu = false;
        Minimap.SetActive(true);
        
        DisplayCongrats = true;
        GameObject.Find("Key Listener").GetComponent<KeyListener>().KeyActive = true;
        GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = true;
    }


    public bool isInteractable;
    public Transform Player;
    public float inBoundOfInteractions;

    public Button activeButton;
    public GameObject CanvasPanel;
    public bool viewingMenu = false;
    public GameObject Minimap;
    public GameObject Board;

    // Use this for initialization
    void Start()
    {
        if (GameAndPlayerManager.IsFBLAMember)
        {
            gameObject.SetActive(false);
            MissionBoard.SetActive(true);

        }
        isInteractable = false;
        Player = GameObject.Find("Main Player").GetComponent<Transform>();
        inBoundOfInteractions = 10f;
    }

    void Update()
    {
        if (viewingMenu)
        {
            if (Input.GetKeyDown(KeyCode.E) && BetaGameOptions.pause == true)
            {
                BetaGameOptions.pause = false;
                CanvasPanel.SetActive(false);
                viewingMenu = false;
                Minimap.SetActive(true);
                GameObject.Find("Key Listener").GetComponent<KeyListener>().KeyActive = true;
                GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = true;
            }
        }
        else
        {

            if (gameObject.activeSelf && !viewingMenu)
            {
                var heading = Player.position - GetComponent<Transform>().position;
                heading.y = 0;

                float offset = Board.GetComponent<Collider>().bounds.size.x * Player.localScale.x;

                if (heading.sqrMagnitude <= inBoundOfInteractions + offset * 1.5)
                {
                    isInteractable = true;
                    if (Input.GetKeyDown(KeyCode.E) && BetaGameOptions.pause == false)
                    {
                        BetaGameOptions.pause = true;
                        Minimap.SetActive(false);
                        activeButton.Select();
                        viewingMenu = true;
                        CanvasPanel.SetActive(true);
                        GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = false;
                        GameObject.Find("Key Listener").GetComponent<KeyListener>().KeyActive = false;
                    }
                }
                else
                {
                    isInteractable = false;
                }
            }

        }
    }
    float guiAlpha = 1;
    bool DisplayCongrats = false;
    private void OnGUI()
    {
        if (isInteractable && BetaGameOptions.pause == false)
        {
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = Screen.width / 20;
            GUI.Box(new Rect(Screen.width / 2 - Screen.width / 6 , Screen.height - Screen.height / 8, Screen.width / 6, Screen.height / 8), "View [E]", myStyle);
        }

        if (DisplayCongrats)
        {
            Color b = Color.black;
            b.a = guiAlpha;
            guiAlpha -= Time.deltaTime / 4;

            GUI.color = b;
            GUIStyle st = new GUIStyle();
            st.fontSize = Screen.width / 25;
            GUI.Box(new Rect(Screen.width / 5, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), "Congrats! View the Mission Board now", st);
            if (guiAlpha <= 0f)
            {
                DisplayCongrats = false;
                gameObject.SetActive(false);
                MissionBoard.SetActive(true);
                guiAlpha = 1f;
            }
        }
    }
}
