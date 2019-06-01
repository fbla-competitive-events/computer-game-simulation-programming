using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinPickUp : MonoBehaviour {
	public GameObject pin;
	public GameObject pinHoverSymbol;

	void Awake(){
		pin.SetActive (false);
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			if (Input.GetKeyUp (KeyCode.Space)) {
				pin.SetActive (true);
				pinHoverSymbol.SetActive (false);
				pinAmount.pinCount = pinAmount.pinCount - 1;
			}
		}
	}
}
