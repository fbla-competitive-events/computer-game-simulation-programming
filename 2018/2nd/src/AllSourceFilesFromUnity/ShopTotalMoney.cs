using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopTotalMoney : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Your Money: "+ string.Format("{0:$#,##0;($#,##0);$0}",Mathf.Floor(GameControl.control.totalMoney));

	}
}
