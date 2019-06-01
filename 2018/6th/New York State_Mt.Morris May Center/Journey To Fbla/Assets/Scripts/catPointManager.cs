using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catPointManager : MonoBehaviour {

	public static int speechPoints;
	public GameObject speech2;
	public GameObject speech3;

	void Start(){
		speech3.SetActive (false);
		speechPoints = 0;
	}

	void Update(){
		if (speechPoints == 5) {
			speech2.SetActive (false);
			speech3.SetActive (true);
		}
	}
}
