using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopdoor : MonoBehaviour {


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

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput) && animator.GetBool("isOpen")) {
			GameControl.control.latestCharPositionOutdoors = PlatformerCharacter2D.control.transform.position;
			GameControl.control.latestCharPositionIndoors = new Vector3 (12f, 0);

			panel.GetComponent<FadeControl>().levelChange("shop",panel);

		}
	}
}
