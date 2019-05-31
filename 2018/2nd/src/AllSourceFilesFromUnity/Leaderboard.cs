using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
/*
The following class contains most of the retrieval functions of the leaderboard. 
it pulls all entries from a firebase and sorts them. It ranks top 5, as well as your
own top submitted game.
*/
public class Leaderboard : MonoBehaviour {
	//This list is of the actual display objects for top entries
	private List<EntryDisplay> displayValues= new List<EntryDisplay>();
	//This is the container where displayValues will be instantiated
	public GameObject leaderboardDisplay;
	//This is text that will be displayed and changed based on state of leaederboard
	public CanvasGroup text;
	//This text signals internet connection issues
	public CanvasGroup connectToWifi;
	// This is the actual prefab instantiated by displayValues
	public GameObject entryPrefab;
	// This is a list of all game entries on the leaderboard
	public List<LeaderboardEntry> allEntries = new List<LeaderboardEntry> ();
	// Use this for initialization
	void Awake () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://fblagame-3c891.firebaseio.com/");

		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		StartCoroutine (checkInternetConnection ());
	}
	/*
	Creates a call to google, if a connection can't made be made, display the 
	internet connection error.
	*/
	IEnumerator checkInternetConnection(){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			connectToWifi.alpha=1;
		} else {
			connectToWifi.alpha = 0;
		}
	} 

	void Start()
	{
		//Creates 5 entry displays
		for (int x = 0; x < 5; x++) {
			GameObject entry= Instantiate (entryPrefab,new Vector3(0,0),Quaternion.identity, leaderboardDisplay.transform);
			EntryDisplay entryScript = entry.GetComponent<EntryDisplay> ();
			entryScript.rank.text = "?";
			entryScript.nameText.text="???";
			entryScript.rating.text = "???";
			displayValues.Add (entryScript);

		}
		//If a value changes in Scores, call the HandlValueChanged function
		FirebaseDatabase.DefaultInstance
			.GetReference("Leaderboard").Child("Scores")
			.ValueChanged += HandleValueChanged;
	}
	//Pull how mmany leader board entries, then call uploadLeaderBoardEntries base on that value
	 void updateLeaderboardCount()
	{
		
		FirebaseDatabase.DefaultInstance.GetReference("Leaderboard").Child("entryCount").GetValueAsync().ContinueWith(task=> {
			if(task.IsFaulted){
				Debug.Log("Error");
			}
			else if(task.IsCompleted){

				DataSnapshot snapshot= task.Result;
				//Must call functions in a chain like this because firebase API is asynchronous
				updateLeaderboardEntries(int.Parse(snapshot.Value.ToString()));
			}

			});
	// Update is called once per frame

}

