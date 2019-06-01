using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOff : MonoBehaviour {

	//the object thats gonna be used to trun off such as a timer when it needs to be deactivated
	public GameObject off;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && Input.GetKeyDown(KeyCode.Space)) {
			//deletes the selected gameobject given to off from the scene
			Destroy (off);
		}
	}
}
