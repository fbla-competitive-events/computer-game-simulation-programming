using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugAttack : MonoBehaviour {
	public GameObject player;



	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			PlayerController.glitched = true;
		}
	}
}