/*
For however many entries ther are, pull the entry at location x
Convert that value to a JSON and then store it into an instance of the Leaderboard entry class,
and add it to allEntires.
*/
	void updateLeaderboardEntries(int entryCount)
	{
		if (entryCount == 0) {
			text.alpha=0;
		}
		for (int x = 0; x < entryCount; x++) {
			FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("Scores").Child(x.ToString()).GetValueAsync ().ContinueWith (task => {
				
				if(task.IsFaulted)
				{
					Debug.Log("Error");

				}
				else if(task.IsCompleted)
				{
					DataSnapshot snapshot= task.Result;
					string JSONsnapshot= snapshot.GetRawJsonValue();
					LeaderboardEntry entry = JsonUtility.FromJson<LeaderboardEntry>(JSONsnapshot);
					allEntries.Add(entry);	

					LeaderboardEntry temp;
					/* Standard Insertion sort, compare all values in a allEntires and swap when the 
						proper index is determined.
					*/
					for(int i=1; i<allEntries.Count;i++)
					{
						for(int j=i; j>0; j--)
						{
							if(allEntries[j].getRating()>allEntries[j-1].getRating())
							{
								temp=allEntries[j];
								allEntries[j]=allEntries[j-1];
								allEntries[j-1]=temp;
							}
						}
					}
					//Update the display to reflect sorted list
					updateBoardDisplay();
				}
			}
			);

		}

	}
	void updateBoardDisplay()
	{

		//The first entry in the sorted list will always equ		
		allEntries [0].setRank(1);
		/*
		This loop is key for assigning ranks because it deals with ties.
		If two ratings are equal, the next element in the list acquires rank of previous, else 
		then it equals rank of previous plus 1
		*/
		for (int x = 1; x < allEntries.Count; x++) {
			if (allEntries [x-1].rating == allEntries [x].rating) {
				allEntries [x].setRank (allEntries [x-1].getRank ());
			}
			else{ 
				allEntries[x].setRank((allEntries [x-1].getRank ()+1));
					}
		}
		/*
		This loop assigns the display values for the entry that matches your top submission. If your entry is a top 5,
		then assign top 5 values regularly. If it isn't a top 5 and there are only 5 display elements, make another and
		assign values. Else, just update 6th display value
		*/
		for(int x=0; x<allEntries.Count;x++) {
			if (allEntries [x].name == GameControl.control.topGameIdentifier) {
				if (x < 5 && displayValues.Count < 6) {

					displayValues [x].rank.text = allEntries[x].getRank().ToString ()+".";
					displayValues [x].rank.color = Color.yellow;
					displayValues [x].nameText.text = allEntries [x].getName ();
					displayValues [x].nameText.color = Color.yellow;
					displayValues [x].rating.text =  string.Format("{0:N1}",allEntries [x].getRating ().ToString ())+"/10";
					displayValues [x].rating.color = Color.yellow;
				} else if (displayValues.Count < 6) {

					GameObject entry = Instantiate (entryPrefab, new Vector3 (0, 0), Quaternion.identity, leaderboardDisplay.transform);
					EntryDisplay entryScript = entry.GetComponent<EntryDisplay> ();
					entryScript.rank.text =allEntries[x].getRank().ToString ();
					entryScript.nameText.text = allEntries [x].name;
				
					entryScript.rating.text =  string.Format("{0:N1}",allEntries [x].getRating ().ToString ())+"/10";
					displayValues.Add (entryScript);
				
					displayValues [displayValues.Count - 1].rank.color = Color.yellow;
					displayValues [displayValues.Count - 1].nameText.color = Color.yellow;
					displayValues [displayValues.Count - 1].rating.color = Color.yellow;
				} else {
					displayValues[displayValues.Count - 1].rank.text =allEntries[x].getRank().ToString ()+".";
					displayValues[displayValues.Count - 1].nameText.text = allEntries [x].name;
					displayValues [displayValues.Count - 1].rating.text = allEntries [x].getRating ().ToString ();

				}
			} 
		}
		/*
		Assings values regularly for top 5 display values
		*/
		for(int x=0; x<5 && x<allEntries.Count; x++)			{
			displayValues[x].rank.text = allEntries[x].getRank().ToString()+".";
			displayValues[x].nameText.text=allEntries[x].getName();
			Debug.Log (allEntries [x].rating);

			displayValues[x].rating.text = string.Format("{0:N1}",allEntries[x].getRating().ToString())+"/10";

			}
		text.alpha = 0;
		/*
		This list searches through create games and finds which one matches your topgame submission,
		and then asssign topGame to it
		*/
		CreatedGame topGame=new CreatedGame("NULL",-1);
		foreach (CreatedGame x in GameControl.control.allGames) {
			if (x.name == GameControl.control.topGameName) {
				topGame = x;
			}
		}
		/*This updates the score of top game entry by checking when gC = your top game index
		 if rating is not equal betweene entry pulled and your own, update it.*/
		int gC = 0;

		if (allEntries [gC].name == GameControl.control.topGameIdentifier) {
			Debug.Log(GameControl.control.topGameIdentifier);
			Debug.Log (allEntries [gC].name);
			if (allEntries [gC].rating != topGame.rating) {

				FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("Scores").Child (gC.ToString ()).Child ("rating").SetValueAsync (topGame.rating);
				FirebaseDatabase.DefaultInstance.GetReference ("Leaderboard").Child ("Scores").Child ("placeholder").SetValueAsync (topGame.rating);

			}
		}
		gC++;

	}
	/*If value changed, clear all entires and update again*/
	void HandleValueChanged(object senders, ValueChangedEventArgs args)
	{allEntries.RemoveRange (0, allEntries.Count);
		text.alpha = 1;
		updateLeaderboardCount ();

	}
}