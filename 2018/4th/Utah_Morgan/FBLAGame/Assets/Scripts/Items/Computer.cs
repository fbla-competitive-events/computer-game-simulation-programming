using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteract
{    

    /// <summary>
    /// The Description shown when the player gets close enough. "Press ["TASK"] to..."
    /// </summary>
    public string Description
    {
        get
        {
            return "turn on computer";
        }
    }

    /// <summary>
    /// Called when the play wants to pull up a computer. Just calls the computer's dialogue
    /// </summary>
    public void Interact()
    {
        DialogueTrigger.TriggerDialogue("ComputerDialogue.json");        
    }
}
