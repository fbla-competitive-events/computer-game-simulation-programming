using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManagerBugCheck : MonoBehaviour {

	public int numLives = 3;
	public GameObject canvas;
	public GameObject lifeBar;
	public GameObject gameOver;
	public static lifeManagerBugCheck control;
	public GameObject heartPrefab;
	public GameObject[] hearts;
	public GameObject panel;
	public bool isGameOver;
	private float spacing=0;
	void Awake()
	{
		control = this;
	}
	void Start() {
		hearts = new GameObject[numLives];
		for (int i = 0; i < numLives; i++) {
			//GameObject temp =Instantiate (heartPrefab, new Vector3((Screen.width-50)-spacing, Screen.height-50,1), Quaternion.identity, canvas.transform) as GameObject;
				//hearts [i] = temp;
			hearts[i]=Instantiate (heartPrefab, new Vector3(0, 0, 0), Quaternion.identity, lifeBar.transform) as GameObject;

		}
	}
	public void removeLife(){
		if (!isGameOver) {
			hearts [numLives - 1].GetComponent<heart> ().dead = true;
			numLives--;
		}
		if (numLives == 0) {
			
		
			isGameOver=true;
			gameOver.GetComponent<Animator> ().SetBool ("isGameOver", isGameOver);
			Instantiate (EnemyManager.control.deathEffect, EnemyManager.control.mainCharacter.transform.position, Quaternion.identity);

			Destroy (EnemyManager.control.mainCharacter.gameObject);
			GameControl.control.allGames [GameControl.control.currentGameSelectionIndex].bugs -= EnemyManager.control.score;
		}
	}
	void Update()
	{
		if (isGameOver&& Input.GetKeyDown(GameControl.control.backInput)) {
			GameControl.control.changeScene (panel);
		}
	}
	// Update is called once per frame

}
