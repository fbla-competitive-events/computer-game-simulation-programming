using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionSelectArrow : MonoBehaviour {
	public float spacing=-360;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GameControl.control.isPCOpen && !GameControl.control.isClearBugsClicked) {
			transform.localPosition = new Vector3 (spacing, transform.localPosition.y);
				if (Input.GetKeyDown (GameControl.control.leftInput)&&GameControl.control.currentGameButtonSelectionIndex!=0) {
				GameControl.control.currentGameButtonSelectionIndex--;
				spacing = -360;
			}
			if (Input.GetKeyDown (GameControl.control.rightInput) &&GameControl.control.currentGameButtonSelectionIndex!=1) {
				GameControl.control.currentGameButtonSelectionIndex++;
				spacing = 60;

			}
		
			}

		
	}
}
