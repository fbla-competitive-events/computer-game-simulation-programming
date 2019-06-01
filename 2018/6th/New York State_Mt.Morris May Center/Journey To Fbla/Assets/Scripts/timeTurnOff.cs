using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeTurnOff : MonoBehaviour {
	//script used to turn off time while talking to npcs

	void Update(){
		
		if (!DialogueManager.dialogueOn) {
			GetComponent <countDownClock> ().enabled = true;
		} else if (DialogueManager.dialogueOn) {
			GetComponent <countDownClock> ().enabled = false;
		}

		if (GetComponent <countDownClock> ().enabled == false) {
			countDownClock.on = true;
			countDownClock.go = true;
		} else {
			countDownClock.on = false;
		}
	}
}
