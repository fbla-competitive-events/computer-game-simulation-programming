using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DonutsPerSecond : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "$" +GameControl.control.donutsPerSecond.ToString("F2")+" per second";
	}
}
