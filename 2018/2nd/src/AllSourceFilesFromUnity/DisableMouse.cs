using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DisableMouse : MonoBehaviour {
	public EventSystem es;
	public Slider slider;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2)) {
			es.SetSelectedGameObject (slider.gameObject);
		}
	}
}
