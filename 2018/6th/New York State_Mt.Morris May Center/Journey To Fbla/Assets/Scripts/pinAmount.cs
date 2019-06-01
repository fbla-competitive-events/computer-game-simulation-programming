using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinAmount : MonoBehaviour {

	public static int pinCount = 9;
	public GameObject pinZero;
	public GameObject pinOne;
	public GameObject pinTwo;
	public GameObject pinThree;
	public GameObject pinFour;
	public GameObject pinFive;
	public GameObject pinSix;
	public GameObject pinSeven;
	public GameObject pinEight;
	public GameObject pinNine;

	void Awake(){
		pinZero.SetActive (false);
		pinOne.SetActive (false);
		pinTwo.SetActive (false);
		pinThree.SetActive (false);
		pinFour.SetActive (false);
		pinFive.SetActive (false);
		pinSix.SetActive (false);
		pinSeven.SetActive (false);
		pinEight.SetActive (false);
		pinNine.SetActive (false);
	}

	void FixedUpdate(){
		if (pinCount == 9) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (true);
		}
		if (pinCount == 8) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (true);
			pinNine.SetActive (false);
		}
		if (pinCount == 7) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (true);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 6) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (true);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 5) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (true);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 4) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (true);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 3) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (true);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 2) {
			pinZero.SetActive (false);
			pinOne.SetActive (false);
			pinTwo.SetActive (true);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 1) {
			pinZero.SetActive (false);
			pinOne.SetActive (true);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
		if (pinCount == 0) {
			pinZero.SetActive (true);
			pinOne.SetActive (false);
			pinTwo.SetActive (false);
			pinThree.SetActive (false);
			pinFour.SetActive (false);
			pinFive.SetActive (false);
			pinSix.SetActive (false);
			pinSeven.SetActive (false);
			pinEight.SetActive (false);
			pinNine.SetActive (false);
		}
	}
}
