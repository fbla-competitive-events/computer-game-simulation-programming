using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class Movement : MonoBehaviour, IInteract 
{
    /// <summary>
    /// What direction the character is moving in
    /// </summary>
    public Vector3 MoveVector;

    /// <summary>
    /// If the character can walk or not
    /// </summary>
    public virtual bool CanWalk { get; set; }

    /// <summary>
    /// The name of the character
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// If the character has already been asked about fundraising cards
    /// </summary>
    public bool AnsweredPlayerCards { get; set; }

    /// <summary>
    /// If the character has already been asked about being recruited for membership
    /// </summary>
    public bool AnsweredPlayerMembership { get; set; }         

    /// <summary>
    /// What is displayed when the player is close enough to this character
    /// </summary>
    public string Description
    {
        get
        {
            return "speak to " + Name;
        }
    }

    /// <summary>
    /// How to deal with walking and other animations
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Tells if the character is moving or not
    /// </summary>
    private bool isMoving
    {
        get
        {
            return (MoveVector.magnitude != 0);
        }
    }

    /// <summary>
    /// How fase the character walks
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
    [SerializeField]
    private float speed;

    /// <summary>
    /// A list of all of the triggers the character is currently being collided with
    /// </summary>
    public List<Collider2D> Triggers
    {
        get
        {
            if (triggers == null)
            {
                triggers = new List<Collider2D>();
            }
            return triggers;
        }

        set
        {
            triggers = value;
        }
    }
    private List<Collider2D> triggers;

    /// <summary>
    /// A list of all of the colliders the character is currently being collided with
    /// </summary>
    public List<Collision2D> Colliders
    {
        get
        {
            return colliders;
        }

        set
        {
            colliders = value;
        }
    }
    private List<Collision2D> colliders;

    /// <summary>
    /// Does all of the initialization
    /// </summary>
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        Triggers = new List<Collider2D>();
        Colliders = new List<Collision2D>();        
        AnsweredPlayerCards = false;
        AnsweredPlayerMembership = false;       
    }

    /// <summary>
    /// Called regularly. Does not depend on the speed of the computer
    /// </summary>
    protected void FixedUpdate()
    {
        //If we cannot walk, we should not be moving
        if (!CanWalk)
            MoveVector = Vector3.zero;  
              
        //Updates the postion by adding the moveVector times the scalar "speed"
        transform.position += MoveVector.normalized * Speed;  
              
        //Update the animations
        handleLayers();
    }

    /// <summary>
    /// Handles all of the animation layers
    /// </summary>
    void handleLayers()
    {        
        //If it is moving, 
        if (isMoving)
        {
            activateLayer("WalkingLayer");

            //If the moveVector is greater than  zero then set the float to 1
            anim.SetFloat("x", MoveVector.x);
            anim.SetFloat("y", MoveVector.y);
        }
        else
        {
            activateLayer("IdleLayer");
        }
    }

    /// <summary>
    /// Activates a layer as the active layer
    /// </summary>
    /// <param name="layerName">The name of the layer to activate</param>
    void activateLayer(string layerName)
    {
        //Disables all of the layers
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        //Set active the given layer
        anim.SetLayerWeight(anim.GetLayerIndex(layerName), 1);
    }    

    /// <summary>
    /// Updates the triggers list
    /// </summary>
    /// <param name="coll"></param>
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {        
        if (!Triggers.Contains(coll))
            Triggers.Add(coll);
    }

    /// <summary>
    /// Updates the colliders list
    /// </summary>
    /// <param name="col"></param>
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (!Colliders.Contains(col))
            Colliders.Add(col);
    }

    /// <summary>
    /// Removes the collision from the list
    /// </summary>
    /// <param name="col"></param>
    protected virtual void OnCollisionExit2D(Collision2D col)
    {        
        int index = Colliders.FindIndex(j => j.gameObject == col.gameObject);
        if (index >= 0)
            Colliders.RemoveAt(index);
    }

    /// <summary>
    /// Removes the trigger from the list
    /// </summary>
    /// <param name="col"></param>
    protected virtual void OnTriggerExit2D(Collider2D col)
    {        
        Triggers.Remove(col);
    }

    /// <summary>
    /// Triggers a dialogue. Any class that needs to trigger a different dialogue, then override this method
    /// </summary>
    public virtual void Interact()
    {
        CanWalk = false;                
        DialogueTrigger.TriggerDialogue("ClassmateDialogue.json", this);
    }
}
