using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graffiti : MonoBehaviour {

	public GameObject badArt;
	public GameObject spaceHudOn;

	void OnTriggerStay2D(Collider2D other)
	{
		spaceHudOn.SetActive (true);
		if (other.tag == "Player") {
			if (Input.GetKeyUp (KeyCode.Space)) {
				Destroy(gameObject);
				fblaPointManager.fblaPoints = fblaPointManager.fblaPoints + 2;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		spaceHudOn.SetActive (false);

	}
}
