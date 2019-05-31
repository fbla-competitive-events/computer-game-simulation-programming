using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine.SceneManagement;
public class StartScript : MonoBehaviour {
	public GameObject panel;
	public Animator menu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!menu.GetBool("isZoomedIn")){
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (File.Exists (Application.persistentDataPath + "/saveinfo.dat")) {
				GameControl.control.Load ();
				panel.GetComponent<FadeControl>().levelChange ("main", panel);
			} else {
				panel.GetComponent<FadeControl>().levelChange ("customize", panel);

			}
		}
			}
	}
}
