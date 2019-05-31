using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class shopGuy : MonoBehaviour {


	public GameObject dialogDisplay;
	public TextMeshProUGUI prompt;
	public GameObject panel;
	public GameObject item;
	public GameObject itemSelectionArrow;
	public GameObject itemContainer;
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
		prompt.text = "Press '" + GameControl.control.eInput + "' to talk to the vendor";
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) ) {
			if (!GameControl.control.isDialogOpen && promptCanvasGroup.alpha == 1) {
				GameControl.control.isShopOpen = true;
				dialogDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", true);
				promptCanvasGroup.alpha = 0;
				GameControl.control.isDialogOpen = true;
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;
				//panel.GetComponent<FadeControl> ().levelChange ("programming", panel);
			} else if (GameControl.control.isDialogOpen && !GameControl.control.isShopItselfOpen) {
				GameControl.control.isShopOpen = false;

				dialogDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", false);

				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;

				promptCanvasGroup.alpha = 1;
				GameControl.control.isDialogOpen = false;

			}
		}
	}
}
