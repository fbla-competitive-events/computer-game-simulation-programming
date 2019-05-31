using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GenGameFlow : MonoBehaviour {
	public TextMeshProUGUI textToActiv;
	public CanvasGroup text;
	public Animator animator;
	public bool isOpen;

	// Use this for initialization
	void Start () {
		textToActiv.text="Press '"+GameControl.control.eInput+"' to view game flow help";
		if (!GameControl.control.hasMainHelpMenuBeenSeen) {
			text.alpha = 1;
			GameControl.control.hasMainHelpMenuBeenSeen = true;
		} else {
			text.alpha = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput)) {
			if (isOpen) {

				animator.SetBool ("isZoomedIn", false);
				isOpen = false;
			} else if (!isOpen && text.alpha == 1) {
				animator.SetBool ("isZoomedIn", true);
				text.alpha = 0;

				isOpen = true;
			}
		} else if (Input.anyKey) {
			text.alpha = 0;
		}
}
}
