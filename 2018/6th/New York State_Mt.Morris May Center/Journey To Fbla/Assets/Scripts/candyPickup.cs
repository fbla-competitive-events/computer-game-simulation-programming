using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candyPickup : MonoBehaviour {

	public GameObject moneySymbol;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.Space)) {
				moneySymbol.SetActive (false);
				moneyPointManager.moneyPoints = moneyPointManager.moneyPoints + 7;
			}
		}
	}
}
