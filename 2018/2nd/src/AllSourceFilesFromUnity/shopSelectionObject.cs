using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSelectionObject : MonoBehaviour {
	public int index;
	// Use this for initialization
	void Awake () {
		if (GameControl.control.itemCount == 0) {
			GameControl.control.selectedItem = gameObject;
		}
	}

	// Update is called once per frame
		void Update () {
		if (index == GameControl.control.selectedItemIndex) {
			GameControl.control.selectedItem =gameObject;
		}

	}
}
