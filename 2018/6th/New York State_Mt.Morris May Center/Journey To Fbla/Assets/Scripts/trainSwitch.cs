using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainSwitch : MonoBehaviour {

	public GameObject currentTrain;
	private GameObject cTrain;
	public bool right;
	public bool left;
	public bool up;
	public bool down;


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			if (Input.GetKeyUp (KeyCode.Space)) {

				currentTrain.GetComponent<train> ().activate = true;
				if (up) {
					currentTrain.GetComponent<train> ().isUp = true;
					train.moveSpeed = 8;
					
				} 
				if (!up) {
					currentTrain.GetComponent<train> ().isUp = false;
				}
				if (down) {
					currentTrain.GetComponent<train> ().isDown = true;
					train.moveSpeed = -8;
				} 
				if (!down) {
					currentTrain.GetComponent<train> ().isDown = false;
				}
			}
		}
	}
}
