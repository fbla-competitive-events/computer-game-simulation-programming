using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class gameStats : MonoBehaviour {
	public int bugs;
	public string name="";
	bool isNameUsed=false;
	public GameObject begin;
	public InputField nameInput;
	public GameObject nameGamePanel;
	public GameObject fadePanel;
	public Animator nameGamePanelAnimator;
	public List<wordCount> finalWC;
	public GameObject invalidName;
	public static int gameScore=0;
	// Use this for initialization
	void Awake() {
		nameGamePanelAnimator = nameGamePanel.GetComponent<Animator> ();
	}
	//when someone types a word
	public void gameComplete()
	{
		GameControl.control.isGameOver = true;
		nameGamePanelAnimator.SetBool ("isZoomedIn", true);
		nameInput.Select ();
	}
	void OnMouseUp()
	{
		if (GameControl.control.isGameOver) {
			nameInput.Select ();

		}
	}
	// Update is called once per frame
	void Update () {
		if (GameControl.control.isGameOver) {
			nameInput.Select ();

		}
		if (GameControl.control.isGameOver &&Input.GetKeyDown(KeyCode.Return)) {
			addToGameList ();
		}
		if (!GameControl.control.wordStarted && Input.GetKeyDown (KeyCode.Space)) {
			begin.GetComponent<CanvasGroup> ().alpha = 0;
			GameControl.control.mainAudio.Play ();

			GameControl.control.wordStarted = true;
		}
	}
	public void addToGameList()
	{
		Debug.Log (isNameUsed);

		foreach (string n in GameControl.control.allGamesNames) {
		
			if (name.Equals (n)) {
				isNameUsed = true;
			} else {
				isNameUsed = false;
			}
		}
		Debug.Log (isNameUsed);
		if (name != "" && !isNameUsed) {
			
			CreatedGame game = new CreatedGame (name, bugs, finalWC, gameScore);
			GameControl.control.allGames.Add (game);
			GameControl.control.allGamesNames.Add (name);
			gameScore = 0;
			GameControl.control.isGameOver = false;
			isNameUsed = false;
			GameControl.control.wordStarted = false;

			GameControl.control.changeScene (fadePanel);

		} else {
			TextMeshProUGUI[] textArray =nameGamePanel.GetComponentsInChildren<TextMeshProUGUI>();
			foreach(TextMeshProUGUI text in textArray)
			{
				if (text.text =="Name your Game:") {
					text.color = Color.red;
					text.text="Invalid Name";
				
				}
			}
		}
		nameInput.Select ();
		nameInput.ActivateInputField ();
	}
	public void gameName(string name)

	{
		this.name = name;
	}
}
