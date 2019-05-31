using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.Tilemaps;
using System;

public class BlockedState : IState
{
    /// <summary>
    /// A reference to the parent in this state
    /// </summary>
    Classmate parent;
     
    /// <summary>
    /// The new direction that the parent should travel
    /// </summary>                  
    Vector3 direction;  
      
    /// <summary>
    /// Used to tell when the parent has been in this state for long enough
    /// </summary>
    float time;

    /// <summary>
    /// The state the parent was in before it went into a blocked state
    /// </summary>
    Classmate.State previousState;

    /// <summary>
    /// The enum version of the state
    /// </summary>
    public Classmate.State State
    {
        get
        {
            return Classmate.State.Blocked;
        }
    }

    /// <summary>
    /// The contructer used to initialize values
    /// </summary>
    /// <param name="direction">The new direction the parent should travel in</param>
    /// <param name="previousState">The previous state that the parent needs to go back to when exiting this state</param>
    public BlockedState(Vector3 direction, Classmate.State previousState)
    {
        this.direction = direction;
        this.previousState = previousState;  
    }

    /// <summary>
    /// Called to initialize the parent
    /// </summary>
    /// <param name="parent">Set's the parent of who is in this state</param>
    public void Enter(Classmate parent)
    {
        this.parent = parent;

        //Sets the direction that the parent needs to travel in
        parent.MoveVector = direction;        
    }

    /// <summary>
    /// Called when exiting this state.
    /// Even though it is not used, it must be implemented by the IState interface
    /// </summary>
    public void Exit()
    {
        
    }    

    /// <summary>
    /// Tells when there was been one second since the parent has been in this state and reverts back to the previous state
    /// </summary>
    public void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            parent.ChangeState(previousState);
        }        
    }    
}
