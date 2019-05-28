using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit ();
		}
	}
}
