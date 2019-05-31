using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to start a new question game
/// </summary>
public static class QuestionTrigger
{
    /// <summary>
    /// Delegate called when the quesiton game ends
    /// </summary>
    /// <param name="score">The score of the quesition game</param>
    public delegate void GameEnd(int score);
    
    /// <summary>
    /// The scene name of the question game
    /// </summary>
    private static string sceneName = "EventQuestions";

    /// <summary>
    /// The json file of the current question game
    /// </summary>
    private static string dataName;

    /// <summary>
    /// Called when the question game ends
    /// </summary>
    private static GameEnd gameEnd;

    /// <summary>
    /// Starts a new question game
    /// </summary>
    /// <param name="dataName">The json file containing the data for the game</param>
    /// <param name="method">The method that is called when the game ends</param>
    public static void StartQuestionGame(string dataName, GameEnd method)
    {
        QuestionTrigger.dataName = dataName;
        gameEnd = method;
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        async.completed += DoneLoading;
    }        

    /// <summary>
    /// When the question game scene is done loading.
    /// Starts the game.
    /// </summary>
    /// <param name="async"></param>
    private static void DoneLoading(AsyncOperation async)
    {
        GameObject[] objects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
        GameController controller = null;
        foreach (GameObject o in objects)
        {
            if (o.name == "GameController")
            {
                controller = o.GetComponent<GameController>();
            }
        }
        controller.Initialize(dataName, gameEnd);
        GameObject.FindObjectOfType<DialogueManager>().gameObject.SetActive(true);
        dataName = null;
        gameEnd = null;
    }
}
