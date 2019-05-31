using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemName : MonoBehaviour {

	public TextMeshProUGUI text;
	// Use this for initialization
	void Awake () {
		text.text = GameControl.control.gameShopItems[GameControl.control.itemCount];
	}
}
