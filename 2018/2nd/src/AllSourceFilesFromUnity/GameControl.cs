using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;


/* This script is a comprehensive link between all the various scenes 
	and activites in the game. Variables are communicated using a Singleton Design pattern.
	In addition, GameControl manages the save and load functions of the game
*/
public class GameControl : MonoBehaviour {
	
	//Lifetime awards
	public int awardPointCount=0;
	public GameObject awardAnim;
	//Tier1 Awards
	public bool buyWord;
	public bool placingMedal;
	public bool raising10k;
	public bool submitToLeaderboard;
	//Tier2 Awards
	public bool placeFirst;
	public bool buyAllWords;
	public bool raise500k;
	//FBLA ULTIMATE MEDAL
	public bool FblaExcellenceAward;
	//UI bindings
	public KeyCode eInput= KeyCode.E;
	public KeyCode backInput=KeyCode.B;

	public KeyCode leftInput= KeyCode.LeftArrow;
	public KeyCode rightInput= KeyCode.RightArrow;
	public KeyCode upInput=KeyCode.UpArrow;
	public KeyCode downInput= KeyCode.DownArrow;

	public KeyCode cancelInput=KeyCode.C;

	//General
	public bool doesCurrentGameExist;
	//Leaderboard Variables
	public int topGameRank;
	public string topGameIdentifier;

	public string topGameName;
	//shop
	public bool isShopItselfOpen;

	public bool isShopOpen;
	public List<string> gameShopItems;
	public List<float> shopItemsCosts;
	public List<GameObject> shopItemsObject;
	public GameObject selectedItem;
	public int itemCount = 0;
	public int selectedItemIndex;
	//compete
	public double rating;
	public double[] competeingRatings=new double[3];
	public DateTime competeAgainDate;
	public bool isCompeting;
	public bool canCompete = true;
	public GameObject results;
	//fundraiser
	public float totalMoney;
	public float totalMoneyEver;
	public int moneyClickMultiplier = 0;

	public List<ShopItem> shopItems;
	public int[] shopCounts={0,0,0,0};
	public float[] shopItemValue={.1f,1,10,50};
	public float[] shopPrices={10,100,1000,10000};
	public float donutsPerSecond;
	public  DateTime latestTime;
	//donut shop
	public bool isDonutShopOpen;
	public GameObject selectedDonutShopButton=null;
	public int currentDonutShopSelection = 0;
	public Rect screenRect;
	//Dialog Box;
	public bool isDialogOpen;
	public int currentDialogButtonSelectionIndex = 0;
	public GameObject selectedDialogButton;
	//computer
	public int gameCount=0;
		//computer navigation
	public int currentGameSelectionIndex=0;
	public bool isClearBugsClicked;
	public int currentGameButtonSelectionIndex = 0;
	public bool isPCOpen;

	public GameObject selectedGame=null;
	public GameObject selectedButton;
	//Level system
	public int programmingLevel=1;
	public int bugCheckLevel=1;
	//Scene Management
	public string currentScene;
	public string latestScene;
	public Vector3 latestCharPositionIndoors;
	public Vector3 latestCharPositionOutdoors;

	//vars specific to TYPING GAME
	public bool wordStarted;
	public bool isGameOver=false;
	//vars shared across game scenes
	public bool hasMainHelpMenuBeenSeen;
	public List<string> unlockedWordList;
	public List<string> allGamesNames;
	public List<CreatedGame> allGames;
	public static  GameControl control;
	public int upperarm=0;
	public bool isCharFlipCorrect=true;
	public int lowerarm= 0;
	public int lowerleg= 0;
	public int upperleg= 0;
	public int body= 0;
	public Color skintone;
	public Color hairColor;
	public int shoes= 0;
	public int hair= 0;
	//sprite lists, this list manages character customization
	public Sprite[] hairSpriteList;
	public Sprite[] bodySpriteList;
	public Sprite[] upperSpriteList;
	public Sprite[] faceSpriteList;
	public int selectedBody;
	public int selectedFace;
	public int selectedHair;
	public AudioClip[] music;
	public AudioSource mainAudio;
	//current scene
	public string sceneName;
	// Use this for initialization

