
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fuller : Movement
{    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();        
        Name = "Fuller";
        MoveVector = Vector3.zero;
    }
    
    /// <summary>
    /// Called when the player wants to talk to Fuller
    /// </summary>
    public override void Interact()
    {        
        DialogueTrigger.TriggerDialogue("FullerDialogue.json");
    }    
}
