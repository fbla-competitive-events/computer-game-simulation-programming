using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonListener : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => OnClick());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        if (GetComponent<Button>().isActiveAndEnabled)
        {
            GameObject.Find("Game Controller").GetComponent<GameAndPlayerManager>().SaveAndQuit();
        }
    }
}
