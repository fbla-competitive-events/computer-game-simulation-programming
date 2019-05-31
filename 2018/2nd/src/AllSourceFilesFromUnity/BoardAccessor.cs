using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoardAccessor : MonoBehaviour {
	public CanvasGroup text;
	public bool inRange;
	public GameObject panel;
	// Use this for initialization
	void OnTriggerEnter2D()
	{
		text.alpha= 1;
		inRange = true;
	}
	void OnTriggerExit2D()
	{
		text.alpha = 0;
		inRange = false;
	}
	void Awake()
	{
		text.GetComponent<TextMeshProUGUI> ().text = "Press '" + GameControl.control.eInput + "' to view leaderboard";
	}
	void Update()
	{
		if(Input.GetKeyDown(GameControl.control.eInput) &&inRange)
			{
			//GameControl.control.latestCharPositionIndoors;
			panel.GetComponent<FadeControl> ().levelChange ("LeaderBoard", panel);
			}
	}
}
