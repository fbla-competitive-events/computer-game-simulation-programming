using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class DialogueTrigger
{            
    /// <summary>
    /// When the dialogue closes, it calls the given event
    /// </summary>
    /// <param name="Event">Event called when closed</param>
    public static void AddOnCloseEvent(DialogueManager.Event Event)
    {
        Object.FindObjectOfType<DialogueManager>().OnClose += Event;
    }

    /// <summary>
    /// Called when a certain dialogue is displayed
    /// </summary>
    /// <param name="OnFinish"></param>
    public static void SetOnFinishEvent(DialogueManager.OnFinishSentence OnFinish)
    {
        Object.FindObjectOfType<DialogueManager>().AddOnFinishSentence(OnFinish);
    }

    /// <summary>
    /// Ends the dialogue at a certain point in its dialogue
    /// </summary>
    /// <param name="position"></param>
    public static void SetEndDialoguePosition(int position)
    {
        Object.FindObjectOfType<DialogueManager>().EndDialoguePosition = position;
    }

    public static void SetContinueToResponses(int position)
    {
        DialogueManager.Instance.ContinueToResponses = position;
    }

    /// <summary>
    /// Allows the dialogue to advance to the next string
    /// </summary>
    /// <param name="state"></param>
    public static void SetCanDisplaySentence(bool state)
    {
        Object.FindObjectOfType<DialogueManager>().CanDisplayNext = state;
    }

    /// <summary>
    /// Advances the dialogue to the next string
    /// </summary>
    public static void DisplayNextSentence()
    {        
        Object.FindObjectOfType<DialogueManager>().DisplayNextSentence(true);
    }

    /// <summary>
    /// Starts a dialogue
    /// </summary>
    /// <param name="jsonFileName">The file name where the dialogue is</param>
    /// <param name="Caller">Who is triggering the dialogue</param>
    /// <param name="startingDialoguePosition">The starting position in the array of dialogues</param>
    public static void TriggerDialogue(string jsonFileName, Movement Caller, int startingDialoguePosition = 0)
    {
        Player.Instance.CanWalk = false;
        Dialogue dialogue = LoadDialogueData(jsonFileName);
        Object.FindObjectOfType<DialogueManager>().StartDialogue(dialogue, Caller, startingDialoguePosition);        
    }

    /// <summary>
    /// Starts a dialogue. Called when the caller is not of type movement
    /// </summary>
    /// <param name="jsonFileName">The file name where the dialogue is</param>
    /// <param name="startingDialoguePosition">The starting position in the array of dialogues</param>
    public static void TriggerDialogue(string jsonFileName, int startingDialoguePosition = 0)
    {
        TriggerDialogue(jsonFileName, null, startingDialoguePosition);
    }

    /// <summary>
    /// Ends the dialogue
    /// </summary>
    public static void EndDialogue()
    {
        Object.FindObjectOfType<DialogueManager>().EndDialogue();
    }

    /// <summary>
    /// Gets the dialogue given the file name
    /// </summary>
    /// <param name="jsonFileName">The file name of the dialogue</param>
    /// <returns></returns>
    private static Dialogue LoadDialogueData(string jsonFileName)
    {        
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        Dialogue loadedData = null;
        if (!Serialize.JsonSerializer.LoadData(filePath, out loadedData))
        {
            Debug.LogError("Cannot find json file " + jsonFileName);
        }        
        return loadedData;
    }    
}
