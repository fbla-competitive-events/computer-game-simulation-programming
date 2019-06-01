using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxActivityStarter : MonoBehaviour {

	public GameObject startActivity;
	public bool activityStarted = false;
	public GameObject BoxHud;
	public GameObject spaceHudOn;


	void Start(){
		startActivity.SetActive (false);
		BoxHud.SetActive (false);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		spaceHudOn.SetActive (true);
		if (other.tag == "Player" && !activityStarted) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				startActivity.SetActive (true);
				activityStarted = true;
				BoxHud.SetActive (true);
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		spaceHudOn.SetActive (false);

	}
}
