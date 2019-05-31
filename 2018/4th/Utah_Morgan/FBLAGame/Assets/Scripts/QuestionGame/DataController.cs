using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;    
using UnityEngine.UI;

/// <summary>
/// Controls the data for the round
/// </summary>
public class DataController : MonoBehaviour
{
    /// <summary>
    /// The data for the round
    /// </summary>
    private RoundData[] allRoundData;    

    /// <summary>
    /// The text on the title screen
    /// </summary>
    [SerializeField]
    Text titleText;

    /// <summary>
    /// The json file for the game
    /// </summary>
    private string gameDataFileName = "data.json";        

    /// <summary>
    /// Initializes the data for the game
    /// </summary>
    /// <param name="dataName">The file containing the data</param>
    public void Initialize(string dataName)
    {        
        LoadGameData(dataName);        
        
        GameObject.Find("QuestionCanvas").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("QuestionCanvas").transform.GetChild(1).gameObject.SetActive(false);        
    }

    /// <summary>
    /// Gets the current round data
    /// </summary>
    /// <returns>The current round data</returns>
    public RoundData GetCurrentRoundData()
    {
        // If we wanted to return different rounds, we could do that here
        // We could store an int representing the current round index in PlayerProgress

        return allRoundData[Random.Range(0, allRoundData.Length)];
    }    
   
    /// <summary>
    /// Gets the data given a json file
    /// </summary>
    /// <param name="dataName">The json file containing the data</param>
    private void LoadGameData(string dataName)
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, dataName);
        GameData loadedData;        
        if (Serialize.JsonSerializer.LoadData<GameData>(filePath, out loadedData))
        {
            allRoundData = loadedData.allRoundData;
            titleText.text = loadedData.Name;
        }                
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }      
}