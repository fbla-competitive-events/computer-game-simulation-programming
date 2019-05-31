using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is the input module for typing game.
// It takes in a wordmanager and passes letter values to it
public class WordInput : MonoBehaviour {

	public WordManager wordManager;

	// Update is called once per frame
	void Update () {
		foreach (char letter in Input.inputString)
		{
			wordManager.TypeLetter(letter);
		}
	}

}
