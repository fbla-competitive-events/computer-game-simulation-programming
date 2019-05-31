using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word {

	public string word;
	private int typeIndex;
	public bool isFaded;
	private WordManager wordManager;
	public WordDisplay display;

	public Word (string _word, WordDisplay _display, WordManager _manager)
	{
		word = _word;
		typeIndex = 0;

		display = _display;
		display.SetWord(word);
	}

	public char GetNextLetter ()
	{
			return word [typeIndex];



	}
	public bool DoesWordExist(){
		if (display.wordDestroyed) {
			return false;
		} else {
			return true;
		}

	}
	public void TypeLetter ()
	{
		typeIndex++;
		display.RemoveLetter();
	}

	public bool WordTyped ()
	{
		bool wordTyped = (typeIndex >= word.Length);
		if (wordTyped)
		{
			display.RemoveWord();
			display.CreateObject ();
		}
		return wordTyped;
	}

}
