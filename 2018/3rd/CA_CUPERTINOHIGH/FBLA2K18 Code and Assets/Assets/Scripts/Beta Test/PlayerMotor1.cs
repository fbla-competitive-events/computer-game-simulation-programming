
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMotor1 : MonoBehaviour {
    public bool CanPlayerMove = true;
    protected CharacterController controller;

   // public Animation run;

    protected Vector3 gravity = Vector3.zero;
    protected Vector3 move = Vector3.zero;
    
   // private float moveSpeed;
    private float sprintSpeed = 20;
    private float walkSpeed = 10;

    public float orgMoveSpeed = 10f;
    public float turnSpeed = 300f;
    public float jumpSpeed = 3;
    public float friction = 0.9f;
    protected bool jump;
    protected float camRotX = 0f;
    public float camPitchMax = 35f;

    private bool sprint = false;

    public Slider walkSlider;
    public Slider sprintSlider;
    public Slider hTurnSlider;
    public Slider vTurnSlider;


    private Animator animator;
    private float stateValue = 0f;

    public float HorizontalTurnValue = 100f;
    public float VerticalTurnValue = 200f;
    public float WalkSpeedValue = 10f;
    public float SprintSpeedValue = 20f;
    /*
     * 0 - idle
     * 0.1-0.4 walking
     * > 0.5 running
     * -0.1- -0.4 walking (backwards)
     * < -0.5 running(backwards)
     * 
     * */

    private void Awake()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
        if (g.Length > 1) {
            GameObject.Destroy(g[1]);
        }
        
    }

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Application.Quit();
        }*/

        if (!BetaGameOptions.pause && CanPlayerMove)
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


            walkSpeed = WalkSpeedValue;
            sprintSpeed = SprintSpeedValue;

            float moveSpeed = (sprint) ? sprintSpeed : walkSpeed;
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
            transform.Rotate(0, Input.GetAxis("Horizontal1") * HorizontalTurnValue * Time.deltaTime, 0f);

            camRotX -= Input.GetAxis("Vertical1") * VerticalTurnValue * Time.deltaTime;
            //camRotX = Mathf.Clamp(camRotX, -camPitchMax, camPitchMax);

            //smoother vertical turning
            if (camRotX > camPitchMax)
            {
                camRotX = camPitchMax;
            }
            else if (camRotX < -camPitchMax)
            {
                camRotX = -camPitchMax;
            }

            Camera.main.transform.forward = transform.forward;
            Camera.main.transform.Rotate(camRotX, 0f, 0f);
            //end turning
            move = transform.TransformDirection(move);
            if (sprint == false) controller.Move(move * walkSpeed * Time.deltaTime);
            else controller.Move(move * sprintSpeed * Time.deltaTime);
        }
      
    }

    
}
