using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
public class LeaderboardActionSelectArrow : MonoBehaviour {
	public Transform arrowObject;
	public Animator leaderboard;
	public GameObject panel;
	List<LeaderboardEntry> allEntries= new List<LeaderboardEntry>();
	public CanvasGroup text;
	public CanvasGroup connectToWifi;
	bool canSubmit=true;
	public List<Vector3> positions; 
	public int buttonIndex=1;
	// Use this for initialization

	void Start () {
		text.gameObject.GetComponent<TextMeshProUGUI> ().text = "Press " +GameControl.control.eInput.ToString()+" to close leaderboard";

	}
	void Awake()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://fblagame-3c891.firebaseio.com/");

	}
	/* Check for connection to internet by pinging google
	if succesful hide connect to internet message*/
	IEnumerator checkInternetConnection(){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			connectToWifi.alpha=1;
		} else {
			connectToWifi.alpha = 0;
		}
	} 
	// Update is called once per frame
	void Update () {
		/*
		This indexing move button between the two button options in the scene

		*/
		if (Input.GetKeyDown (GameControl.control.upInput) && buttonIndex < 1) {
			buttonIndex++;
			arrowObject.localPosition = positions [buttonIndex];
		} else if (Input.GetKeyDown (GameControl.control.downInput) && buttonIndex > 0) {
			buttonIndex--;
			arrowObject.localPosition = positions [buttonIndex];

			}
		// open and close leader board
		if (buttonIndex == 1 && Input.GetKeyDown(KeyCode.Return)) {
			text.gameObject.GetComponent<TextMeshProUGUI> ().text = "Press '" +GameControl.control.eInput.ToString()+"' to close leaderboard";

			leaderboard.SetBool ("zoomedIn", true);
			text.alpha = 1;
		}
		if(Input.GetKeyDown(GameControl.control.eInput) && leaderboard.GetBool("zoomedIn"))
		{
			leaderboard.SetBool ("zoomedIn", false);
			text.alpha = 0;
		}
		//Submit button
		if (buttonIndex == 0 && Input.GetKeyDown(KeyCode.Return)) {
			canSubmit = false;

			submitGame();

		}
		if (Input.GetKeyDown (GameControl.control.backInput)) {
			panel.GetComponent<FadeControl> ().levelChange ("playerhome", panel);

		}
	}
	void submitGame()
	{
		StartCoroutine(checkInternetConnection());
		CreatedGame yourTopGame;
		//Determines what game to submit. Submtis game with highest rating
		if (GameControl.control.allGames.Count > 0) {
			yourTopGame = GameControl.control.allGames [0];
			foreach (CreatedGame x in GameControl.control.allGames) {
				if (x.rating > yourTopGame.rating) {
					yourTopGame = x;
				} 
			}
			//if topGame name already registered and is the same, dont submit and display this message
			if (yourTopGame.name == GameControl.control.topGameName) {
			
				text.gameObject.GetComponent<TextMeshProUGUI> ().text = "Top Game Already Submitted and Updated!";
				text.alpha = 1;
			} else {
				//Checks for award
				if (!GameControl.control.submitToLeaderboard) {
					GameControl.control.submitToLeaderboard = true;
					GameControl.control.awardPointCount += 1;
					GameControl.control.checkForGoalPoints();
					GameControl.control.awardUnlocked();
				}
				uploadToFirebase (yourTopGame);
			}
		}
		//If no game created.
			else  {
				text.gameObject.GetComponent<TextMeshProUGUI> ().text = "You have no applicable games to submit!";
			
			text.alpha = 1;

		}


	}
	/*
	Gets entry count
	*/
	void uploadToFirebase(CreatedGame game)
	{
		int gameNumber;
		FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("entryCount").GetValueAsync().ContinueWith (task => {
			if(task.IsCanceled)
			{
				Debug.Log("Faulted");
			}
			else if(task.IsCompleted)
			{
				DataSnapshot snapshot=task.Result;
				gameNumber=int.Parse(snapshot.Value.ToString()) +1;
				FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("entryCount").SetValueAsync(gameNumber);
				checkForEqualGames(gameNumber-2,gameNumber,game);
			}
		});
	}
	/* 
	This is the first phase for checking equal games, first it adds all games 
	using recursion, and then it calls phase 2
	*/
	void checkForEqualGames(int recursionCount, int gameNumber, CreatedGame game)
	{
		FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("Scores").Child(recursionCount.ToString()).GetValueAsync ().ContinueWith (task=>{
				if(task.IsFaulted)
				{
					Debug.Log("fauted");

				}
				else if(task.IsCompleted)
				{
					
				DataSnapshot snapshot= task.Result;
				allEntries.Add(JsonUtility.FromJson<LeaderboardEntry>(snapshot.GetRawJsonValue()));
				if(recursionCount>0)
				checkForEqualGames(recursionCount-1,gameNumber,game);
			else
				{
				checkForEqualGamesTwo(gameNumber,game);
				}
				}
			});

	}
	/*
	Phase 2 detects if there is agame in leaderboard with same name, if so add an asterik
	to the submitted game name and change the identifier to that new modified name.
	This solves issue of duplicate entries. Then submit game

	*/
	void checkForEqualGamesTwo( int gameNumber, CreatedGame game)
	{
		string gameIdentifier = game.name;	
		GameControl.control.topGameIdentifier = game.name;


		GameControl.control.topGameName = game.name;
		for(int x=0;x<allEntries.Count;x++)
		{
			if (allEntries[x].name == gameIdentifier) {
				gameIdentifier+="*";
				x = -1;
			}
		}
		GameControl.control.topGameIdentifier = gameIdentifier;
		LeaderboardEntry entry = new LeaderboardEntry (gameIdentifier, game.rating);
		FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child("Scores").Child((gameNumber-1).ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(entry));
		canSubmit = true;

	}
}
