using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopOfMonsterCollision : MonoBehaviour {
	private bool isDead;
	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (!isDead) {
			if (other.gameObject.name == "Character") {
				isDead = true;

				Destroy (transform.parent.gameObject, .1f);
				Instantiate (EnemyManager.control.deathEffect, transform.parent.position, Quaternion.identity);
				EnemyManager.control.score++;
				if (EnemyManager.control.numberOfBugs == 0) {
					EnemyManager.control.gameOverWin ();
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
