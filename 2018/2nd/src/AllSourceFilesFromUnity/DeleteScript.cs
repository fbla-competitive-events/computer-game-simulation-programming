using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class DeleteScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		File.Delete (Application.persistentDataPath + "/saveinfo.dat");
		Application.Quit ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
