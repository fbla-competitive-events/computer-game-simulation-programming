using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainSwitch2 : MonoBehaviour {

	public GameObject currentTrain;
	private GameObject cTrain;
	public bool right;
	public bool left;


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			if (Input.GetKeyUp (KeyCode.Space)) {

				currentTrain.GetComponent<train1> ().activate = true;
				if (right) {
					currentTrain.GetComponent<train1> ().isRight = true;
					train1.moveSpeed = 8;

				} 
				if (!right) {
					currentTrain.GetComponent<train1> ().isRight = false;
				}
				if (left) {
					currentTrain.GetComponent<train1> ().isLeft = true;
					train1.moveSpeed = -8;
				} 
				if (!left) {
					currentTrain.GetComponent<train1> ().isLeft = false;
				}
			}
		}
	}
}
