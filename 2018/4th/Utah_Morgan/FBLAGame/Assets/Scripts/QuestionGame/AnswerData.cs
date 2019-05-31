using UnityEngine;
using System.Collections;

/// <summary>
/// Data container for a specific answer
/// </summary>
[System.Serializable]
public class AnswerData
{
    /// <summary>
    /// The answer
    /// </summary>
    public string answerText;

    /// <summary>
    /// If this answer is the correct answer
    /// </summary>
    public bool isCorrect;
}