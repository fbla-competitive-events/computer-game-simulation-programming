using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train : MonoBehaviour {
	public static float moveSpeed;
	private Rigidbody2D rigidBody;
	public GameObject location;
	public GameObject location2;
	private Transform myTrans;
	public bool isRight = false;
	public bool isLeft = false;
	public bool isUp = false;
	public bool isDown = false;
	public bool stop = false;
	public bool activate;
	//public Animator anim;

	void Awake(){
		myTrans = this.transform;	
		rigidBody = GetComponent<Rigidbody2D> ();
		activate = false;
	}

	void FixedUpdate(){
		if (activate == true) {
			if (isUp || isDown) {
				if (isUp) {
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, moveSpeed);
					if (myTrans.position.y > location.transform.position.y) {
						moveSpeed = 0;
						isDown = true;
						isUp = false;
					}
				}
				if (isDown) {
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, moveSpeed);
					if (myTrans.position.y < location2.transform.position.y) {
						moveSpeed = 0;
						isUp = true;
						isDown = false;
					}
				}
			}
		}
	}
}
