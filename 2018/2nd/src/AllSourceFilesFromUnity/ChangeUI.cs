using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ChangeUI : MonoBehaviour {
	public Animator animator;
	public bool isOpen;
	public Slider main;
	public EventSystem es;
	// Use this for initialization
	void Start () {

	}
	public void OnPointerClick(PointerEventData eventData) // 3
	{
		Debug.Log ("GO");
		es.SetSelectedGameObject (main.gameObject);
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2)) {
			Debug.Log ("GO");
			es.SetSelectedGameObject (main.gameObject);
		}
		if (Input.GetKeyDown (GameControl.control.eInput)) {
			if (isOpen) {

				animator.SetBool ("isZoomedIn", false);
				isOpen = false;
			}
			else if(!isOpen)
			{
				animator.SetBool("isZoomedIn", true);
				isOpen = true;
			}
		}

	}
}
