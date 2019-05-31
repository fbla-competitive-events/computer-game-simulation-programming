using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for the game
/// </summary>
[System.Serializable]
public class GameData
{
    /// <summary>
    /// The name of the gameSS
    /// </summary>
    public string Name;

    /// <summary>
    /// The data for all of the rounds of the game
    /// </summary>
    public RoundData[] allRoundData;
}
