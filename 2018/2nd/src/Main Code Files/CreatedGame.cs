using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The following class manages the values of a created game. 
 It uses them to calculate the game rating at instantiation and when called on*/
[System.Serializable]
public class CreatedGame {
	/*
	Name is simply the user inputted name of the game
	Bugs is the count of bugs in  a game. This starts high, but can
	be lowered by player. wordCount is a list containing every distinct word the user typed and how 
	many times they did. This list is included as it helps calculate rating and may help future expansions of the game.
	score is the score awarded during the programming game.
	*/
	public string name;
	public int bugs;
	public double rating;
	public List<wordCount> wordCount;
	public int score;
	// Use this for initialization
	public CreatedGame(string name, int bugs,List<wordCount> wordC, int score)
	{
		this.name=name;
		this.bugs = bugs;
		this.bugs = Random.Range(40, 55) ;
		wordCount = wordC;
		this.score = score;
/*A perfect game rating is 10, but a score can exceed that to allow for more competitiveness on the leader board.
	Rating depends on how many different words were typed, programming score, and how many bugs. These
	multiplier values were determined through calculations ensuring for a reasonably challenging perfect score.
	using all words (10) total results in 5 points, reducing bugs to 0 results in  2.5 points, 50 bugs means -2.5. and a score of 3000 = 5 points.
	Added together, an ideal perfect score 10/10 is 0 bugs, 3000 score, all words. */ 
		rating = (.5*wordC.Count)-(this.bugs*.05)+(score*.00167);
	}
	public CreatedGame(string name, int score)
	{
		this.name=name;

		this.score = score;
	}
/*A perfect game rating is 10, but a score can exceed that to allow for more competitiveness on the leader board.
	Rating depends on how many different words were typed, programming score, and how many bugs. These
	multiplier values were determined through calculations ensuring for a reasonably challenging perfect score.
	using all words (10) total results in 5 points, reducing bugs to 0 results in  2.5 points, 50 bugs means -2.5. and a score of 3000 = 5 points.
	Added together, an ideal perfect score 10/10 is 0 bugs, 3000 score, all words. */ 
	public void reCalculateRating()
	{
		rating = (.5*wordCount.Count)-(this.bugs*.05)+(score*.00167);

	}

}
