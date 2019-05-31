using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DonutScore : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		text.text= string.Format("{0:$#,##0;($#,##0);$0}",Mathf.Floor(GameControl.control.totalMoney));
	}
}