	/*The following are the slider functions
	  that allow for custom UI input. They take 
	  user choice as a float and bind keys as such
	  */
	public void changeEInput(float x)
	{
		if (x == 0) {
			eInput = KeyCode.E;
		} else
			eInput = KeyCode.T;
	}
	public void changeCancelInput(float x)
	{
		if (x == 0) {
			cancelInput = KeyCode.C;
		} else
			cancelInput = KeyCode.X;
		
	}
	public void changeBackInput(float x)
	{
		if (x == 0) {
			backInput = KeyCode.B;
		} else
			backInput = KeyCode.Backspace;
	}
	public void changeNavInput(float x)
	{
		if (x == 0) {
			leftInput = GameControl.control.leftInput;
			rightInput = GameControl.control.rightInput;
			upInput = GameControl.control.upInput;
			downInput = GameControl.control.downInput;
		} else {
			leftInput = KeyCode.A;
			rightInput = KeyCode.D;
			upInput = KeyCode.W;
			downInput = KeyCode.S;
		}
	}
	//Awards, check if enough points have been achieved for fbla award,
// If so, award it!
	public void checkForGoalPoints()
	{
		if (awardPointCount >= 3) {
			FblaExcellenceAward = true;
			awardUnlocked ();
		}
	}
	//If any award is unlocked, play the animation
	public void awardUnlocked()
	{
		GameObject award = Instantiate (awardAnim);
		Destroy (award, 3f);
	}

