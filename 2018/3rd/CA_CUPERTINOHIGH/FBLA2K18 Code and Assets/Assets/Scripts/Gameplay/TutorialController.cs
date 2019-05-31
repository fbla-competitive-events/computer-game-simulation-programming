using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public TextBoxManager t;

    public GameObject tutorialObj;
    protected Introduction intro;
	// Use this for initialization
	void Start () {
       // intro = GetComponent<Introduction>();
        //intro.enabled = false;
        t = GameObject.Find("Text Box Manager").GetComponent<TextBoxManager>();
        
        tutorialObj = GameObject.Find("Tutorial OBJ");
        tutorialObj.SetActive(false);
    }


    public bool showStart = true;
    public bool showTutorialText = true;
    public bool doneTutorial = false;
    public bool interactBoard = false;
    public bool finishTutorial = false;
    bool showIntro = true;
    // Update is called once per frame
    void Update () {
		if (showStart)
        {
            StartTutorial();
        }
        if (doneTutorial)
        {
            WaitForInteract();            
        }
        if (finishTutorial)
        {
            //intro.enabled = true;
            //enabled = false;
            //Debug.Log("A");
            ShowIntro();
        }


	}

    private void ShowIntro()
    {
        //make 'Show Intro' text file
        Debug.Log(t.finishFile + " " + showStart);
        if (t.finishFile && showIntro)
        {
            //Debug.Log("Aafafd");
            t.SetText("Show Intro");
            //Debug.Log("Aasafasf");
            showIntro = false;
        }
        if (t.finishFile && !showIntro)
        {
            //t.SetText("Tutorial Door Text");
            finishTutorial = false;
        }
    }

    private void WaitForInteract()
    {
        if (t.finishFile == false)
        {
            interactBoard = true;
        }
        if (t.finishFile && interactBoard)
        {
            GameObject.Find("Tutorial Door").gameObject.tag = "Objective";
            if (t.textFileName.Equals("Tutorial Door Text.txt"))
            {
                if (t.finishFile)
                {
                    GameObject.Find("Tutorial Box").SetActive(false);
                    ObjectivePlayerController src = GetComponent<ObjectivePlayerController>();
                    src.isInteractable = false;
                    doneTutorial = false;
                    finishTutorial = true;
                }
            }
        }
    }

    private void StartTutorial()
    {
        if (showTutorialText)
        {
            t.SetText("tutorialText");
            showTutorialText = false;
        }
        if (t.finishFile)
        {
            tutorialObj.SetActive(true);
            showStart = false;
            doneTutorial = true;
        }
    }
}
