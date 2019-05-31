using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour {
	public bool facingRight = true; // For determining which way the player is currently facing.
	[SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
	[SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
	// Amount of maxSpeed applied to crouching movement. 1 = 100%

	[SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

	private Transform groundCheck; // A position marking where to check if the player is grounded.
	private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool grounded = false; // Whether or not the player is grounded.
	private Transform ceilingCheck; // A position marking where to check for ceilings
	private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	private Animator anim; // Reference to the player's animator component.


	private void Awake()
	{

		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponentInChildren<Animator>();
	}


	private void FixedUpdate()
	{
		
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);
		// Set the vertical animation
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
		if (!GameControl.control.isCharFlipCorrect) {
			Flip ();
			GameControl.control.isCharFlipCorrect = true;
		}

	}


	public void Move(float move, bool crouch, bool jump)
	{


		// If crouching, check to see if the character can stand up
		if (!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move*crouchSpeed : move);

			// The SpeGamed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));
			// Move the character
			GetComponent<Rigidbody2D>().velocity = new Vector2(move*maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (move < 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move > 0 && facingRight)
				// ... flip the player.
				Flip();
		}
		// If the player should jump...
		if (grounded && jump && anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			grounded = false;
			anim.SetBool("Ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
		}
	}


	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
