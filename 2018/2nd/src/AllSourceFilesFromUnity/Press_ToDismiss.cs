using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Press_ToDismiss : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start () {
		text.text = "Press '" + GameControl.control.eInput + "' to dismiss";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
