using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingBug : MonoBehaviour {

	private PlayerController player;

	public GameObject buddy;

	public float moveSpeed;
	public float savedSpeed;
	public float bonusSpeed;

	public float playerRange;

	void Start(){
		player = FindObjectOfType<PlayerController> ();
		savedSpeed = moveSpeed;
	}

	void Update(){
		if (glitchONoff.srtGlitch == true) {
			moveSpeed = bonusSpeed;
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, moveSpeed * Time.deltaTime);
		} else {
			moveSpeed = savedSpeed;
			transform.position = Vector3.MoveTowards (transform.position, buddy.transform.position, moveSpeed * Time.deltaTime);
		}
	}
}
