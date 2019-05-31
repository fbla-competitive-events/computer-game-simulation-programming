using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class HelpScript : MonoBehaviour {
	public Animator animator;
	public TextMeshProUGUI[] texts;
	public TextMeshProUGUI header;
	// Use this for initialization
	void Start () {
		if (GameControl.control.hasMainHelpMenuBeenSeen && SceneManager.GetActiveScene ().name == "main") {
			animator.SetBool ("isZoomedIn", false);
		} else {
			animator.SetBool ("isZoomedIn", true);

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.GetBool ("isZoomedIn") && (Input.GetKeyDown(GameControl.control.eInput)|| Input.GetKeyDown(KeyCode.H))) {
			if (SceneManager.GetActiveScene ().name == "main") {
				GameControl.control.hasMainHelpMenuBeenSeen = true;
			}

			animator.SetBool ("isZoomedIn", false);
		}
		else if (!animator.GetBool ("isZoomedIn") && (Input.GetKeyDown(KeyCode.H))) {
			animator.SetBool ("isZoomedIn", true);
		}
		else if(animator.GetBool ("isZoomedIn") && Input.GetKeyDown(KeyCode.Space)&& SceneManager.GetActiveScene().name=="bugcheck"){
			animator.SetBool ("isZoomedIn", false);

		}
	}
}
