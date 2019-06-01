using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class topDownMovement : MonoBehaviour {

	public float moveSpeed;
	public float extraSpeed;
	private Rigidbody2D rigidBody;
	private Vector2 lastMove;
	public Animator anim;
	public GameObject pinTradingScreen;
	private bool spaceOn;

	void Awake(){
		rigidBody = GetComponent<Rigidbody2D>();
		pinTradingScreen.SetActive (false);
		extraSpeed = moveSpeed;
	}

	void Update()
	{
		if (DialogueManager.dialogueOn == true) {
			moveSpeed = 0;
		} else if (DialogueManager.dialogueOn == false && !spaceOn){
			moveSpeed = extraSpeed;
		}
		//Movement
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) {
			rigidBody.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, rigidBody.velocity.y);
			lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, 0f);
		}

		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, Input.GetAxisRaw ("Vertical") * moveSpeed);
			lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical") * moveSpeed);
		}

		if (Input.GetAxisRaw ("Horizontal") < 0.5f && Input.GetAxisRaw ("Horizontal") > -0.5f) {
			rigidBody.velocity = new Vector2 (0f, rigidBody.velocity.y);
		}

		if (Input.GetAxisRaw ("Vertical") < 0.5f && Input.GetAxisRaw ("Vertical") > -0.5f) {
			rigidBody.velocity = new Vector2 (rigidBody.velocity.x, 0f);
		}


		AnimationController();
		pinTradingMenu();
		moving();
		//running ();

	}
	void moving()
	{
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			anim.SetBool("Run", true);
		}
		else
		{
			anim.SetBool("Run", false);
		}
	}

	void AnimationController(){
		//Right
		if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			anim.SetBool ("Right", true);
		} else {
			if (Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.RightArrow)) {

				anim.SetBool ("Right", false);
			}
		}

		//Left
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
			anim.SetBool ("Left", true);
		} else {
			if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.LeftArrow)) {

				anim.SetBool ("Left", false);
			}
		}

		//Up
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			anim.SetBool ("Up", true);
		} else {
			if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow)) {

				anim.SetBool ("Up", false);
			}
		}

		//Down
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			anim.SetBool ("Down", true);
		} else {
			if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow)) {

				anim.SetBool ("Down", false);
			}
		}
	}

	void pinTradingMenu(){
		if (Input.GetKeyDown (KeyCode.M) && pinTradingScreen.activeInHierarchy == false) {
			pinTradingScreen.SetActive (true);
			Time.timeScale = 0;
		}else if (Input.GetKeyDown (KeyCode.M) && pinTradingScreen.activeInHierarchy == true) {
			pinTradingScreen.SetActive (false);
			moveSpeed = extraSpeed;
			Time.timeScale = 1;
		}
		
	}

	/*void running(){
		if (Input.GetKeyDown(KeyCode.Space)) {
			spaceOn = true;
			moveSpeed = moveSpeed + 1;
		}else if (Input.GetKeyUp (KeyCode.Space)) {
			moveSpeed = extraSpeed;
			spaceOn = false;
		}

	}*/
}
