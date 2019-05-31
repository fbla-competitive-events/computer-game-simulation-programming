using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class score : MonoBehaviour {
	public TextMeshProUGUI text;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		text.text = gameStats.gameScore.ToString();
}
}
