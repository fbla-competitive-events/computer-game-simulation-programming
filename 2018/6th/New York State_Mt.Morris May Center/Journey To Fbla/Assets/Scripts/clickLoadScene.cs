using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clickLoadScene : MonoBehaviour {

	public string nxtScene;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene (nxtScene);
		}
	}
}
