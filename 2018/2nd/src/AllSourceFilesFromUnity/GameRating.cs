using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameRating : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Awake()
	{
		GameControl.control.allGames [GameControl.control.gameCount].reCalculateRating ();
		text.text = string.Format("{0:N1}",GameControl.control.allGames [GameControl.control.gameCount].rating)+"/10";
	}
}
