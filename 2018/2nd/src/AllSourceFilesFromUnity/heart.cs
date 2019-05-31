using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour {

	// Use this for initialization
	public Animator animator;

	public bool dead = false;
	// Update is called once per frame
	void Update () {
		if (dead) {
			animator.SetBool ("isDead", true);

		}
	}
}
