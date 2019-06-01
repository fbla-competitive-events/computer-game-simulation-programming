/*
 
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
    bool jumpKeyHeld;
    public Vector2 counterJumpForce = new Vector2(0, -8);
    public ParticleSystem party;
    private Transform myTrans;
    private float myWidth;
    public float dashCoolDown = 0.0001f;
    private float lastDash = 0f;
    



    void Awake()
    {
        
        rigidBody = GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        //Player = GetComponent<Transform>();
    }

    /*IEnumerator DoJump()
   // {
        //anim.SetBool("jump", true);
       // rigidBody.AddForce(Vector2.up * jumpForce);

       // yield return null;

       // float currentForce = jumpForce;

       // while (Input.GetKey(KeyCode.Space) && currentForce > 0 || Input.GetButton("Jump") && currentForce > 0)
       // {
         //   rigidBody.AddForce(Vector2.up * currentForce);

          //  currentForce -= decayRate * Time.deltaTime;
         //   yield return null;
      //  }
    //}

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
        else if (!IsGrounded())
        {
            anim.SetBool("jumpAttack", true);
            Time.timeScale = 0.15f;
            //party.Emit(50); use on dash insted

        }
    }
    else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Jump"))
    {
        jumpKeyHeld = false;

    }

    if (IsGrounded())
    {
        anim.SetBool("jumpAttack", false);
    }




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
        Time.timeScale = 1f;
    }
    if (!IsGrounded())
    {
        checkDash();
    }



    //allTheJumpCrap();


    //newAllTheJumpCrap();
    moving();
    velX = Input.GetAxisRaw("Horizontal");
    velY = rigidBody.velocity.y;
    rigidBody.velocity = new Vector2(velX * moveSpeed, velY);
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
    //Vector2 lineCastPos = myTrans.position - myTrans.right * (myWidth - 0.11f);
    //Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
    //isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, groundLayer);
}

/* void newAllTheJumpCrap()
// {
   //  if (IsGrounded())
   //  {
        // anim.SetBool("jump", false);
        // if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        // {
       //      StartCoroutine(DoJump());
        // }
    // }
    // else
    // {
    //     anim.SetBool("jump", true);
    // }
// }

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




/*void jump()
{
    //anim.SetBool("jump", true);
    rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    anim.SetBool("jump", true);
    //startAnim();
}


void allTheJumpCrap()
{
    if (IsGrounded())
    {
        anim.SetBool("jump", false);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
    }
    else
    {
        anim.SetBool("jump", true);

    }
}
// dot thing ends here

private void checkDash()
{
    lastDash += Time.deltaTime;

    if (lastDash >= dashCoolDown && Input.GetKeyDown(KeyCode.C))
    {
        party.Emit(50);
        lastDash = 0f;
        //I've pasted some of your dash code here:
        //float dashDirection = Input.GetAxisRaw("Horizontal");
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
//void startAnim()
//{

//}

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
*/