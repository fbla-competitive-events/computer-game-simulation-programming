using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pointDropOff : MonoBehaviour {
	public GameObject speech;
	public GameObject spaceHud;
	public GameObject speech1;

	void Start(){
		speech.SetActive (false);
		speech1.SetActive (false);
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (fblaPointManager.fblaPoints >= 42 && moneyPointManager.moneyPoints >= 42) {
			spaceHud.SetActive (true);
			speech.SetActive (true);
		}

		if (other.tag == "Player" && fblaPointManager.fblaPoints >= 42 && moneyPointManager.moneyPoints >= 21 && moneyPointManager.moneyPoints < 42) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				speech1.SetActive (true);
				//fblaPointManager.fblaPoints = 0;
				//moneyPointManager.moneyPoints = 0;
			}
			if (Input.GetKeyDown (KeyCode.O) && Input.GetKeyDown (KeyCode.K)) {
				speech1.SetActive (false);
				speech.SetActive (true);
				fblaPointManager.fblaPoints = 0;
				moneyPointManager.moneyPoints = 0;
			}
		}
		if (other.tag == "Player" && fblaPointManager.fblaPoints >= 42 &&  moneyPointManager.moneyPoints >= 42) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				speech.SetActive (true);
				fblaPointManager.fblaPoints = 0;
				moneyPointManager.moneyPoints = 0;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		spaceHud.SetActive (false);
	}


}
