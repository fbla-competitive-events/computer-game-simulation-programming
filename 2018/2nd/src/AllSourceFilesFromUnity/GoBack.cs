using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoBack : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start () {
		text.text = "Press '" + GameControl.control.backInput + "' to go back";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
