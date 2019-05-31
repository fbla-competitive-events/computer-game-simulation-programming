using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
/*
The following class contains the actions used by the submit button for FBLA comptettive events
If the user is currently in competition, it simply displays text indiciated inability to compete.
else, submit the selected game, enable compeition variable, and provide submission receipt.
*/
public class SubmitGameButton : MonoBehaviour {
	public TextMeshProUGUI text;
	public bool submitted =false;
	public GameObject panel;	// Use this for initialization
	void Start () {
		if (!GameControl.control.canCompete) {
			text.text="Sorry, you can't compete again yet. If you haven't already, go to your home to view your last competition results.";
		}

	}
	
	// Update is called once per frame
	void Update () {
		
		if (!submitted&& GameControl.control.allGames.Count > 0 && Input.GetKeyDown (KeyCode.Return) &&GameControl.control.canCompete) {
			text.text = "Game Submitted! You can compete again 15 minutes after going home and viewing your results. Press '"+GameControl.control.backInput+"' to return";
			GameControl.control.rating=GameControl.control.allGames [GameControl.control.currentGameSelectionIndex].rating;
			GameControl.control.isCompeting = true;
			GameControl.control.canCompete=false;
			submitted = true;
		}
		else if(Input.GetKeyDown(GameControl.control.backInput))
		{
			panel.GetComponent<FadeControl> ().levelChange ("fblabuilding", panel);	
		}
	}
}
