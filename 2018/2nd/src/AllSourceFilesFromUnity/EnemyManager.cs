using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public GameObject mainCharacter;
	public int numberOfBugs;
	public int score;
	public GameObject deathEffect;
	//how many available for instantiating
	public int numberOfUnkillableBugs;
	public int clearedBugCount;
	public int selectedGameIndex;
	public GameObject spikedMonster;
	public int tempCount = 0;
	public GameObject normalMonster;
	private float currentTime;
	public bool isGameStarted;
	public GameObject winMessage;
	public GameObject startMessage;
	public float spawnDelay=5f;
	public static EnemyManager control;
	// Use this for initialization
	void Awake () {
		
		control = this;
		selectedGameIndex = GameControl.control.currentGameSelectionIndex;
		numberOfUnkillableBugs = (int)Random.Range (5,10);
		//numberOfBugs = GameControl.control.allGames [selectedGameIndex].bugs-numberOfUnkillableBugs;

		numberOfBugs =  GameControl.control.allGames [selectedGameIndex].bugs;
	}
	void Start()
	{
		mainCharacter.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;
	}
	public void gameOverWin()
	{
		lifeManagerBugCheck.control.isGameOver = true;
		GameControl.control.allGames [GameControl.control.currentGameSelectionIndex].bugs = numberOfBugs;
		GameControl.control.allGames [GameControl.control.currentGameSelectionIndex].reCalculateRating ();
		winMessage.GetComponent<CanvasGroup> ().alpha = 1;

	}
	// Update is called once per frame
	public bool isRangeBad(float r)
	{
		
		if (r < PlatformerCharacter2D.control.transform.position.x + 4 && PlatformerCharacter2D.control!=null&& r > PlatformerCharacter2D.control.transform.position.x - 4)
			return true;
		else if (r + 5.5f > 14.31 || r < -14.21) {
			return true;
		} else
			return false;
		
	}
	void Update () {
		if (isGameStarted) {
			currentTime -= Time.deltaTime;
			if (currentTime < 0) {
				if (tempCount == 5 && numberOfUnkillableBugs >= 1) {
					float range = Random.Range (-20, 20);
					while (isRangeBad (range)) {
						range = Random.Range (-20, 20);
					}
					Instantiate (spikedMonster, new Vector3 (range, 0), Quaternion.identity);
					numberOfUnkillableBugs--;
					tempCount = 0;
				} else if (numberOfBugs >= 1) {
					float range = Random.Range (-20, 20);
					while (isRangeBad (range)) {
						range = Random.Range (-20, 20);
					}
				
					Instantiate (normalMonster, new Vector3 (range, 0), Quaternion.identity);
					numberOfBugs--;
					tempCount++;

			
				} 

				if (spawnDelay > 2) {
					spawnDelay -= .2f;
				}
				currentTime = spawnDelay;
			}
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			startMessage.GetComponent<CanvasGroup> ().alpha = 0;
			GameControl.control.mainAudio.Play ();

			isGameStarted = true;
			mainCharacter.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;
		}
	}
}
