using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateAttack : MonoBehaviour {

	public GameObject characterBoundary;

	void Update(){
		Attack ();
	}

	void Attack (){
		if (Input.GetKey (KeyCode.C) && glitchONoff.srtGlitch == false) {
			characterBoundary.SetActive (true);
		} else if (Input.GetKeyUp (KeyCode.C)) {
			characterBoundary.SetActive (false);
		}
	}
}
