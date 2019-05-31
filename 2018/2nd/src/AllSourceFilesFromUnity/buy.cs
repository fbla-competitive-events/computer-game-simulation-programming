using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameControl.control.isShopItselfOpen && GameControl.control.selectedItem!=null && Input.GetKeyDown(KeyCode.Return))
		{
			if(GameControl.control.totalMoney>=GameControl.control.shopItemsCosts[GameControl.control.selectedItemIndex])
			{
				if (!GameControl.control.buyWord) {
					GameControl.control.buyWord = true;
					GameControl.control.awardPointCount += 1;
					GameControl.control.awardUnlocked ();
				}
				GameControl.control.totalMoney-=GameControl.control.shopItemsCosts[GameControl.control.selectedItemIndex];
				GameControl.control.unlockedWordList.Add (GameControl.control.gameShopItems [GameControl.control.selectedItemIndex]);
				GameControl.control.gameShopItems.RemoveAt(GameControl.control.selectedItemIndex);
				GameControl.control.shopItemsCosts.RemoveAt(GameControl.control.selectedItemIndex);
				GameControl.control.shopItemsObject.RemoveAt(GameControl.control.selectedItemIndex);

				Destroy(GameControl.control.selectedItem);
				for (int x = GameControl.control.selectedItemIndex; x < GameControl.control.shopItemsObject.Count; x++) {
					GameControl.control.shopItemsObject [x].GetComponent<shopSelectionObject> ().index -= 1;
				}

			}

		}	
	}
}
