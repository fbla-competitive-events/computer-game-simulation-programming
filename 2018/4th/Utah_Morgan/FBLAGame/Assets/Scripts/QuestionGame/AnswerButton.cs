using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The script on the answer button Gameobject
/// </summary>
public class AnswerButton : MonoBehaviour
{
    /// <summary>
    /// The text of the button object
    /// </summary>    
    public Text answerText;

    /// <summary>
    /// What answer this button displays
    /// </summary>
    private AnswerData answerData;
    /// <summary>
    /// The game controller
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// Initializes stuff
    /// </summary>
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    /// <summary>
    /// Sets up the button
    /// </summary>
    /// <param name="data">The answer data for this button</param>
    public void SetUp(AnswerData data)
    {
        answerData = data;
        answerText.text = answerData.answerText;
    }

    /// <summary>
    /// Called when the user selects this button
    /// </summary>
    public void HandleClick()
    {
        gameController.AnswerButtonClicked(answerData.isCorrect);
    }
}