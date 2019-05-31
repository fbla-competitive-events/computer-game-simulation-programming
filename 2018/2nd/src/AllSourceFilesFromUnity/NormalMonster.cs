using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : MonoBehaviour {
	public bool facingRight;
	public int direction;
	public float moveSpeed;
	public bool isAlive;
	private float leftBound;
	private float rightBound;
	// Use this for initialization
	void Start () {
		leftBound = transform.position.x;
		rightBound= leftBound + 6f;
	}
	
	void Flip()
	{
		GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
		direction *= -1;
	}
	// Update is called once per frame
	void Update () {
		if (transform.position.x > rightBound && facingRight) {

			Flip ();
			facingRight = false;
		}
		if(transform.position.x < leftBound&& !facingRight)
		{

			Flip ();
			facingRight = true;

		}

		Vector3 pos = transform.position;
		pos.y = Mathf.Clamp(pos.y,1.02f,1.23f);
		transform.position = pos;

		GetComponent<Rigidbody2D>().velocity=new Vector2(direction*moveSpeed,GetComponent<Rigidbody2D>().velocity.y);
		//
	}
}
