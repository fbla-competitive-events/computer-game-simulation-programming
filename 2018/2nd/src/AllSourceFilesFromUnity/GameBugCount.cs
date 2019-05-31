using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameBugCount : MonoBehaviour {

	public TextMeshProUGUI text;
	// Use this for initialization
	void Awake()
	{
		GameControl.control.allGames [GameControl.control.gameCount].reCalculateRating ();
		text.text = GameControl.control.allGames [GameControl.control.gameCount].bugs.ToString ();
	}
}