using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{    
    /// <summary>
    /// An array of what the caller of the dialogue system is saying
    /// </summary>
    public List<string> DialogueText = new List<string>();    

    /// <summary>
    /// Who triggered the dialouge. If the caller is a computer, "Caller" is null
    /// </summary>
    public Movement Caller { get; set; }

    /// <summary>
    /// The name of the Caller
    /// </summary>
    public string Name;

    /// <summary>
    /// The responses the player has
    /// </summary>
    public List<Response> Responses
    {
        get
        {            
            //If there needs to be a method to decide what response to display, call that method
            if (thisType.Type == Type.Method)
            {
                return DialogueMethod.InvokeMethod(thisType.ValueOfType, new object[2] { PossibleResponses, this }) as List<Response>;
            }
            return PossibleResponses;
        }        
    }    

    /// <summary>
    /// A list of all of the possible responses the player could have
    /// </summary>
    public List<Response> PossibleResponses = new List<Response>();   

    /// <summary>
    /// The type of dialogue. Custom, Method, Loaded etc.
    /// </summary>
    [SerializeField]
    private ResponsesType thisType;

    /// <summary>
    /// The method when the type of this dialogue is method
    /// </summary>
    public UnityEvent typeMethod;

#if UNITY_EDITOR
    /// <summary>
    /// Used in the Dialogue Editor for easy dialogue editing
    /// </summary>
    public UnityEditorInternal.ReorderableList responseReorderableList;

    /// <summary>
    /// Used in the Dialogue Editor for easy dialogue editing
    /// </summary>
    public UnityEditorInternal.ReorderableList dialogueTextReorderableList;

    /// <summary>
    /// Used in the Dialogue Editor to tell how tall this particular dialogue should be displayed in the Editor
    /// </summary>
    public float ElementListHeight
    {
        get
        {
            //Default height
            float height = 63f;

            ////Height of all of the Dialouge Texts
            //height += 21 * DialogueText.Count;

            if (dialogueTextReorderableList != null)
            {
                height += dialogueTextReorderableList.GetHeight();
            }

            //If there is no possible resonponses to be displayed, just return now
            if (ThisType.Type == Type.Dialogue)
                return height;

            //Height of empty reorderable list;
            height += 63f;
            foreach (Response response in PossibleResponses)
            {
                height += response.ElementListHeight;
            }
            return height;
        }
    }    
#endif



    /// <summary>
    /// Initializes a new dialogue
    /// </summary>
    public Dialogue()
    {                     
    }

    /// <summary>
    /// What type of possible response there is. Custom, Method, Loaded, etc.
    /// </summary>
    public ResponsesType ThisType
    {
        get
        {
            return thisType;
        }

        set
        {
            if (value.Type == Type.Dialogue)
            {
                PossibleResponses = value.PossibleResponses.DataAsResponse;
            }
            thisType = value;
        }
    }

    /// <summary>
    /// The type of possible responses
    /// </summary>
    public enum Type { Custom, Dialogue, Method }
    [Serializable]
    public class ResponsesType
    {
        /// <summary>
        /// The type of possible responses
        /// </summary>
        public Type Type;

        /// <summary>
        /// The value of the type if it is a method
        /// </summary>
        public string ValueOfType;

        /// <summary>
        /// The possible responses
        /// </summary>
        public ReuseableData PossibleResponses;

        /// <summary>
        /// Converts this to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Type == Type.Custom)
            {
                return "Custom";
            }
            string s = Type.ToString() + ": ";
            if (Type == Type.Dialogue && PossibleResponses != null)
            {
                s += PossibleResponses.IdName;
            }
            else if (Type == Type.Method)
            {
                s += ValueOfType;
            }
            return s;
        }

        /// <summary>
        /// Two ResponsesType types equal if their values equal, not the reference
        /// </summary>
        /// <param name="obj">The ResponsesType comparing to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ResponsesType o = obj as ResponsesType;
            return (Type == o.Type && ValueOfType == o.ValueOfType && PossibleResponses == o.PossibleResponses);
        }
    }
}
