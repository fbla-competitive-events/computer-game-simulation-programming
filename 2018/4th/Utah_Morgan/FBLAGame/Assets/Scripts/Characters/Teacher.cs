
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Teacher : Movement
{
    /// <summary>
    /// The name of the teacher
    /// </summary>
    [SerializeField]
    private string name;

    /// <summary>
    /// The name of the dialogue file
    /// </summary>
    [SerializeField]
    private string fileName = "TeacherDialogue.json";

    /// <summary>
    /// Initialization
    /// </summary>    
    protected override void Start ()
    {
        base.Start();
                
        Name = name;
        
        //The teacher is not moving, so set the movement to zero
        MoveVector = Vector3.zero;
    }
	
    /// <summary>
    /// When the player interacts, trigger the dialogue assigned
    /// </summary>
    public override void Interact()
    {                
        DialogueTrigger.TriggerDialogue(fileName, this);
    }    
}
