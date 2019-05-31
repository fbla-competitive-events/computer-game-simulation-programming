using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActionSelectArrow : MonoBehaviour {

	// Use this for initialization
	public float spacing=-360;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (GameControl.control.isDialogOpen ) {
			transform.localPosition = new Vector3 (spacing, transform.localPosition.y);
			if (Input.GetKeyDown (GameControl.control.leftInput)&&GameControl.control.currentDialogButtonSelectionIndex!=0) {
				GameControl.control.currentDialogButtonSelectionIndex--;
				spacing = -360;
			}
			if (Input.GetKeyDown (GameControl.control.rightInput) &&GameControl.control.currentDialogButtonSelectionIndex!=1) {
				GameControl.control.currentDialogButtonSelectionIndex++;
				spacing = 60;

			}

		}


	}
}
