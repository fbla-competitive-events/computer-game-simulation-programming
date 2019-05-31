using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour {
    protected TextBoxManager t;

   
    // Use this for initialization
    void Start () {
        t = GameObject.Find("Text Box Manager").GetComponent<TextBoxManager>();
        ObjectivePlayerController obj = GameObject.Find("Main Player").GetComponent<ObjectivePlayerController>();
        obj.TogglePointer(true);
        
    }
	//bool 
	// Update is called once per frame
	void Update () {
		
	}
}
