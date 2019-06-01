using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugDefense : MonoBehaviour {


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "attackBound" && glitchONoff.srtGlitch == false) {
			bugPointManager.bugPoints = bugPointManager.bugPoints + 1;
			Destroy (gameObject);
		}
	}
}
