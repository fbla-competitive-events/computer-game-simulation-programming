using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Response
{       
    /// <summary>
    /// The response text
    /// </summary>
    public string Text;
#if UNITY_EDITOR
    /// <summary>
    /// The reorderable list of possible dialogues used by the response drawer
    /// </summary>
    public UnityEditorInternal.ReorderableList reorderableList;

    ///<summary>
    ///Used by the response drawer to determine the height of the seralized property
    ///</summary>    
    public float ElementListHeight
    {
        get
        {
            //Default height
            float height = 42f;
            //If the Type = Dialogue, there is nothing else to calculate because there is no visible reoderable list
            if (ThisType.Type == Type.Dialogue || ThisType.Type == Type.LoadDialogue)// && !ThisType.PossibleDialogues.ShowChildren)
                return height;
            //height of empty reorderable list;
            height += 63f;
            foreach (Dialogue dialogue in PossibleDialogues)
            {
                height += dialogue.ElementListHeight;
            }
            return height;
        }
    }
#endif

    /// <summary>
    /// A list of the possible dialogues that the response will show when clicked
    /// </summary>
    public List<Dialogue> PossibleDialogues;
    
    /// <summary>
    /// The type of response, mainly used in the response drawer
    /// </summary>    
    public PossibleDialoguesType ThisType
    {
        get
        {
            return thisType;
        }
        set
        {
            if (value.Type == Type.Dialogue)
            {
                PossibleDialogues = value.PossibleDialogues.DataAsDialogue;
            }
            if (value.Type == Type.LoadDialogue)
            {
                PossibleDialogues = new List<Dialogue>();
            }
            thisType = value;
        }
    }
    public enum Type { Custom, Method, Dialogue, LoadDialogue }
    [SerializeField]
    private PossibleDialoguesType thisType;
    
    ///<summary>
    ///The name of the caller
    ///</summary>    
    public string Name;
    
    

    /// <summary>
    /// Initialization
    /// </summary>
    public Response()
    {
        PossibleDialogues = new List<Dialogue>();
        ThisType = new PossibleDialoguesType() { Type = Type.Custom };
    }
    
    /// <summary>
    /// Called when the user presses enter on this response
    /// </summary>
    /// <returns>The next dialogue to be displayed</returns>
    public Dialogue Enter()
    {
        if (ThisType.Type == Type.LoadDialogue)
        {
            Dialogue dialogue;
            if (Serialize.JsonSerializer.LoadData<Dialogue>(Application.streamingAssetsPath + "/" + ThisType.PossibleDialogues.DataAsString, out dialogue))
            {
                return dialogue;
            }
            else
            {
                Debug.LogError(string.Format("Cannot find file name {0}", ThisType.PossibleDialogues.DataAsString));
            }

        }                    
        //If there is only one possible dialogue, just return that. There is no need to go through a method to decide which dialogue to choose if there is only one
        if (PossibleDialogues.Count == 1 && ThisType.Type != Type.Method)
        {
            return PossibleDialogues[0];
        }

        if (ThisType.Type == Type.Method)
        {
            //Basically does the method that is assigned to ThisType
            return DialogueMethod.InvokeMethod(ThisType.ValueOfType, new object[1] { PossibleDialogues }) as Dialogue;
        }

        return null;

    }
    
    /// <summary>
    /// Type of possible dialogue. Mainly used in the property drawer
    /// </summary>
    [Serializable]
    public class PossibleDialoguesType
    {
        /// <summary>
        /// The type of response
        /// </summary>
        public Type Type;

        /// <summary>
        /// The value of the type
        /// </summary>
        public string ValueOfType;  
        
        ///<summary>
        ///The possible dialouges
        ///</summary>                      
        public ReuseableData PossibleDialogues;  
        
        ///<summary>
        ///How to display this in the editor
        ///</summary>      
        public override string ToString()
        {
            if (Type == Type.Custom)
            {
                return "Custom";
            }
            string s = Type.ToString() + ": ";
            if (Type == Type.Method)
            {
                s += (string)ValueOfType;
            }
            else if (PossibleDialogues != null)
            {
                s += PossibleDialogues.IdName;
            }            
            return s;
        }

        /// <summary>
        /// Equal if the values are equal, not the reference
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PossibleDialoguesType o = obj as PossibleDialoguesType;
            return (Type == o.Type && ValueOfType == o.ValueOfType && PossibleDialogues.Equals(o.PossibleDialogues));
        }
    }  
}


