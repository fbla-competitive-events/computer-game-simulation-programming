using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameName : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Awake () {
		text.text = GameControl.control.allGames [GameControl.control.gameCount].name;
	}
	
	// Update is called once per frame

}
