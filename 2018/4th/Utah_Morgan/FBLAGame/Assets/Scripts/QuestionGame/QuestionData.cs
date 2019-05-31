using UnityEngine;
using System.Collections;

/// <summary>
/// Data containing the questions for the round
/// </summary>
[System.Serializable]
public class QuestionData
{
    /// <summary>
    /// The question
    /// </summary>
    public string questionText;

    /// <summary>
    /// The answers to the question
    /// </summary>
    public AnswerData[] answers;
}