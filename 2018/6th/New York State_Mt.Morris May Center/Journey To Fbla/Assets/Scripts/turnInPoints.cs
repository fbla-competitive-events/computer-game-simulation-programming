using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnInPoints : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && fblaPointManager.fblaPoints >= 42 & moneyPointManager.moneyPoints >= 42) {
			if (Input.GetKeyUp (KeyCode.Space)) {

			
				//fblaPointManager.fblaPoints = 0;
				//moneyPointManager.moneyPoints = 0;
			}
		}
	}
}
