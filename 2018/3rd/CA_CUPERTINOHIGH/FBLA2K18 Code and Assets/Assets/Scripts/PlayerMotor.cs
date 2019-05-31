
using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {

    protected CharacterController controller;

   // public Animation run;

    protected Vector3 gravity = Vector3.zero;
    protected Vector3 move = Vector3.zero;
    private float moveSpeed;
    public float orgMoveSpeed = 10f;
    public float turnSpeed = 300f;
    public float jumpSpeed = 3;
    public float friction = 0.9f;
    protected bool jump;
    protected float camRotX = 0f;
    public float camPitchMax = 35f;

    private bool sprint = false;

    private Animator animator;
    private float stateValue = 0f;
    /*
     * 0 - idle
     * 0.1-0.4 walking
     * > 0.5 running
     * -0.1- -0.4 walking (backwards)
     * < -0.5 running(backwards)
     * 
     * */
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Debug.Log(GameOptions.pause);
        if (!GameOptions.pause)
        {
            //Debug.Log(Input.GetAxis("Horizontal"));
            move = Vector3.zero;
            /*
            if (Input.GetKeyDown("escape"))
            {
                Cursor.lockState = CursorLockMode.None;
            }*/

            //animation
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprint = !sprint;
            }

            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                stateValue = 0;
            }
            if (sprint)
            {
                moveSpeed = orgMoveSpeed * 2;
                stateValue = 1 * Input.GetAxis("Vertical");
            }
            else
            {
                moveSpeed = orgMoveSpeed;
                stateValue = 0.4f * Input.GetAxis("Vertical");
            }
            animator.SetFloat("MoveSpeed", stateValue);

            //movement
            
            float velX = 0; float velY = 0;
            if (Input.GetKey(KeyCode.W) && velY <= moveSpeed)
            {
                velY += moveSpeed / 3 * Time.deltaTime;
            } else if (Input.GetKey(KeyCode.S) && velY >= -moveSpeed)
            {
                velY -= moveSpeed / 3 * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) && velX <= moveSpeed)
            {
                velX += moveSpeed/ 3 * Time.deltaTime;
            } else if (Input.GetKey(KeyCode.A) && velX >= -moveSpeed)
            {
                velX -= moveSpeed / 3 * Time.deltaTime;
            }
            velY *= friction*Time.deltaTime; 
            velX *= friction*Time.deltaTime;
            //Debug.Log(velX + " " + velY);
            move = new Vector3(velX, 0f, velY);
            move.Normalize();



            //maybe disable jumping
            
            if (controller.isGrounded)
            {
               // if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    jump = true;
               // }
                gravity = Vector3.zero;
            }
            else
            {
                gravity += Physics.gravity * Time.deltaTime;
                //if (jump)
                //{
                 //   gravity.y = jumpSpeed;
                 //   jump = false;
                //}
            }
            move += gravity;
            
            //turning
            transform.Rotate(0, Input.GetAxis("Horizontal1") * turnSpeed * Time.deltaTime, 0f);

            camRotX -= Input.GetAxis("Vertical1") * 1.5f;
            camRotX = Mathf.Clamp(camRotX, -camPitchMax, camPitchMax);

            Camera.main.transform.forward = transform.forward;
            Camera.main.transform.Rotate(camRotX, 0f, 0f);
            //end turning
            move = transform.TransformDirection(move);
            controller.Move(move * moveSpeed * Time.deltaTime);
        }
       










        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(0, Vector3())
            //isJumping = true;
        }
        //if (verticalVelocity > 0) verticalVelocity -= gravity * Time.deltaTime;
        if (isJumping && verticalVelocity < jumpForce)
        {
            //verticalVelocity += speed;
        }
        if (verticalVelocity > 0)
        {
            //verticalVelocity -=
        }
/*
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector = lastMove;
        }
        float trans = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;
        trans *= Time.deltaTime; straffe *= Time.deltaTime;
        transform.Translate(straffe, 0, trans);
        /*
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= speed;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);
        lastMove = moveVector;
        */
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /*
        if (!controller.isGrounded && hit.normal.y < 0.1f)
         {
        if (Input.GetKeyDown(KeyCode.Space))
         {
                //Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                verticalVelocity = jumpForce;
                moveVector = hit.normal * speed;
            }
        }*/
    }
}
