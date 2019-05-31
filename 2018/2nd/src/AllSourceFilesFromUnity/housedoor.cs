using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class housedoor : MonoBehaviour {
	public Animator animator;
	public CanvasGroup prompt;
	public GameObject panel;
	void OnTriggerEnter2D()
	{
		animator.SetBool ("isOpen", true);
		prompt.alpha = 1;

	}
	void OnTriggerExit2D()
	{
		animator.SetBool ("isOpen", false);	
		prompt.alpha = 0;


	}

	// Use this for initialization
	void Start () {
		prompt.GetComponent<TextMeshProUGUI> ().text = "Press '" + GameControl.control.eInput + "' to enter";

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) && animator.GetBool("isOpen")) {
			GameControl.control.latestCharPositionOutdoors = PlatformerCharacter2D.control.transform.position;
			GameControl.control.latestCharPositionIndoors = new Vector3 (12f, 0);

			panel.GetComponent<FadeControl>().levelChange("playerhome",panel);

		}
	}
}
