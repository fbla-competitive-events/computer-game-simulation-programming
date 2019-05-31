using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
The following class demonstrates the back end for the dialog interface of the teacher in the game.
It takes is various dynamic keyboard input, freezes the character movement, controls prompt text, and displays dialog menu
*/
public class Teacher : MonoBehaviour {

	public GameObject dialogDisplay;
	public TextMeshProUGUI prompt;
	public GameObject panel;
	public GameObject game;
	public GameObject GameSelectionArrow;
	public GameObject gameContainer;
	public CanvasGroup promptCanvasGroup;
	public GameObject scrollRect;
	// Use this for initialization
	void OnTriggerEnter2D()
	{
		promptCanvasGroup.alpha = 1;
	}
	void OnTriggerExit2D()
	{
		promptCanvasGroup.alpha = 0;

	}
	void Awake () {
		promptCanvasGroup=prompt.GetComponent<CanvasGroup>();
		prompt.text = "Press '" + GameControl.control.eInput + "' to talk to your teacher";

	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) ) {
			if (!GameControl.control.isDialogOpen && promptCanvasGroup.alpha == 1) {

				dialogDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", true);
				promptCanvasGroup.alpha = 0;
				GameControl.control.isDialogOpen = true;
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;
				//panel.GetComponent<FadeControl> ().levelChange ("programming", panel);
			} else if (GameControl.control.isDialogOpen) {
				dialogDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", false);

				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;

				promptCanvasGroup.alpha = 1;
				GameControl.control.isDialogOpen = false;

			}
		}
	}
}
