using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopItem : MonoBehaviour {
	public GameObject countText;
	public GameObject priceText;
	public string nameOfItem;
	private float price;
	private int count;
		public int index;
		// Use this for initialization
		void Start () {
			if (GameControl.control.currentDonutShopSelection ==0) {
				GameControl.control.selectedDonutShopButton = gameObject;
			}
		price = GameControl.control.shopPrices [index];
		count = GameControl.control.shopCounts [index];
		}

		// Update is called once per frame
		void Update () {
		priceText.GetComponent<TextMeshProUGUI>().text=price.ToString();
		countText.GetComponent<TextMeshProUGUI> ().text = GameControl.control.shopCounts[index].ToString();
			if (index == GameControl.control.currentDonutShopSelection) {
				GameControl.control.selectedDonutShopButton =gameObject;
			}
		if (index == GameControl.control.currentDonutShopSelection && GameControl.control.isDonutShopOpen && Input.GetKeyDown (KeyCode.Return)) {
			if ( GameControl.control.totalMoney >= price) {
						GameControl.control.shopCounts[index]+=1;
				GameControl.control.totalMoney -= price;
				price += Mathf.FloorToInt(price / 10);
				GameControl.control.shopPrices [index] = price;

			} /*else if (nameOfItem == "wagon" && GameControl.control.totalMoney >= price) {
				GameControl.control.numOfWagons += 1;
			  GameControl.control.totalMoney -= price;
				price += 10;
			}
			else if (nameOfItem == "mobile" && GameControl.control.totalMoney >= price) {
				GameControl.control.numOfMobiles += 1;
				GameControl.control.totalMoney -= price;
			}*/
		}

		}


}
