#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditor : EditorWindow
{

    /// <summary>
    /// The target being serialized
    /// </summary>
    public Dialogue myDialogue;

    /// <summary>
    /// The name of the file being saved
    /// </summary>
    private string fileName;

    /// <summary>
    /// The position of the scroll view
    /// </summary>
    private Vector2 scrollPos;
    
    /// <summary>
    /// The height of a single line
    /// </summary>
    const float k_SingleLineHeight = 16f;

    /// <summary>
    /// The width of a label
    /// </summary>
    const float k_LabelWidth = 53f;

    /// <summary>
    /// Pops up the dialogue editor
    /// </summary>
    [MenuItem("Window/Dialogue Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(DialogueEditor)).Show();
    }

    /// <summary>
    /// A list of the saved responses used for easy access in the editor
    /// </summary>
    public List<ReuseableData> SavedResponses;

    /// <summary>
    /// A list of saved dialogues 
    /// </summary>
    public List<ReuseableData> SavedPossibleDialogues;  
    
    ///<summary>
    ///The dialogue that was copied
    ///</summary>  
    public List<Dialogue> CopyedPossibleDialogues;  
    
    ///<summary>
    ///The response that was copied
    ///</summary>   
    public List<Response> CopyedResponses;

    /// <summary>
    /// A list of the loaded dialogues
    /// </summary>
    public List<ReuseableData> LoadedDialogues;    

    /// <summary>
    /// Called when ever the dialogue needs to be drawn
    /// </summary>
    void OnGUI() 
    {
        //Initalizes lists
        if (SavedPossibleDialogues == null)
        {
            SavedPossibleDialogues = new List<ReuseableData>();            
        }
        if (SavedResponses == null)
        {
            SavedResponses = new List<ReuseableData>();
        }
        if (LoadedDialogues == null)
        {
            LoadedDialogues = new List<ReuseableData>();
        }

        //Gets the scroll position
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        //If the dialogue isn't null, serialize it
        if (myDialogue != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("myDialogue");            
            
            EditorGUILayout.PropertyField(serializedProperty, true);
            
            serializedObject.ApplyModifiedProperties();               

            //If the user clicks the save button, save the data
            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        //Creates a new dialogue
        if (GUILayout.Button("New Data"))
        {
            NewData();
        }

        //Loads a dialogue
        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }        
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Creates a new dialogue
    /// </summary>
    private void NewData()
    {
        myDialogue = new Dialogue();          
    }

    /// <summary>
    /// Pulls up a save window to save the dialogue
    /// </summary>
    private void SaveGameData()
    {        
        string filePath = EditorUtility.SaveFilePanelInProject("Save Dialogue", fileName, "json", "Save Dialogue", "Assets/StreamingAssets");
        Serialize.JsonSerializer.SaveData(myDialogue, filePath);      
    }

    /// <summary>
    /// Pulls up an open file window to open an already saved dialogue
    /// </summary>
    private void LoadGameData()
    {
        string path = EditorUtility.OpenFilePanel("Open Dialouge", "Assets/StreamingAssets", "json");
        if (path == "") return;        
        fileName = path.Split('/')[path.Split('/').Length - 1];
        LoadedDialogues.Add(new ReuseableData(fileName, "This"));
        Serialize.JsonSerializer.LoadData(path, out myDialogue);        
    }
   
}
#endif    
