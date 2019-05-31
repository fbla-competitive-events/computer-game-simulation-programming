using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Officer : Movement
{
    /// <summary>
    /// Singleton instance of the officer
    /// </summary>
    public static Officer Instance
    {
        get
        {
            return FindObjectOfType<Officer>();
        }
    }

    /// <summary>
    /// The name of the officer
    /// </summary>
    [SerializeField]
    string name;

    /// <summary>
    /// The file name of the dialogue
    /// </summary>
    const string FILE = "OfficerDialogue.json";            

    // Use this for initialization
    protected override void Start ()
    {
        Name = name;

        //Starts the tutorial dialogue
        if (GameManager.Instance.Stage == 0)
        {
            GameManager.Instance.SetNextStage();        
            StartCoroutine(StartTutorial());
        }        
        MoveVector = Vector3.zero;
        base.Start();        
    }
	
    /// <summary>
    /// Starts the tutorial
    /// </summary>
    /// <returns></returns>
    IEnumerator StartTutorial()
    {
        yield return new WaitForEndOfFrame();

        

        //Set an event where it waits for the player to walk around
        DialogueTrigger.SetOnFinishEvent(new DialogueManager.OnFinishSentence(mWalkAround, 4));

        //Set an event where it waits for the player to press the task box button
        DialogueTrigger.SetOnFinishEvent(new DialogueManager.OnFinishSentence(mWaitUntilTask, 5));

        DialogueTrigger.SetEndDialoguePosition(8);
        DialogueTrigger.SetContinueToResponses(2);
        DialogueTrigger.TriggerDialogue(FILE);
    }

    /// <summary>
    /// Called when the player interacts with the officer. Jumps straight to the possible responses
    /// </summary>
    public override void Interact()
    {
        //Set an event where it waits for the player to walk around
        DialogueTrigger.SetOnFinishEvent(new DialogueManager.OnFinishSentence(mWalkAround, 4));
        DialogueTrigger.SetOnFinishEvent(new DialogueManager.OnFinishSentence(mWaitUntilTask, 5));
        DialogueTrigger.SetEndDialoguePosition(8);
        DialogueTrigger.TriggerDialogue(FILE, 14);
    }

    /// <summary>
    /// Called for the walk around tutorial
    /// </summary>
    /// <returns></returns>
    public bool mWalkAround()
    {
        StartCoroutine(WalkAround());
        return false;
    }

    /// <summary>
    /// Waits until the task button is pressed
    /// </summary>
    /// <returns></returns>
    public bool mWaitUntilTask()
    {
        //DialogueTrigger.SetCanDisplaySentence(false);
        StartCoroutine(WaitUntil(() => {
            return Input.GetKeyDown(KeybindManager.Instance.Keybinds("TASK"));
        }));
        return false;
    }

    /// <summary>
    /// Waits until the player walks around
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkAround()
    {
        FindObjectOfType<Player>().CanWalk = true;
        yield return new WaitUntil(() => { return Input.GetKeyDown(KeybindManager.Instance.Keybinds("UP"))
            || Input.GetKeyDown(KeybindManager.Instance.Keybinds("RIGHT")) || Input.GetKeyDown(KeybindManager.Instance.Keybinds("LEFT"))
            || Input.GetKeyDown(KeybindManager.Instance.Keybinds("DOWN")); });
        yield return new WaitForSeconds(1f);
        FindObjectOfType<Player>().CanWalk = false;
        DialogueTrigger.DisplayNextSentence();
    }

    /// <summary>
    /// Waits until the given function is true and displays the next sentence
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IEnumerator WaitUntil(Func<bool> predicate)
    {
        yield return new WaitUntil(predicate);                
        DialogueTrigger.DisplayNextSentence();
        Player.Instance.CanWalk = false;
    }
}
