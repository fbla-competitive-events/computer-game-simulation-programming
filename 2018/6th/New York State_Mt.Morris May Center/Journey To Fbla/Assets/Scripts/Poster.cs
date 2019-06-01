using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poster : MonoBehaviour {

	public GameObject poster;
	GameObject posterPlaced;
	public Transform posterSpot;
	public GameObject spaceHudOn;

	void Start() {
	}
	void spawnPoster()
	{
		posterPlaced = Instantiate(poster, posterSpot.position, Quaternion.identity) as GameObject;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			spaceHudOn.SetActive (true);
			if (Input.GetKeyUp (KeyCode.Space)) {
				spawnPoster ();
				Destroy(gameObject);
				fblaPointManager.fblaPoints = fblaPointManager.fblaPoints + 1;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
			spaceHudOn.SetActive (false);
		
	}
}
