using UnityEngine;
using System.Collections;

/// <summary>
/// Container for the data of the round of questions
/// </summary>
[System.Serializable]
public class RoundData
{
    /// <summary>
    /// The name of this round
    /// </summary>
    public string Name;

    /// <summary>
    /// The time limit of the round
    /// </summary>
    public int timeLimitInSeconds;

    /// <summary>
    /// The points added for every correct answer
    /// </summary>
    public int pointsAddedForCorrectAnswer;

    /// <summary>
    /// The questions for the round
    /// </summary>
    public QuestionData[] questions;

}