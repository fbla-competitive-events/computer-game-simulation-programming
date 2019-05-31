#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddToSavedResponsesPopUp : EditorWindow
{

    /// <summary>
    /// The name used to access the saved response
    /// </summary>
    string IDName;

    /// <summary>
    /// How we are going to access the response wanting to be saved
    /// </summary>
    Dialogue target;

    /// <summary>
    /// A reference to the Dialogue Editor where the resposne will be saved
    /// </summary>
    DialogueEditor editor;
    private void OnGUI()
    {
        //Where the user enters the ID Name
        IDName = EditorGUILayout.TextField("ID Name", IDName);
                
        //If the use clicks done, save the reponse to the Dialogue Editor
        if (GUILayout.Button("Done"))
        {
            Done();
        }
        //If the user clicks cancel, close the window
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
    }

    /// <summary>
    /// Where the response is saved to the Dialogue Editor
    /// </summary>
    void Done()
    {
        editor.SavedResponses.Add(new ReuseableData(target.PossibleResponses, IDName));
        Close();
    }

    /// <summary>
    /// Called to initialize data in the popup window
    /// </summary>
    /// <param name="data">A reference to the data needed to save</param>
    /// <param name="editor">A reference to the Dialogue Editor where the data is going to be saved</param>
    public void SetData(ref Dialogue data, ref DialogueEditor editor)
    {
        this.target = data;
        this.editor = editor;
    }
}
#endif

