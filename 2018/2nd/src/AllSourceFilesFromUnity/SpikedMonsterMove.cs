using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedMonsterMove : MonoBehaviour {
	public float direction;
	public float moveSpeed;
	public float realSpeed;
	public float valueMultiplier = 1;
	public bool facingRight=false;
	public Animator animator;
	public float collisionCountDown=3f;
	public bool isCollided;
	private float leftBound;
	private float rightBound;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10f);
		leftBound = transform.position.x;
		rightBound = leftBound + 6f;
	}


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.name == "Character" && !isCollided) {

			lifeManagerBugCheck.control.removeLife ();
			collisionCountDown = 3f;
			isCollided = true;
			animator.SetBool ("isCollided", true);


			//GetComponent<Rigidbody2D> ().MovePosition (transform.forward * -5);
		}
	}
	void Flip()
	{
		GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
		direction *= -1;
	}
	// Update is called once per frame
	void Update () {
		if (isCollided) {
			if (collisionCountDown > 0) {
				collisionCountDown -= Time.deltaTime;
			} else {
				isCollided = false;
				animator.SetBool ("isCollided", false);
			}
		}
		if (valueMultiplier * transform.position.x > rightBound && facingRight) {
			
			Flip ();
			facingRight = false;
		} else if (valueMultiplier * transform.position.x < leftBound && !facingRight) {
			
			Flip ();
			facingRight = true;

		} 

			Vector3 pos = transform.position;
			pos.y = Mathf.Clamp(pos.y,1.44f,1.66f);
			transform.position = pos;
		

			GetComponent<Rigidbody2D>().velocity=new Vector2(direction*moveSpeed,GetComponent<Rigidbody2D>().velocity.y);
		//
}
}
