using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/* The following class handles most of the fundraising aspect of the game. 
   It involves the active and idle donut clicker game. It also manages
   aspects of the shop for fundraising.

*/
public class Donut : MonoBehaviour {
	//Actual image of donut
	public Image donut;
	//Level change fade panel
	public GameObject panel;
	//How many shop items
	public int itemCount=3;
	// Use this for initialization
	void Start () {
		StartCoroutine (addDonutCoroutine());
		catchUp ();
	}
	/*
	This denotes how much money to add per second. It is determined by taking the value of idle shop
	items and multiplieing it by how many. 
	*/
	public  float DonutsPerClick()
	{
		//Instantiate shop items based on the count
		float value = 0;
		for (int x = 0; x < itemCount; x++)
			value += GameControl.control.shopCounts [x] * GameControl.control.shopItemValue[x];
				return value;
	}
	/*
	Every time this function is called, add whatever the value of DonutsPerClick is and divide it by 10
	It is divided by 10 because it is called 10 times a seconds rather than 1 to allow fluidness.
	*/
	public void addDonut(){
		GameControl.control.totalMoney += DonutsPerClick () / 10;
		GameControl.control.totalMoneyEver+= DonutsPerClick () / 10;
	}
	/*
	This function calaculates how much money should have been earned when the game was idle
	by taking the current time minus the time of last game exit and multiplieing it by the donuts
	per second. This is then awarded
	*/
	public void catchUp(){
		float timeElapse = (float) DateTime.Now.Subtract (GameControl.control.latestTime).TotalSeconds;
		GameControl.control.totalMoney += DonutsPerClick () * timeElapse;
		GameControl.control.totalMoneyEver += DonutsPerClick () * timeElapse;

	}
	/*
	Using a coroutine allows us update the money value based on a set time frame, 
	controllnig execution time of the function. Rather than calling the function every second,
	it is called every tenth of a second so that the money updates more fluidly.
	*/
	IEnumerator addDonutCoroutine()
	{
		while(true)
			{
			addDonut ();
			yield return new WaitForSeconds (.1f);
		}
	}
	// Update is called once per frame
	void Update () {
	
/*Just statements for checking achievment unlock status.
*/
		if(GameControl.control.totalMoneyEver>=10000 && !GameControl.control.raising10k)
		{
			GameControl.control.raising10k = true;
			GameControl.control.awardPointCount += 1;
			GameControl.control.checkForGoalPoints();
			GameControl.control.awardUnlocked ();


		}
		if(GameControl.control.totalMoneyEver>=500000 && !GameControl.control.raise500k)
		{
			GameControl.control.raise500k = true;
			GameControl.control.awardPointCount += 2;
			GameControl.control.checkForGoalPoints();

			GameControl.control.awardUnlocked ();
		}
		if (Input.GetKeyDown (GameControl.control.backInput)) {
			GameControl.control.latestTime = DateTime.Now;
			panel.GetComponent<FadeControl> ().levelChange ("fblabuilding", panel);
		
		}
		GameControl.control.donutsPerSecond = DonutsPerClick();
		/*
		This is the active fundraising portion of this game. When the spacebar is clicked and shop is closed,
		add a flat value 1 to the donut score plus moneyClickMultiplier, which is how much a click is worth.
		The size of the image of the donut is shrunk when spacebar is held and returned to normal when released,
		this provides a better experience for the user as it is more responsive
		*/
		if (!GameControl.control.isDonutShopOpen) {
		
			if (Input.GetKeyDown (KeyCode.Space)) {
				gameObject.GetComponent<RectTransform> ().localScale = new Vector2 (.8f, .8f);
			}
			if (Input.GetKeyUp (KeyCode.Space)) {
				gameObject.GetComponent<RectTransform> ().localScale = new Vector2 (1, 1);
				GameControl.control.totalMoney += 1+GameControl.control.moneyClickMultiplier;
				GameControl.control.totalMoneyEver += 1 + GameControl.control.moneyClickMultiplier;
			}
		}
	}
}
