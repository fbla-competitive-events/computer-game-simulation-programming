using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapSwitch : MonoBehaviour {

	public GameObject map1;
	public GameObject map2;
	private int sideCount;

	void Update(){
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			sideCount = sideCount + 1;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			sideCount = sideCount - 1;
		}
		if (sideCount >= 2) {
			sideCount = 2;
		}
		if (sideCount == 0) {
			map1.SetActive (false);
			map2.SetActive (false);
		}
		if (sideCount <= 0) {
			sideCount = 0;
		}
		if (sideCount == 1) {
			map1.SetActive (true);
			map2.SetActive (false);
		}
		if (sideCount == 2) {
			map2.SetActive (true);
		}
	}
}
