using UnityEngine;
using System.Collections;
using System;

public class IdleState : IState
{
    /// <summary>
    /// The classmate that is in this state
    /// </summary>
    private Classmate parent;

    /// <summary>
    /// The enum version of this state
    /// </summary>
    public Classmate.State State
    {
        get
        {
            return Classmate.State.Idle;
        }
    }

    /// <summary>
    /// Initializes values. Sets the rigidbody to static and the move vector to zero
    /// </summary>
    /// <param name="parent">The parent</param>
    public void Enter(Classmate parent)
    {
        this.parent = parent;
        parent.MoveVector = Vector3.zero;
        //While we are idleing, we don't want to move at all.
        parent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    /// <summary>
    /// Sets the rigid body back to dynamic
    /// </summary>
    public void Exit()
    {
        parent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    /// <summary>
    /// Does nothing, but must be implemented because of the interface
    /// </summary>
    public void Update()
    {
        
    }
}
