using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AwardBoard : MonoBehaviour {
	//prompt
	public CanvasGroup prompt;
	public bool isInRange;
	public Animator AchievementBoard;
	public bool isZoomedIn;
	//Tier1
	public SpriteRenderer buyWord;
	public SpriteRenderer placingMedal;
	public SpriteRenderer raising10k;
	public SpriteRenderer submitToLeaderboard;
	//Tier2
	public SpriteRenderer placeFirst;
	public SpriteRenderer buyAllWords;
	public SpriteRenderer raise500k;
	//FBLA ULTIMATE MEDAL
	public SpriteRenderer FblaExcellenceAward;
	void OnTriggerEnter2D()
	{
		prompt.alpha = 1;
		isInRange = true;
	}
	void OnTriggerExit2D()
	{
		prompt.alpha = 0;
		isInRange = false;

	}
	// Use this for initialization
	void Start () {
		prompt.gameObject.GetComponent<TextMeshProUGUI>().text= "Press '" + GameControl.control.eInput + "' to toggle achievement board";

		if (GameControl.control.buyWord) {
			buyWord.color = Color.white;
		}
		if (GameControl.control.placingMedal) {
			placingMedal.color = Color.white;
		}	
		if (GameControl.control.raising10k) {
			raising10k.color = Color.white;
		}	
		if (GameControl.control.submitToLeaderboard) {
			submitToLeaderboard.color = Color.white;
		}	
		if (GameControl.control.placeFirst) {
			placeFirst.color = Color.white;
		}
		if (GameControl.control.buyAllWords) {
			buyAllWords.color = Color.white;
		}	
		if (GameControl.control.raise500k) {
			raise500k.color = Color.white;
		}

		if (GameControl.control.FblaExcellenceAward) {
			FblaExcellenceAward.color = Color.white;
		}



	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) && isInRange) {
			if (!isZoomedIn) {
				AchievementBoard.SetBool ("zoomedIn", true);
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;

				isZoomedIn = true;
			}
		
			else if (isZoomedIn) {
				AchievementBoard.SetBool ("zoomedIn", false);
				isZoomedIn = false;
				PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;

			}
		}
	}
}