	void OnApplicationQuit()
	{
		//If in fundraise scence, set the last date to current data. Aids in money calculation
		if (SceneManager.GetActiveScene ().name == "fundraise") {
			latestTime = DateTime.Now;
		}
		if(SceneManager.GetActiveScene().name!="customize"&& SceneManager.GetActiveScene().name!="startreal") 
		Save ();
	}
	void Awake () {
		//This Rect is reffered to for certain click events across game
		screenRect = new Rect (0,0, Screen.width, Screen.height-80);
		//Keeping track of the current scene name allows to control some of the logic decisions in the game.
		sceneName = SceneManager.GetActiveScene ().name;
		//The following if statement is part of the singketon design. A public static 'control' is set to this script to make all variables accesible globally
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		}
		else if (control != this) {
			Destroy(gameObject);
		}
		//Is there a save file?
		if (!File.Exists (Application.persistentDataPath + "/saveinfo.dat")) {
			doesCurrentGameExist = false;
		} else if (File.Exists (Application.persistentDataPath + "/saveinfo.dat")) {
			doesCurrentGameExist = true;
		}
	}
	public void sceneChanged()
	{
		
	}
	//Extensions of the fundraising component. Details found in the 'Donut' script
	public  float DonutsPerClick()
	{
		float value = 0;
		for (int x = 0; x <4; x++)
			value += GameControl.control.shopCounts [x] * GameControl.control.shopItemValue[x];
		return value;
	}
	public void addDonut(){
		GameControl.control.totalMoney += DonutsPerClick () / 10;
		GameControl.control.totalMoneyEver += DonutsPerClick () / 10;
	}
	public void catchUp(){
		float timeElapse = (float) DateTime.Now.Subtract (GameControl.control.latestTime).TotalSeconds;
		GameControl.control.totalMoney += DonutsPerClick () * timeElapse;
		GameControl.control.totalMoneyEver += DonutsPerClick () * timeElapse;

	}

	// Update is called once per frame
	void Update () {
		if (gameShopItems.Count <= 0 && !GameControl.control.buyAllWords) {
			GameControl.control.buyAllWords = true;
			GameControl.control.awardPointCount += 2;
			GameControl.control.checkForGoalPoints ();
			awardUnlocked ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		//Music Manager
		if (sceneName != SceneManager.GetActiveScene ().name) {
			if (SceneManager.GetActiveScene ().name == "startreal") {
				mainAudio.clip = music [0];
				mainAudio.Play ();
			}
			if (SceneManager.GetActiveScene ().name == "bugcheck" || SceneManager.GetActiveScene ().name == "programming") {
				mainAudio.clip = music [2];
			} else {
				mainAudio.clip = music [1];
				mainAudio.Play ();
			}
			if (SceneManager.GetActiveScene ().name == "playerhome"&& isCompeting) {
				StartCoroutine (compete());
			}
			//Close all windows and dialog counts if scene is changed
			isPCOpen = false;
			isClearBugsClicked = false;
			gameCount = 0;
			//selectedDialogButtonIndex = 0;
			//selectedButton = 0;
			currentDialogButtonSelectionIndex = 0;
			currentGameButtonSelectionIndex = 1;
			isDialogOpen = false;
			itemCount = 0;
			isDonutShopOpen = false;
			sceneName = SceneManager.GetActiveScene ().name;
		}
	if (!isCompeting && !canCompete &&DateTime.Now>=competeAgainDate) {
			canCompete = true;
		}
	
	}
	//Controls FBLA competitive event competition
	IEnumerator compete()
	{
		yield return new WaitForSeconds (1);
		int rank = 4;
		for (int x=0; x<3;x++){
			competeingRatings[x] = (Double)Random.Range (5.0f, 8.0f);
			if (competeingRatings[x]<= rating) {
				rank--;
			}
		}

		GameObject obj=Instantiate (results);
		obj.GetComponent<results> ().rank = rank;
		isCompeting = false;
		competeAgainDate = DateTime.Now.AddMinutes (15);
	}
	/*The following 2 blocks of code are part of the character customization menu. 
	These  blocks assign the value*/
	public  void changeSkinTone(float newskintone, GameObject headS, GameObject lowerarmS, GameObject lowerarmleftS, GameObject neckS, GameObject handS, GameObject handleftS)
	{
		GameControl.control.skintone= new Color (0.5F*newskintone,0.3F*newskintone,0.2F*newskintone,1.0F);

		headS.GetComponent<SpriteRenderer> ().color = skintone;
		lowerarmS.GetComponent<SpriteRenderer> ().color = skintone;
		lowerarmleftS.GetComponent<SpriteRenderer> ().color = skintone;
		neckS.GetComponent<SpriteRenderer> ().color = skintone;
		handS.GetComponent<SpriteRenderer> ().color = skintone;
		handleftS.GetComponent<SpriteRenderer> ().color = skintone;

	}

	public void changeHairColor(float haircolorvalue)
	{
		Color hairc;
		//Color hairc= Color.HSVToRGB(haircolorvalue,1f,haircolorvalue);
		if (haircolorvalue == 0) {
			hairc = new Color (0F, 0F, 0F, 1.0F);

		} else if (haircolorvalue == 1) {
			hairc = new Color (.36F, .25F, .20F, 1.0F);

		} else if (haircolorvalue == 2) {
			hairc = new Color (0.6F, 0.37F, 0.26F, 1.0F);

		} else if (haircolorvalue == 3) {
			hairc = new Color (.66F, .66F, .66F, 1.0F);
		} else if (haircolorvalue == 4) {
			hairc = new Color (.50F, .45F, .24F, 1.0F);
			
		} else if (haircolorvalue == 5) {
			hairc = new Color (.98F, .94F, .74F, 1.0F);
		} else if (haircolorvalue == 6) {
			hairc = new Color (0.3F, 0F, .78F, 1.0F);
		} else if (haircolorvalue == 7) {
			hairc = new Color (0F, .78F, 0F, 1.0F);
		} else if (haircolorvalue == 8) {
			hairc = new Color (.78F, 0F, 0F, 1.0F);
		} else if (haircolorvalue == 9) {
			hairc = new Color (1F, 1F, 1F, 1.0F);
		} else if (haircolorvalue == 10) {
			hairc = new Color (1F, .3F, .5F, 1.0F);

		} else if(haircolorvalue==11){
			hairc = new Color (.1F, .6F, .7F, 1.0F);
		}
		else{
			hairc = new Color (0f, 0f, .78F, 1.0F);

		}
			
		GameObject.Find ("hair").GetComponent<SpriteRenderer> ().color = hairc;
		hairColor = hairc;
	}


	public void Save()
	{
		Debug.Log ("saving");

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/saveinfo.dat");
		saveData data = new saveData ();
		data.allGames = allGames;
		data.upperarm = upperarm;
		data.lowerarm = lowerarm;
		data.lowerleg = lowerleg;
		data.upperleg = upperleg;
		data.body = body;
		data.skintoneR = skintone.r;
		data.skintoneG= skintone.g;
		data.skintoneB = skintone.b;


		data.shoes = this.shoes;
		data.hair = hair;
		data.hairR =hairColor.r;			
		data.hairG =hairColor.g;
		data.hairB =hairColor.b;



		data.selectedBody=selectedBody;
		data.selectedFace=selectedFace;
		data.selectedHair=selectedHair;
		data.unlockedWordList=unlockedWordList;
		data.allGamesNames=allGamesNames;
		data.rating=rating;
		data.competeingRatings=competeingRatings;
		data.competeAgainDate=competeAgainDate;
		data.isCompeting=isCompeting;
		data.canCompete = canCompete;
		data.totalMoney=totalMoney;		
		data.totalMoneyEver=totalMoneyEver;

		data.gameShopItems = gameShopItems;
		data.shopItemsCosts = shopItemsCosts;
		data.moneyClickMultiplier =moneyClickMultiplier;

		data.shopCounts=shopCounts;
		data.shopItemValue = shopItemValue;
		data.shopPrices=shopPrices;
		data.donutsPerSecond=donutsPerSecond;
		data.latestTime=latestTime;
		data.awardPointCount=awardPointCount;
		//Tier1
		data.buyWord=buyWord;
		data.placingMedal=placingMedal;
		data.raising10k=raising10k;
		data.submitToLeaderboard=submitToLeaderboard;
		//Tier2
		data.placeFirst=placeFirst;
		data.buyAllWords=buyAllWords;
		data.raise500k=raise500k;
		//FBLA ULTIMATE MEDAL
		data.FblaExcellenceAward=FblaExcellenceAward;


		bf.Serialize (file, data);
		file.Close ();
	}
	public  void SaveLoad()
	{
		if (File.Exists (Application.persistentDataPath + "/saveinfo.dat")) {
			Debug.Log ("loading");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file=File.Open(Application.persistentDataPath+"/saveinfo.dat",FileMode.Open);
			saveData data = (saveData)bf.Deserialize (file);
			file.Close ();
			topGameName = data.topGameName;
			topGameIdentifier = data.topGameIdentifier;
			allGames = data.allGames;
			upperarm = data.upperarm;
			lowerarm = data.lowerarm;
			lowerleg = data.lowerleg;
			upperleg = data.upperleg;
			body = data.body;
			skintone = new Color(data.skintoneR,data.skintoneG,data.skintoneB,1f);
			shoes = data.shoes;
			hair = data.hair;
			hairColor = new Color(data.hairR,data.hairG,data.hairB,1f);
			gameShopItems = data.gameShopItems;
			shopItemsCosts = data.shopItemsCosts;
			selectedBody=data.selectedBody;
			selectedFace=data.selectedFace;
			selectedHair=data.selectedHair;
			unlockedWordList=data.unlockedWordList;
			allGamesNames=data.allGamesNames;
			rating=data.rating;
			competeingRatings=data.competeingRatings;
			competeAgainDate=data.competeAgainDate;
			isCompeting=data.isCompeting;
			canCompete = data.canCompete;
			totalMoney=data.totalMoney;			
			totalMoneyEver=data.totalMoneyEver;
		

			moneyClickMultiplier =data.moneyClickMultiplier;

			shopCounts=data.shopCounts;
			shopItemValue = data.shopItemValue;
			shopPrices=data.shopPrices;
			donutsPerSecond=data.donutsPerSecond;
			latestTime=data.latestTime;

			awardPointCount=data.awardPointCount;

			//Tier1
			buyWord=data.buyWord;
			placingMedal=data.placingMedal;
			raising10k=data.raising10k;
			submitToLeaderboard=data.submitToLeaderboard;
			//Tier2
			placeFirst=data.placeFirst;
			buyAllWords=data.buyAllWords;
			raise500k=data.raise500k;
			//FBLA ULTIMATE MEDAL
			FblaExcellenceAward=data.FblaExcellenceAward;


		} else {
			Debug.Log ("saving");

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/saveinfo.dat");
			saveData data = new saveData ();
			data.topGameIdentifier = topGameIdentifier;
			data.topGameName = topGameName;
			data.allGames = allGames;
			data.upperarm = upperarm;
			data.lowerarm = lowerarm;
			data.lowerleg = lowerleg;
			data.upperleg = upperleg;
			data.body = body;
			data.skintoneR = skintone.r;
			data.skintoneG= skintone.g;
			data.skintoneB = skintone.b;
			data.gameShopItems = gameShopItems;
			data.shopItemsCosts = shopItemsCosts;

			data.shoes = this.shoes;
			data.hair = hair;
			data.hairR =hairColor.r;			
			data.hairG =hairColor.g;
			data.hairB =hairColor.b;
			data.awardPointCount=awardPointCount;
			//Tier1
			data.buyWord=buyWord;
			data.placingMedal=placingMedal;
			data.raising10k=raising10k;
			data.submitToLeaderboard=submitToLeaderboard;
			//Tier2
			data.placeFirst=placeFirst;
			data.buyAllWords=buyAllWords;
			data.raise500k=raise500k;
			//FBLA ULTIMATE MEDAL
			data.FblaExcellenceAward=FblaExcellenceAward;



			data.selectedBody=selectedBody;
			data.selectedFace=selectedFace;
			data.selectedHair=selectedHair;
			data.unlockedWordList=unlockedWordList;
			data.allGamesNames=allGamesNames;
			data.rating=rating;
			data.competeingRatings=competeingRatings;
			data.competeAgainDate=competeAgainDate;
			data.isCompeting=isCompeting;
			data.canCompete = canCompete;
			data.totalMoney=totalMoney;
			data.totalMoneyEver=totalMoneyEver;

			data.moneyClickMultiplier =moneyClickMultiplier;

			data.shopCounts=shopCounts;
			data.shopItemValue = shopItemValue;
			data.shopPrices=shopPrices;
			data.donutsPerSecond=donutsPerSecond;
			data.latestTime=latestTime;

			 
			bf.Serialize (file, data);
			file.Close ();
		}
	}
	public  void StartButton(){
		if (File.Exists (Application.persistentDataPath + "/saveinfo.dat")) {
		
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file=File.Open(Application.persistentDataPath+"/saveinfo.dat",FileMode.Open);
			saveData data = (saveData)bf.Deserialize (file);
			file.Close ();
			allGames = data.allGames;
			upperarm = data.upperarm;
			lowerarm = data.lowerarm;
			lowerleg = data.lowerleg;
			upperleg = data.upperleg;
			body = data.body;
			skintone = new Color(data.skintoneR,data.skintoneB,data.skintoneG);
			shoes = data.shoes;
			hair = data.hair;
		}
		SceneManager.LoadScene("customize",LoadSceneMode.Single);
	}
	public void changeScene(GameObject panel)
	{
		panel.GetComponent<FadeControl> ().levelChange (latestScene,panel);
	}

	public void StartGame()
	{
		
		SceneManager.LoadScene("main",LoadSceneMode.Single);
	}
	public void Load()
	{
		Debug.Log ("loading");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file=File.Open(Application.persistentDataPath+"/saveinfo.dat",FileMode.Open);
		saveData data = (saveData)bf.Deserialize (file);
		file.Close ();
		topGameName = data.topGameName;
		topGameIdentifier = data.topGameIdentifier;
		allGames = data.allGames;
		upperarm = data.upperarm;
		lowerarm = data.lowerarm;
		lowerleg = data.lowerleg;
		upperleg = data.upperleg;
		body = data.body;
		skintone = new Color(data.skintoneR,data.skintoneG,data.skintoneB,1f);
		shoes = data.shoes;
		hair = data.hair;
		hairColor = new Color(data.hairR,data.hairG,data.hairB,1f);

		selectedBody=data.selectedBody;
		selectedFace=data.selectedFace;
		selectedHair=data.selectedHair;
		unlockedWordList=data.unlockedWordList;
		allGamesNames=data.allGamesNames;
		rating=data.rating;
		competeingRatings=data.competeingRatings;
		competeAgainDate=data.competeAgainDate;
		isCompeting=data.isCompeting;
		canCompete = data.canCompete;
		totalMoney=data.totalMoney;
		moneyClickMultiplier =data.moneyClickMultiplier;
		gameShopItems = data.gameShopItems;
		shopItemsCosts = data.shopItemsCosts;
		shopCounts=data.shopCounts;
		shopItemValue = data.shopItemValue;
		shopPrices=data.shopPrices;
		donutsPerSecond=data.donutsPerSecond;
		latestTime=data.latestTime;



		awardPointCount=data.awardPointCount;
		//Tier1
		buyWord=data.buyWord;
		placingMedal=data.placingMedal;
		raising10k=data.raising10k;
		submitToLeaderboard=data.submitToLeaderboard;
		//Tier2
		placeFirst=data.placeFirst;
		buyAllWords=data.buyAllWords;
		raise500k=data.raise500k;
		//FBLA ULTIMATE MEDAL
		FblaExcellenceAward=data.FblaExcellenceAward;

	}
	void OnGUI()
	{
	}
}

