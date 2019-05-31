using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fundraise : MonoBehaviour {

	// Use this for initialization
	public GameObject panel;
	// Use this for initialization
	private void buttonClicked(){
		GameControl.control.gameCount = 0;
		GameControl.control.latestCharPositionIndoors = PlatformerCharacter2D.control.transform.position;
		GameControl.control.latestScene = SceneManager.GetActiveScene().name;
		GameControl.control.isPCOpen = false;

		panel.GetComponent<FadeControl> ().levelChange ("fundraise", panel);
	}
	void Update()
	{
		if (GameControl.control.currentDialogButtonSelectionIndex == 1 && GameControl.control.isDialogOpen) {
			if(GameControl.control.selectedDialogButton!=gameObject)
				GameControl.control.selectedDialogButton = gameObject;
			if (Input.GetKeyDown (KeyCode.Return)) {
				buttonClicked ();
			}
		}
	}
}
