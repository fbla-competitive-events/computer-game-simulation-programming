using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class itemPrice : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Awake()
	{
		text.text = string.Format("{0:$#,##0;($#,##0);$0}",GameControl.control.shopItemsCosts[GameControl.control.itemCount]);
	}
}
