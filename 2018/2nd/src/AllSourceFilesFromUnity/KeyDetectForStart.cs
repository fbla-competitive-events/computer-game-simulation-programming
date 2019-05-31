using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KeyDetectForStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMeshProUGUI> ().text = "Press '" + GameControl.control.eInput + "' to view control settings";
	}
}
