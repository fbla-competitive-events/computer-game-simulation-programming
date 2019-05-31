using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

/// <summary>
/// Controls the question game
/// </summary>
public class GameController : MonoBehaviour
{
    public ObjectPool answerButtonObjectPool;
    public Text questionText;
    public Text scoreDisplay;
    public Text timeRemainingDisplay;    
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;     

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive = false;
    private float timeRemaining;
    private int playerScore;
    private int questionIndex;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    private QuestionTrigger.GameEnd gameEnd;
    private GameObject canvas;

    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private GameObject questionScreen;

    /// <summary>
    /// Initializes the game
    /// </summary>
    /// <param name="data">The json file of the round data</param>
    /// <param name="gameEnd">Called when the game is finished</param>
    public void Initialize(string data, QuestionTrigger.GameEnd gameEnd)
    {        
        this.gameEnd = gameEnd;
        FindObjectOfType<DataController>().Initialize(data);
        canvas = GameObject.Find("QuestionCanvas");
        UIManager.Instance.OpenClose(mainScreen.GetComponent<UIBox>());
    }

    /// <summary>
    /// Starts the questions
    /// </summary>
    public void StartRound()
    {
        canvas.transform.GetChild(0).gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(true);
        ResetValues();

        //Set the focus to the first question
        UIManager.Instance.UpdateSelected(answerButtonGameObjects[0]);
    }    

    /// <summary>
    /// Called every frame and updates the timer
    /// </summary>
    void Update()
    {
        if (isRoundActive)
        {
            // If the round is active, subtract the time since Update() was last called from timeRemaining
            timeRemaining -= Time.deltaTime;   
            UpdateTimeRemainingDisplay();

            // If timeRemaining is 0 or less, the round ends
            if (timeRemaining <= 0f)                                                     
            {
                EndRound();
            }
        }
    }

    /// <summary>
    /// Display the questions for the round
    /// </summary>
    private void ShowQuestion()
    {
        RemoveAnswerButtons();

        // Get the QuestionData for the current question
        QuestionData questionData = questionPool[questionIndex];
        // Update questionText with the correct text
        questionText.text = questionData.questionText;

        // For every AnswerData in the current QuestionData...
        for (int i = 0; i < questionData.answers.Length; i++)
        {
            // Spawn an AnswerButton from the object pool
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            answerButtonGameObject.transform.localScale = Vector3.one;
            
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer
            answerButton.SetUp(questionData.answers[i]); 
        }

        //Allows for pure, genuine keyboard stroke
        for (int i = 0; i < answerButtonGameObjects.Count; i++)
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;

            //Wraps selection around to the last item
            if (i == 0)
            {
                navigation.selectOnLeft = answerButtonGameObjects[answerButtonGameObjects.Count - 1].GetComponent<Button>();
            }
            else
            {
                navigation.selectOnLeft = answerButtonGameObjects[i - 1].GetComponent<Button>();
            }

            //Wraps selection around to the first item
            if (i == answerButtonGameObjects.Count - 1)
            {
                navigation.selectOnRight = answerButtonGameObjects[0].GetComponent<Button>();

            }
            else
            {
                navigation.selectOnRight = answerButtonGameObjects[i + 1].GetComponent<Button>();
            }

            answerButtonGameObjects[i].GetComponent<Button>().navigation = navigation;
        }
    }

    /// <summary>
    /// Removes all of the answer buttons
    /// </summary>
    private void RemoveAnswerButtons()
    {
        // Return all spawned AnswerButtons to the object pool
        while (answerButtonGameObjects.Count > 0) 
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    
    /// <summary>
    /// Called when an answer button is clicked
    /// </summary>
    /// <param name="isCorrect">Whether the answer clicked is correct or not</param>
    public void AnswerButtonClicked(bool isCorrect)
    {        
        if (isCorrect)
        {
            // If the AnswerButton that was clicked was the correct answer, add points
            playerScore += currentRoundData.pointsAddedForCorrectAnswer; 
            scoreDisplay.text = playerScore.ToString();
        }

        // If there are more questions, show the next question
        if (questionPool.Length > questionIndex + 1)                                  
        {
            questionIndex++;
            ShowQuestion();
        }
        // If there are no more questions, the round ends
        else
        {
            EndRound();
        }
    }

    /// <summary>
    /// Updates the time remaining text
    /// </summary>
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
    }

    /// <summary>
    /// Ends the round
    /// </summary>
    public void EndRound()
    {
        isRoundActive = false;        
        UIManager.Instance.OpenClose(questionDisplay.GetComponent<UIBox>());
        UIManager.Instance.OpenClose(roundEndDisplay.GetComponent<UIBox>());
    }

    /// <summary>
    /// Resets the values for the round
    /// </summary>
    void ResetValues()
    {
        // Store a reference to the DataController so we can request the data we need for this round
        dataController = FindObjectOfType<DataController>();   
                                     
        // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
        currentRoundData = dataController.GetCurrentRoundData();        
        
        // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object        
        questionPool = currentRoundData.questions;        

        // Set the time limit for this round based on the RoundData object
        timeRemaining = currentRoundData.timeLimitInSeconds;                                
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        questionIndex = 0;

        scoreDisplay.text = playerScore.ToString();

        ShowQuestion();
        isRoundActive = true;

    }

    /// <summary>
    /// Called when the user clicks return to menu
    /// Loads the scene that returns back to the game
    /// </summary>
    public void ReturnToMenu()
    {
        AsyncOperation async = SceneManager.UnloadSceneAsync("EventQuestions");
        async.completed += OnCompletion;
    }
    /// <summary>
    /// Called when the scene has loaded and updates the score of the player
    /// </summary>
    /// <param name="async"></param>
    void OnCompletion(AsyncOperation async)
    {
        gameEnd(playerScore + ((Player.Instance.Debug) ? 10000: 0));
    }    
}