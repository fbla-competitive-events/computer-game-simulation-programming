#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddToSavedDialoguesPopUp : EditorWindow
{

    /// <summary>
    /// The name used to reference the saved dialogue
    /// </summary>
    string IDName;   
    
    /// <summary>
    /// Used to access the possible dialogues
    /// </summary>
    Response target;

    /// <summary>
    /// A reference to the Dialogue Editor
    /// </summary>
    DialogueEditor editor;

    /// <summary>
    /// All of the draw functions go here
    /// </summary>
    private void OnGUI()
    {
        //Where the user enters the ID Name
        IDName = EditorGUILayout.TextField("ID Name", IDName);      
        
        //If the user clicks done, call the done function   
        if (GUILayout.Button("Done"))
        {
            Done();
        }
        //If the user clicks cancel, close the popup
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
    }

    /// <summary>
    /// Saves the dialogue to the Didalogue Editor for access
    /// </summary>
    void Done()
    {
        editor.SavedPossibleDialogues.Add(new ReuseableData(target.PossibleDialogues, IDName));
        Close();
    }

    /// <summary>
    /// Called to initialize data in the popup window
    /// </summary>
    /// <param name="data">A reference to the data needed to save</param>
    /// <param name="editor">A reference to the Dialogue Editor where the data is going to be saved</param>
    public void SetData(ref Response data, ref DialogueEditor editor)
    {
        this.target = data;
        this.editor = editor;
    }
}
#endif

