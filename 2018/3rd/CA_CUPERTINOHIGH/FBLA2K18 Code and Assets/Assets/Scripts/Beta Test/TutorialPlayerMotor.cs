using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerMotor : MonoBehaviour {
    protected CharacterController controller;

    // public Animation run;

    protected Vector3 gravity = Vector3.zero;
    protected Vector3 move = Vector3.zero;

    // private float moveSpeed;
    //private float sprintSpeed;
    //private float walkSpeed;

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

    public bool Pause = false;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause;
        }

        if (!Pause)
        {
            move = Vector3.zero;

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
                //moveSpeed = orgMoveSpeed * 2;
                stateValue = 1 * Input.GetAxis("Vertical");
            }
            else
            {
                //moveSpeed = orgMoveSpeed;
                stateValue = 0.4f * Input.GetAxis("Vertical");
            }
            animator.SetFloat("MoveSpeed", stateValue);



            float moveSpeed = (sprint) ? orgMoveSpeed : orgMoveSpeed*2;
            //movement

            float velX = 0; float velY = 0;
            if (Input.GetKey(KeyCode.W) && velY <= moveSpeed)
            {
                velY += moveSpeed / 3 * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && velY >= -moveSpeed)
            {
                velY -= moveSpeed / 3 * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) && velX <= moveSpeed)
            {
                velX += moveSpeed / 3 * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A) && velX >= -moveSpeed)
            {
                velX -= moveSpeed / 3 * Time.deltaTime;
            }
            velY *= friction * Time.deltaTime;
            velX *= friction * Time.deltaTime;

            move = new Vector3(velX, 0f, velY);
            move.Normalize();



            if (controller.isGrounded)
            {
                gravity = Vector3.zero;
            }
            else
            {
                gravity += Physics.gravity * Time.deltaTime;
            }
            move += gravity;
            
            //turning
            transform.Rotate(0, Input.GetAxis("Horizontal1") * 200f * Time.deltaTime, 0f);

            camRotX -= Input.GetAxis("Vertical1") * 100 * Time.deltaTime;
            //camRotX = Mathf.Clamp(camRotX, -camPitchMax, camPitchMax);
            if (camRotX > camPitchMax)
            {
                camRotX = camPitchMax;
            } else if (camRotX < -camPitchMax)
            {
                camRotX = -camPitchMax;
            }
            Camera.main.transform.forward = transform.forward;
            Camera.main.transform.Rotate(camRotX, 0f, 0f);
            //end turning
            move = transform.TransformDirection(move);
            if (sprint == false) controller.Move(move * orgMoveSpeed * Time.deltaTime);
            else controller.Move(move * orgMoveSpeed * 2 * Time.deltaTime);
        }

    }

}
