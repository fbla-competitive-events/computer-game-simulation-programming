using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashPU : MonoBehaviour {

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player"){
				Destroy(gameObject);
				fblaPointManager.fblaPoints = fblaPointManager.fblaPoints + 4;
				moneyPointManager.moneyPoints = moneyPointManager.moneyPoints + 2;
		}
	}
}