// List of variables that need to be saved and loaded
[System.Serializable]
class saveData:System.Object
{
	public int awardPointCount;
	//Tier1
	public bool buyWord;
	public bool placingMedal;
	public bool raising10k;
	public bool submitToLeaderboard;
	//Tier2
	public bool placeFirst;
	public bool buyAllWords;
	public bool raise500k;
	//FBLA ULTIMATE MEDAL
	public bool FblaExcellenceAward;
	public List<string> gameShopItems;
	public List<float> shopItemsCosts;
	public float totalMoneyEver;
	public string topGameName;
	public string topGameIdentifier;
	public List<CreatedGame> allGames;
	public int upperarm;
	public int lowerarm;
	public int lowerleg;
	public int upperleg;
	public int body;
	public float skintoneR;
	public float skintoneG;
	public float skintoneB;
	public float hairR;
	public float hairG;
	public float hairB;
	public int shoes;
	public int hair;
	public int selectedBody;
	public int selectedFace;
	public int selectedHair;
	public List<string> unlockedWordList;
	public List<string> allGamesNames;
	public double rating;
	public double[] competeingRatings=new double[3];
	public DateTime competeAgainDate;
	public bool isCompeting;
	public bool canCompete = true;
	public float totalMoney;
	public int moneyClickMultiplier = 0;

	public int[] shopCounts={0,0,0,0};
	public float[] shopItemValue={.1f,1,10,50};
	public float[] shopPrices={10,100,1000,10000};
	public float donutsPerSecond;
	public  DateTime latestTime;

}