using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewGame : MonoBehaviour {
	public GameObject panel;
	// Use this for initialization
	private void buttonClicked(){
		GameControl.control.gameCount = 0;
		GameControl.control.latestCharPositionIndoors = PlatformerCharacter2D.control.transform.position;
		GameControl.control.latestScene = SceneManager.GetActiveScene().name;
		GameControl.control.isPCOpen = false;

		panel.GetComponent<FadeControl> ().levelChange ("programming", panel);
	}
	 void Update()
	{
		if (GameControl.control.currentGameButtonSelectionIndex == 1 && GameControl.control.isPCOpen) {
			if(GameControl.control.selectedButton!=gameObject)
			GameControl.control.selectedButton = gameObject;
			if (Input.GetKeyDown (KeyCode.Return)) {
				buttonClicked ();
			}
		}
	}
}
