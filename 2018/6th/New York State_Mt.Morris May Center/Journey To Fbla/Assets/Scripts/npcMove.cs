using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMove : MonoBehaviour {

	public float moveSpeed;
	private Rigidbody2D rigidBody;
	public GameObject location;
	public GameObject location2;
	private Transform myTrans;
	public bool isRight;
	public bool isLeft;
	public Animator anim;

	void Awake(){
		myTrans = this.transform;	
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update(){
		if (myTrans.position.x < location.transform.position.x && isRight) {
			//myTrans.position = new Vector2 (moveSpeed, rigidBody.velocity.y);
			rigidBody.velocity = new Vector2 (moveSpeed, rigidBody.velocity.y);
			anim.SetBool ("Right", true);
			anim.SetBool ("Left", false);
			anim.SetBool ("Run", true);

		}else if (myTrans.position.x > location.transform.position.x) {
			isRight = false;
			rigidBody.velocity = new Vector2 (-moveSpeed, rigidBody.velocity.y);
			anim.SetBool ("Right", false);
			anim.SetBool ("Left", true);
			anim.SetBool ("Run", true);
		}
		if (myTrans.position.x > location2.transform.position.x && isLeft) {
			//myTrans.position = new Vector2 (moveSpeed, rigidBody.velocity.y);
			rigidBody.velocity = new Vector2 (-moveSpeed, rigidBody.velocity.y);
			anim.SetBool ("Right", false);
			anim.SetBool ("Left", true);
			anim.SetBool ("Run", true);
		}
		if (myTrans.position.x < location2.transform.position.x) {
			isLeft = false;
			rigidBody.velocity = new Vector2 (moveSpeed, rigidBody.velocity.y);
			anim.SetBool ("Right", true);
			anim.SetBool ("Left", false);
			anim.SetBool ("Run", true);
		}
		/*if (myTrans.position.x != location2.transform.position.x) {
			//myTrans.position = new Vector2 (moveSpeed, rigidBody.velocity.y);
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, -moveSpeed);
		}
		if (myTrans.position.x == location2.transform.position.x) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 0f);

		}*/
	}
}


