using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activityStarter : MonoBehaviour {
	public GameObject startActivity;
	public bool activityStarted = false;
	public GameObject hud;

	void Start(){
		startActivity.SetActive (false);
		hud.SetActive (false);
		catPoints.cat_Points = 0;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && !activityStarted) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				startActivity.SetActive (true);
				activityStarted = true;
				hud.SetActive (true);
			}
		}
		if (other.tag == "Player" && activityStarted && catPoints.cat_Points >= 5) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				fblaPointManager.fblaPoints = fblaPointManager.fblaPoints + 5;
				catPoints.cat_Points = 0;
				hud.SetActive (false);
			}
		}

	}
}
