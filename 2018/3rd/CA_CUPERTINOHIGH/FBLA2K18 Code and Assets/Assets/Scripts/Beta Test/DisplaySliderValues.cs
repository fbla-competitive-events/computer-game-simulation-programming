using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplaySliderValues : MonoBehaviour {
    public Slider slider; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Text t = GetComponent<Text>();
        t.text = "" + slider.value;
	}
}
