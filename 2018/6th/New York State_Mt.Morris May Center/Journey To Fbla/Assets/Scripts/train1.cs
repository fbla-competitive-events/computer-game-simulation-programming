using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train1 : MonoBehaviour {

	public static float moveSpeed;
	private Rigidbody2D rigidBody;
	public GameObject location;
	public GameObject location2;
	private Transform myTrans;
	public bool isRight = false;
	public bool isLeft = false;
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
			if (isRight || isLeft) {
				if (isRight) {
					rigidBody.velocity = new Vector2 (moveSpeed, rigidBody.velocity.y);
					if (myTrans.position.x > location.transform.position.x) {
						moveSpeed = 0;
						isLeft = true;
						isRight = false;
					}
				}
				if (isLeft) {
					rigidBody.velocity = new Vector2 (moveSpeed, rigidBody.velocity.y);
					if (myTrans.position.x < location2.transform.position.x) {
						moveSpeed = 0;
						isRight = true;
						isLeft = false;
					}
				}
			}
		}
	}
}
