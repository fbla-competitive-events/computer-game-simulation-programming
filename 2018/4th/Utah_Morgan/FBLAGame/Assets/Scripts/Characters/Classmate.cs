using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Tilemaps;

public class Classmate : Movement
{    
    /// <summary>
    /// Easy access for telling what state the classmate is in
    /// </summary>
    public enum State { Class, Idle, Blocked }                                                
    
    /// <summary>
    /// An easy way to set the starting state of the classmate.
    /// </summary>
    [SerializeField]
    State state;       

    /// <summary>
    /// Determines if the classmate can walk or not
    /// </summary>
    public override bool CanWalk
    {
        get
        {
            return base.CanWalk;
        }

        set
        {
            //If we are changing the value to true, go to the state is was at before it was set to false
            if (value && !CanWalk)
            {
                ChangeState(state);
            }
            //If we are changing the value to false, make the classmate go idle
            else if (!value && CanWalk)
            {
                ChangeState(new IdleState());
            }
            base.CanWalk = value;
        }
    }

    /// <summary>
    /// If the classmate has reached its spawner and is not in the classmate pool
    /// </summary>
    public bool Destroyed
    {
        get
        {
            return destroyed;
        }
        set
        {
            destroyed = value;
            if (!value)
            {
                Start();
            }
        }
    }
    private bool destroyed;

    /// <summary>
    /// The destination of where the classmate needs to go
    /// </summary>
    public GameObject Spawner { get; set; }

    /// <summary>
    /// The state that the classmate is currently in
    /// </summary>
    private IState currentState;

    /// <summary>
    /// Used for Initialization
    /// </summary>
    protected override void Start()
    {
        base.Start();
        //Sets the classmate to initially not move        
        MoveVector = Vector3.zero;

        //The classmate can walk
        CanWalk = true;     
        
        //A array for possible names that the classmate could have. Only used in this scope     
        string[] names = new string[] { "Bob", "Henry", "Henrick", "Gernaldo", "Garth Gooch" };
        //Sets the name to be a random one from the above array
        Name = names[UnityEngine.Random.Range(0, names.Length - 1)];    
        
        //Assign the state the classmate will be in based on if "state" has a value or not    
        ChangeState(((int)state == 0) ? State.Class : state);

        destroyed = false;
    }

    /// <summary>
    /// Called to update the state of the classmate
    /// </summary>
    void Update()
    {
        //If the classmate is just chilling in the object pool, leave
        if (Destroyed)
        {
            return;
        }
            
        //The current state determines how the classmate acts
        currentState.Update();        

    }       
    

    /// <summary>
    /// Is called whenever the classmate collides with something
    /// </summary>
    /// <param name="col">What it collides with</param>
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        //The only change we want to do is change the state to a blocked state. This is only if the classmate is walking out and about, not in an idle state       
        if (CanWalk)
        {
            //Changes the direction that the classmate is going
            ChangeState(new BlockedState(Quaternion.AngleAxis(90, Vector3.back) * MoveVector, state));
        }            
    }

    /// <summary>
    /// Called whenever the classmate needs to change states
    /// </summary>
    /// <param name="newState">The new state to change to</param>
    public void ChangeState(IState newState)
    {   
        //Exit the old state             
        if (currentState != null)
        {            
            currentState.Exit();            
        }

        //Enter into the new state
        currentState = newState;        
        currentState.Enter(this);        
    }

    /// <summary>
    /// Called whenever the classmate needs to change states
    /// </summary>
    /// <param name="newState">An enum version of the new state to change</param>
    public void ChangeState(State newState)
    {
        //Exits the old state
        if (currentState != null)
        {
            currentState.Exit();
        }

        //Figures out what state to change to based on the enum
        switch (newState)
        {
            case State.Class: currentState = new ClassState(); break;            
            case State.Idle: currentState = new IdleState(); break;
            case State.Blocked: currentState = new BlockedState(Vector3.zero, State.Blocked); break;            
            default: throw new Exception("Not a valid state");
        }       

        //Enter into the state
        currentState.Enter(this);
    }    
}


