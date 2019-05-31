using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startFromCustom : MonoBehaviour {
	public GameObject panel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void saveLoad(){
		GameControl.control.SaveLoad ();
	}
	public void startGame()
	{
		panel.GetComponent<FadeControl>().levelChange ("main", panel);
	}
	public void changeHairC(float x)
	{
		GameControl.control.changeHairColor (x);		
	}
}
