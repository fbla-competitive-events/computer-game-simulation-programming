using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    //public Button[] menuButtons;
    public static GameObject menuPanel;

    
    //hardcoded buttons here
    public Button optionB;
    public Button statB;
    public Button display2B;

    //hardcoded Parent
    public GameObject optionG;
    public GameObject statG;
    public GameObject display2G;
    // Use this for initialization
    void Start()
    {
        //hardcode all the menu buttons here
        optionB = GameObject.Find("Options Button").GetComponent<Button>();
        statB = GameObject.Find("Stats Button").GetComponent<Button>();
        display2B = GameObject.Find("Button Displayer 2").GetComponent<Button>();
        

        optionB.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OptionOnClick());
        statB.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StatOnClick());
        display2B.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Display2OnClick());


        optionG = GameObject.Find("Option Menu");
        statG = GameObject.Find("Stat Menu");
        display2G = GameObject.Find("Display2 Menu");
        optionG.SetActive(true); statG.SetActive(false); display2G.SetActive(false);

        menuPanel = GameObject.Find("Menu Panel");
        menuPanel.SetActive(false);
        
    }

    


    void OptionOnClick()
    {
        optionG.SetActive(true); statG.SetActive(false); display2G.SetActive(false);
    }

    //IMPORTANT
    //Find() can't find Inactive game objects
    void StatOnClick()
    {
        optionG.SetActive(false); statG.SetActive(true); display2G.SetActive(false);
    }

    void Display2OnClick()
    {
        optionG.SetActive(false); statG.SetActive(false); display2G.SetActive(true);
    }
    // Update is called once per frame
    void Update () {
        //Button ptionB = GameObject.FindGameObjectWithTag("Optionbutton").GetComponent<Button>();
        //Debug.Log(ptionB.name);
    }

    public static void ToggleMenuPanel(bool show)
    {
        if (menuPanel == null) return;
        if (show)
        {
            menuPanel.SetActive(true);
            
            /*foreach (Button b in Object.FindObjectsOfType<Button>())
            {
                b.enabled = true;  
                Debug.Log(b.name);
            }*/
            GameObject.Find("Options Button").GetComponent<Button>().Select();
        } else
        {
            menuPanel.SetActive(false);
           /*foreach (Button b  in Object.FindObjectsOfType<Button>())
            {
                Navigation navi = new Navigation();
                navi.mode = Navigation.Mode.None;
                b.navigation = navi;
                b.enabled = false;
                Debug.Log(b.name + "A");
            }*/
        }
    }
}
