using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour {

	//where the scene name will be placed
	public string OriginalArea;

	void Update(){
		if (Input.GetKeyDown (KeyCode.M)) {
			//if m is clicked go to the title screen
			SceneManager.LoadScene("titleScreen");
		}
		if (Input.GetKeyDown (KeyCode.Return)) {
			//if Enter/Return pressed go to scene you were previoulsy at
			SceneManager.LoadScene(OriginalArea);
		}
	}
}
