using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ClearBugs : MonoBehaviour {
	public int index;
	public GameObject gameSelectionArrow;
	public GameObject selectGamePrompt;
	public GameObject panel;
	// Use this for initialization
	private void buttonClicked(){
		GameControl.control.latestCharPositionIndoors = PlatformerCharacter2D.control.transform.position;
		GameControl.control.latestScene = SceneManager.GetActiveScene().name;
		panel.GetComponent<FadeControl> ().levelChange ("bugcheck", panel);
	}	


	// Update is called once per frame
	void Update () {
		if (GameControl.control.currentGameButtonSelectionIndex == 0 && GameControl.control.isPCOpen&&GameControl.control.allGames.Count>0) {
			if(GameControl.control.selectedButton!=gameObject)
			GameControl.control.selectedButton = gameObject;
			if (Input.GetKeyDown (KeyCode.Return) && !GameControl.control.isClearBugsClicked) {
				
				gameSelectionArrow.GetComponent<CanvasGroup> ().alpha = 1;
				selectGamePrompt.GetComponent<CanvasGroup> ().alpha = 1;
				selectGamePrompt.GetComponent<TextMeshProUGUI> ().text = "Press '" + GameControl.control.cancelInput + "' to cancel selection";

				GameControl.control.isClearBugsClicked = true;

			}
			else if (Input.GetKeyDown (KeyCode.Return) && GameControl.control.isClearBugsClicked &&GameControl.control.allGames[GameControl.control.currentGameSelectionIndex].bugs>0) {
				if (GameControl.control.selectedGame != null) {
					GameControl.control.isClearBugsClicked = false;
					GameControl.control.isPCOpen = false;
					GameControl.control.gameCount = 0;
					buttonClicked ();
				}		
			}
			if (Input.GetKeyDown(GameControl.control.cancelInput) && GameControl.control.isClearBugsClicked) {
				gameSelectionArrow.GetComponent<CanvasGroup> ().alpha = 0;
				selectGamePrompt.GetComponent<CanvasGroup> ().alpha = 0;
				GameControl.control.isClearBugsClicked = false;

			}

		}
	}
}
