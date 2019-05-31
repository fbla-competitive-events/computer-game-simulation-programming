using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AwardDescriptionBoard : MonoBehaviour {
	//Tier1
	public TextMeshProUGUI buyWord;
	public TextMeshProUGUI placingMedal;
	public TextMeshProUGUI raising10k;
	public TextMeshProUGUI submitToLeaderboard;
	//Tier2
	public TextMeshProUGUI placeFirst;
	public TextMeshProUGUI buyAllWords;
	public TextMeshProUGUI raise500k;
	//FBLA ULTIMATE MEDAL
	public TextMeshProUGUI FblaExcellenceAward;
	// Use this for initialization
	void Start () {
		if (GameControl.control.buyWord) {
			buyWord.color = Color.green;
			buyWord.text = "Unlocked!";
		}
		if (GameControl.control.placingMedal) {
			placingMedal.color = Color.green;
			placingMedal.text = "Unlocked!";
		}	
		if (GameControl.control.raising10k) {
			raising10k.color = Color.green;
			raising10k.text = "Unlocked!";
		}	
		if (GameControl.control.submitToLeaderboard) {
			submitToLeaderboard.color = Color.green;
			submitToLeaderboard.text = "Unlocked!";
		}	
		if (GameControl.control.placeFirst) {
			placeFirst.color = Color.green;
			placeFirst.text = "Unlocked!";
		}
		if (GameControl.control.buyAllWords) {
			buyAllWords.color = Color.green;
			buyAllWords.text = "Unlocked!";
		}	
		if (GameControl.control.raise500k) {
			raise500k.color = Color.green;
			raise500k.text = "Unlocked!";
		}

		if (GameControl.control.FblaExcellenceAward) {
			FblaExcellenceAward.color = Color.green;
			FblaExcellenceAward.text = "Unlocked!";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
