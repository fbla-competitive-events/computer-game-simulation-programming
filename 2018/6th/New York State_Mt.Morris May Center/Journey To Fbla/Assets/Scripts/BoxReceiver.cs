using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxReceiver : MonoBehaviour {

	public GameObject BoxHud;
	public GameObject spaceHudOn;

	void OnTriggerStay2D(Collider2D other)
	{
		spaceHudOn.SetActive (true);
		if (other.tag == "Player" && BoxHud.activeInHierarchy) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				BoxHud.SetActive (false);
				fblaPointManager.fblaPoints = fblaPointManager.fblaPoints + 10;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		spaceHudOn.SetActive (false);

	}
}
