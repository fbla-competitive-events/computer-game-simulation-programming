using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

[Serializable]
public class DialogueText
{
    /// <summary>
    /// The message this dialogue text displays
    /// </summary>
    public string Message;

    /// <summary>
    /// If there will be a method invoked after this dialogue text
    /// </summary>
    public bool HasAfterMessage = false;

    /// <summary>
    /// The method invoked after this dialogue text is displayed
    /// </summary>
    public UnityEvent AfterMessage;

    /// <summary>
    /// If the dialogue will end after this dialogue text
    /// </summary>
    public bool EndDialogue = false;

#if UNITY_EDITOR
    public float elementHeight;
#endif

    public DialogueText(string message)
    {
        Message = message;
    }
}

