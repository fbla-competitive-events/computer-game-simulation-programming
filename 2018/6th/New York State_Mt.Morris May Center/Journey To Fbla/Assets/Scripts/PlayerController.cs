using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float jumpForce = 3.8f;
    public float runningSpeed = 1.5f;
    public float maxDashSpeed = 10f;
    private Rigidbody2D rigidBody;
    public Animator anim;
    public float moveSpeed = 1.07f;
    float velX;
    float velY;
    bool facingRight = true;
    bool isJumping;
    bool isBinaryAttacking;
    bool jumpKeyHeld;
    bool spinAttack;
    public Vector2 counterJumpForce = new Vector2(0, -8);
    public ParticleSystem party;
    private Transform myTrans;
    private float myWidth;
    public float dashCoolDown = 0.0001f;
    //private float lastDash = 0f;
    public Transform projectileTip;
    public GameObject projectile;
   // float fireRate = 2f;
    //float nextFire = 0f;
	public static bool glitched;
	public GameObject retry;
	public static bool champ;





    void Awake()
    {

        rigidBody = GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
		glitched = false;
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            jumpKeyHeld = true;
            if (IsGrounded())
            {
                isJumping = true;
                rigidBody.AddForce(Vector2.up * jumpForce * rigidBody.mass, ForceMode2D.Impulse);
            } 
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
        {
            jumpKeyHeld = false;

        }


        //unlock after demo done
        /*if (IsGrounded())
        {
            anim.SetBool("jumpAttack", false);
            isBinaryAttacking = false;
        }*/



        
        if (!IsGrounded())
        {
            if (anim.GetBool("jumpAttack") == false)
            {
                anim.SetBool("jump", true);
            }

        }
        else
        {

            anim.SetBool("jump", false);
            //Time.timeScale = 1f;
        }


        //unlock after demo done
        /*if (!IsGrounded() && !isBinaryAttacking)
        {
            checkDash();
        }*/
        //unlock after demo done
        /*if (!IsGrounded() && Input.GetKeyDown(KeyCode.Z))
        {
            isBinaryAttacking = true;
            anim.SetBool("jumpAttack", true);
            Time.timeScale = 0.15f;
        }
        if (!IsGrounded() && Input.GetKeyUp(KeyCode.Z))
        {
            isBinaryAttacking = false;
            anim.SetBool("jumpAttack", false);
            Time.timeScale = 1f;
        }*/


        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetBool("spinAttack", true);
            //anim.SetBool("run", false);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            anim.SetBool("spinAttack", false);
        }


        moving();
        velX = Input.GetAxisRaw("Horizontal");
        velY = rigidBody.velocity.y;
        rigidBody.velocity = new Vector2(velX * moveSpeed, velY);


        //unlock after demo done
        //player shoot stuff
        /*if (Input.GetKeyUp(KeyCode.X) && !isBinaryAttacking)
        {
            fireProjectile();
        }
        else
        {
            anim.SetBool("isBlasting", false);
        }
        */

		if (glitched) {
			StartCoroutine (startGlitch ());
		}

		if (champ) {
			moveSpeed = 0;
			anim.SetBool("won", true);
		}
    }


    void FixedUpdate()
    {
        if (isJumping)
        {
            if (!jumpKeyHeld && Vector2.Dot(rigidBody.velocity, Vector2.up) > 0)
            {
                rigidBody.AddForce(counterJumpForce * rigidBody.mass);
            }
        }
    }


    void LateUpdate()
    {
        Vector3 localScale = transform.localScale;
        if (velX > 0)
        {
            facingRight = true;
        }
        else if (velX < 0)
        {
            facingRight = false;
        }

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }


    /*private void checkDash()
    {
        lastDash += Time.deltaTime;

        if (lastDash >= dashCoolDown && Input.GetKeyDown(KeyCode.Space))
        {
            party.Emit(50);
            lastDash = 0f;
            float dashSpeed = 1300;
            //rigidBody.velocity = new Vector2(maxDashSpeed * dashDirection, rigidBody.velocity.y);

            if (facingRight)
            {
                rigidBody.AddForce(transform.right * dashSpeed);
            }

            if (!facingRight)
            {
                rigidBody.AddForce(-transform.right * dashSpeed);
            }

        }
    }
    */

    void moving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
    }

	IEnumerator startGlitch(){
		moveSpeed = 0;
		anim.SetBool ("glitched", true);
		yield return new WaitForSeconds (3);
		Destroy (gameObject);
		retry.SetActive (true);

	}
    //unlock after demo done
    /*void fireProjectile()
    {
        if(Time.time > nextFire)
        {
            anim.SetBool("isBlasting", true);
            nextFire = Time.time + fireRate;
            if (facingRight)
            {
                Instantiate(projectile, projectileTip.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }else if (!facingRight)
            {
                Instantiate(projectile, projectileTip.position, Quaternion.Euler(new Vector3(0, 0, -180f)));
            }
        }
    }*/

    public LayerMask groundLayer;

    bool IsGrounded()
    {
        Vector2 RayCastPos1 = myTrans.position - myTrans.right * (myWidth - 0.11f);
        Vector2 RayCastPos2 = myTrans.position - myTrans.right * (myWidth - 0.21f);
        if (Physics2D.Raycast(RayCastPos1, Vector2.down, 0.2f, groundLayer.value) || Physics2D.Raycast(RayCastPos2, Vector2.down, 0.2f, groundLayer.value))
        {
            Debug.DrawRay(RayCastPos1, Vector2.down);
            Debug.DrawRay(RayCastPos2, Vector2.down);
            return true;

        }
        else
        {
            Debug.DrawRay(RayCastPos1, Vector2.down);
            Debug.DrawRay(RayCastPos2, Vector2.down);
            return false;
        }
    }
}
