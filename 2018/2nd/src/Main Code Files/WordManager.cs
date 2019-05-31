using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
The following class controls majority of the logic for the typing 
functionality of the programming game. It uses a wordGenerator and
Spawner for game flow.

*/
public class WordManager : MonoBehaviour {

	public List<Word> words;
	public WordSpawner wordSpawner;
	public WordGenerator wordGenerator;
	public List<wordCount> wordCounters;
	private bool hasActiveWord;
	private Word activeWord;
	private int space = 0;
	//On start, the variable wordCounters, which counts word usage to submit to gameStats is set equal to the list of words used
	//By the words generator.
	public void Awake()
	{
		wordCounters=wordGenerator.wordCList();
	}
	//Add word is used by the spawner class and simple pulls a random word from the word generator list
	public void AddWord ()
	{
		Word word = new Word(wordGenerator.GetRandomWord(), wordSpawner.SpawnWord(space), this);
		space += Screen.width / 2;
//That word object to also added to the word list.
		words.Add(word);
	}
/*
This is one of the core functions of the typing game. Active Word is a variable indicated what word the user 
has chose to being typing. This is determined if there is no active word and a character is pressed that equals the first char in a word 
that is in word list. Then hasActiveWord is set to true and typing is locked to the target word.
*/
	public void TypeLetter (char letter)
	{ 
		if (hasActiveWord && !activeWord.DoesWordExist ()) {
			words.Remove(activeWord);

			hasActiveWord = false;
		}
		if (hasActiveWord)
		{
			
			if (activeWord.GetNextLetter () == letter) {
				activeWord.TypeLetter ();
			} else if (activeWord.GetNextLetter () != letter) {
				/*If the next letter typed is incorrect, call the remove life function, fade the word and remove it from the list
					Also set active word to false*/
				hasActiveWord = false;
				lifeManager.control.removeLife ();

				activeWord.display.fade();
				words.Remove (activeWord);
			}
		} else
		{
			foreach(Word word in words)
			{
				if (word.DoesWordExist() && word.display.wordInView && word.GetNextLetter() == letter)
				{
					/*If no active word and letter is typed for first letter of an existing word that is within game view
					 set that word as active and type the first letter. Break from the loop*/
					activeWord = word;
					hasActiveWord = true;
					word.TypeLetter();
					break;
				}
			}
		}

		if (hasActiveWord && activeWord.WordTyped())
		{
			/*If word is compeltely typed, up its count in word counter, remove is as active word, and increase score by length of the word.*/
			hasActiveWord = false;
			foreach (wordCount count in wordCounters) {
				if (count.name.Equals(activeWord.word)) {
					count.addCount();
				}
			}
			gameStats.gameScore+=(10*activeWord.word.Length);

			words.Remove(activeWord);
		}
	}

}
