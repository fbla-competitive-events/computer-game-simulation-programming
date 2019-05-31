#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

/// <summary>
/// The editor for the questions
/// </summary>
public class GameDataEditor : EditorWindow
{
    /// <summary>
    /// The current data being displayed
    /// </summary>
    public GameData gameData;

    /// <summary>
    /// The name of the file of the json file
    /// </summary>
    private string fileName;

    /// <summary>
    /// Opens the editor window
    /// </summary>
    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    /// <summary>
    /// Called evertime the user does something to the GUI
    /// </summary>
    void OnGUI()
    {        
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }
        if (GUILayout.Button("New Data"))
        {
            NewData();
        }
        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    /// <summary>
    /// Creates a new game data
    /// </summary>
    private void NewData()
    {
        gameData = new GameData();
    }

    /// <summary>
    /// Loads data from the file name
    /// </summary>
    private void LoadGameData()
    {
        string path = EditorUtility.OpenFilePanel("Open Dialouge", "Assets/StreamingAssets", "json");
        if (path == "") return;
        fileName = path.Split('/')[path.Split('/').Length - 1];        
        Serialize.JsonSerializer.LoadData(path, out gameData);        
    }

    /// <summary>
    /// Saves the current data to the file name
    /// </summary>
    private void SaveGameData()
    {            
        string filePath = EditorUtility.SaveFilePanelInProject("Save Questions", fileName, "json", "Save Questions", "Assets/StreamingAssets");
        Serialize.JsonSerializer.SaveData(gameData, filePath);        
    }
}
#endif