using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class irisGlitchAttack : MonoBehaviour {

	public GameObject irisField;

	void Update(){
		if (glitchONoff.srtGlitch == true) {
			irisField.SetActive (false);
		} else {
			irisField.SetActive (true);
		}
	}
}
