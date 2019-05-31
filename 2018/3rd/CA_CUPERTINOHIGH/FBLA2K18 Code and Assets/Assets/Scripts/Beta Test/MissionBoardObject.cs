using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionBoardObject : MonoBehaviour {
    public bool isInteractable;
    public Transform Player;
    public float inBoundOfInteractions;

    public GameObject activeMissionButton;
    public GameObject MissionSelection;
    public bool viewingMenu = false;
    public GameObject Minimap;
    public GameObject Board;

    public bool DisplayFailure = false;
    // Use this for initialization
    void Start()
    {
        
        isInteractable = false;
        Player = GameObject.Find("Main Player").GetComponent<Transform>();
        inBoundOfInteractions = 10f;
    }

    void Update()
    {
        if (KeyListener.IsProfileActive)
        {
            MissionSelection.SetActive(false);
            viewingMenu = false;
            Minimap.SetActive(true);
            return;
        }
        if (viewingMenu)
        {
            if (Input.GetKeyDown(KeyCode.E) && BetaGameOptions.pause == true)
            {
                BetaGameOptions.pause = false;
                MissionSelection.SetActive(false);
                viewingMenu = false;
                Minimap.SetActive(true);
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
                        if (GameAndPlayerManager.IsFBLAMember)
                        {
                            BetaGameOptions.pause = true;
                            Minimap.SetActive(false);
                            MissionSelection.SetActive(true);
                            activeMissionButton.GetComponent<MissionSelections>().activeMission.Select();
                            viewingMenu = true;
                        } else
                        {
                            DisplayFailure = true;
                        }
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
    private void OnGUI()
    {
        if (isInteractable)
        {
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = Screen.width / 20;
            GUI.Box(new Rect(Screen.width / 2 - Screen.width / 6, Screen.height - Screen.height / 8, Screen.width / 6, Screen.height / 8), "View [E]", myStyle);
        }

        if (DisplayFailure)
        {
            Color b = Color.black;
            b.a = guiAlpha;
            guiAlpha -= Time.deltaTime / 4;

            GUI.color = b;
            GUIStyle st = new GUIStyle();
            st.fontSize = Screen.width / 18;
            GUI.Box(new Rect(Screen.width / 2 - Screen.width / 3, Screen.height / 2 - Screen.height / 3, Screen.width / 2, Screen.height / 5), "Must Be A FBLA Member!", st);
            if (guiAlpha <= 0f)
            {
                DisplayFailure = false;
                guiAlpha = 1f;
            }
        }
    }
}
