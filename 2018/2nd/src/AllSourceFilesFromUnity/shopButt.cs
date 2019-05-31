using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class shopButt : MonoBehaviour {

	public GameObject shopDisplay;
	public TextMeshProUGUI prompt;
	public GameObject panel;
	public GameObject item;
	public GameObject itemSelectionArrow;
	public GameObject itemContainer;
	// Use this for initialization
	void Start () {
		while(GameControl.control.itemCount<GameControl.control.gameShopItems.Count){
			GameControl.control.shopItemsObject.Add(Instantiate (item, itemContainer.transform));
			GameControl.control.shopItemsObject[GameControl.control.itemCount].GetComponent<shopSelectionObject>().index = GameControl.control.itemCount;
			GameControl.control.itemCount++;
		}
		itemSelectionArrow.transform.SetAsLastSibling ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameControl.control.isShopOpen && Input.GetKeyDown (KeyCode.Return) && !GameControl.control.isShopItselfOpen) {
			shopDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", true);
		}
		if (GameControl.control.isShopOpen && Input.GetKeyUp(KeyCode.Return) && !GameControl.control.isShopItselfOpen) {
			
		
			GameControl.control.isShopItselfOpen = true;

		}
		if (GameControl.control.isShopItselfOpen && Input.GetKeyDown (GameControl.control.eInput)) {
			shopDisplay.GetComponent<Animator> ().SetBool ("isZoomedIn", false);
			GameControl.control.isShopItselfOpen = false;
		}
	}
}
