using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glitchONoff : MonoBehaviour {

	public GameObject glitch;
	public static bool srtGlitch = true;
	private bool restart = false;

	void Start(){
		
	}
	void Update(){
		if (!restart) {
			StartCoroutine (startGlitch ());
		}
		if (srtGlitch) {
			glitch.SetActive (true);
		}
		if (!srtGlitch) {
			glitch.SetActive (false);
		}
	}
		
	IEnumerator startGlitch(){
		srtGlitch = false;
		restart = true;
		yield return new WaitForSeconds (5);
		srtGlitch = true;
		yield return new WaitForSeconds (5);
		restart = false;

	}
}
