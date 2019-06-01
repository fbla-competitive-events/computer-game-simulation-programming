using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class countDownClock : MonoBehaviour {

	public int timeLeft = 5;

	public Text countdownText;

	public string restart;

	public static bool on;

	public static bool go;

	void Start(){
		StartCoroutine ("LoseTime");
	}

	void Update(){
		countdownText.text = ("" + timeLeft);

		if (timeLeft <= 0) {
			StopCoroutine ("LoseTime");
			countdownText.text = "Times Up!";
			SceneManager.LoadScene (restart);
		}
		if (!on && go) {
			StartCoroutine ("LoseTime");
			go = false;
		}


	}

	IEnumerator LoseTime(){
		while (true) {
			yield return new WaitForSeconds (1);

			timeLeft--;
			if (on) {
				StopCoroutine ("LoseTime");
			}
		}

	}
}


