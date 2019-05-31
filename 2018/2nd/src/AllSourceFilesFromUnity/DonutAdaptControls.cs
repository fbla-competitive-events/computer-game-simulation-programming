using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DonutAdaptControls : MonoBehaviour {
	public TextMeshProUGUI toggleS;
	public TextMeshProUGUI leave;

	// Use this for initialization
	void Start () {
		leave.text = "Press '" + GameControl.control.backInput + "' to return to the FBLA building";
		toggleS.text="Press '" + GameControl.control.eInput + "' to toggle shop";

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
