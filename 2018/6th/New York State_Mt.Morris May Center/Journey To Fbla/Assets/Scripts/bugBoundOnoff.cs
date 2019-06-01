using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugBoundOnoff : MonoBehaviour {

	public GameObject bugField;
	public Animator anim;

	void Update(){
		if (glitchONoff.srtGlitch == true) {
			bugField.SetActive (true);
			anim.SetBool ("glitchIsOn", true);
		} else {
			bugField.SetActive (false);
			anim.SetBool ("glitchIsOn", false);
		}
	}
}
