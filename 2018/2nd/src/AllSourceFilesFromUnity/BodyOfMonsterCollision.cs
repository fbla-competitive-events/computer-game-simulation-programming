using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyOfMonsterCollision : MonoBehaviour {
	private bool isDead;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (!isDead) {
			if (other.gameObject.name == "Character") {
				Destroy (transform.parent.gameObject);
				Instantiate (EnemyManager.control.deathEffect, transform.parent.position, Quaternion.identity);
				lifeManagerBugCheck.control.removeLife ();
				isDead = true;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
