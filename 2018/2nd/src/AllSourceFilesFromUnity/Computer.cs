using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Computer : MonoBehaviour {
	public GameObject computerDisplay;
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
		prompt.GetComponent<TextMeshProUGUI> ().text = "Press '" + GameControl.control.eInput + "' to access PC";

	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) ) {
			if (!GameControl.control.isPCOpen && promptCanvasGroup.alpha == 1) {
				
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;
				//GameControl.control.latestCharPositionInScene = transform.position;
				//SGameControl.control.latestScene = SceneManager.GetActiveScene().name;
				while (GameControl.control.gameCount < GameControl.control.allGames.Count) {
					GameObject gameObj = Instantiate (game, new Vector3 (0, 0), Quaternion.identity, gameContainer.transform);
					gameObj.GetComponent<GameSelectionObject> ().index = GameControl.control.gameCount;
					GameControl.control.gameCount++;

				}
				computerDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", true);
		
				promptCanvasGroup.alpha = 0;
				GameControl.control.isPCOpen = true;
				GameSelectionArrow.transform.SetAsLastSibling ();
				if(GameControl.control.isClearBugsClicked)
				GameSelectionArrow.transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x - 5, GameControl.control.selectedGame.transform.position.y);

				//panel.GetComponent<FadeControl> ().levelChange ("programming", panel);
			} else if (GameControl.control.isPCOpen) {
				computerDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", false);
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;


				promptCanvasGroup.alpha = 1;
				GameControl.control.isPCOpen = false;
			
			}
		}
	}
}
