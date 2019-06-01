using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introController : MonoBehaviour {
	//public GameObject harves;
	public Animator anim;
	//3 days
	public GameObject step1;
	//meter 1
	public GameObject step2;
	//meter 2
	public GameObject step3;
	//hand1
	public GameObject step4;
	//hand2
	public GameObject step5;
	//student and cash symbol
	public GameObject step6;
	//graffetti
	public GameObject step7;
	//trash
	public GameObject step8;
	//posters
	public GameObject step9;
	//pins
	public GameObject step10;
	//packages
	public GameObject step11;
	//5 mins
	public GameObject step12;
	//controls
	public GameObject step13;

	public GameObject boundary;

	private int steps = 0;

	void Awake () {
		step1.SetActive (false);

		step2.SetActive (false);

		step3.SetActive (false);

		step4.SetActive (false);

		step5.SetActive (false);

		step6.SetActive (false);

		step7.SetActive (false);

		step8.SetActive (false);

		step9.SetActive (false);

		step10.SetActive (false);

		step11.SetActive (false);

		step12.SetActive (false);

		step13.SetActive (false);

		boundary.SetActive (false);

	}


	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			steps = steps + 1;
		}
		if (steps == 0) {
			anim.SetBool ("talking", false);
		}
		if (steps == 1) {
			anim.SetBool ("talking", true);
		}
		if (steps == 2) {
			anim.SetBool ("talking", true);
			step1.SetActive (true);
		}
		if (steps == 3) {
			step1.SetActive (false);
			step2.SetActive (true);
			step3.SetActive (true);
			step4.SetActive (true);
			step5.SetActive (true);

			
		}
		if (steps == 4) {
			step1.SetActive (false);
			step2.SetActive (true);
			step3.SetActive (false);
			step4.SetActive (true);
			step5.SetActive (false);


		}
		if (steps == 5) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (true);
			step4.SetActive (false);
			step5.SetActive (true);


		}
		if (steps == 6) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (true);

		}
		if (steps == 7) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (true);
		}
		if (steps == 8) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (true);
			step8.SetActive (true);
		}
		if (steps == 9) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (true);
			step8.SetActive (true);
			step9.SetActive (true);
		}
		if (steps == 10) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (true);
			step8.SetActive (true);
			step9.SetActive (true);
			step10.SetActive (true);
		}
		if (steps == 11) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (true);
			step8.SetActive (true);
			step9.SetActive (true);
			step10.SetActive (true);
			step11.SetActive (true);
		}
		if (steps == 12) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (false);
			step8.SetActive (false);
			step9.SetActive (false);
			step10.SetActive (false);
			step11.SetActive (false);
			step12.SetActive (true);
		}
		if (steps == 13) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (false);
			step8.SetActive (false);
			step9.SetActive (false);
			step10.SetActive (false);
			step11.SetActive (false);
			step12.SetActive (false);
			boundary.SetActive (true);
		}
		if (steps == 14) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (false);
			step8.SetActive (false);
			step9.SetActive (false);
			step10.SetActive (false);
			step11.SetActive (false);
			step12.SetActive (false);
			step13.SetActive (true);
			boundary.SetActive (false);
		}
		if (steps == 15) {
			step1.SetActive (false);
			step2.SetActive (false);
			step3.SetActive (false);
			step4.SetActive (false);
			step5.SetActive (false);
			step6.SetActive (false);
			step7.SetActive (false);
			step8.SetActive (false);
			step9.SetActive (false);
			step10.SetActive (false);
			step11.SetActive (false);
			step12.SetActive (false);
			step13.SetActive (false);
		}
		if (steps == 16) {
			anim.SetBool ("talking", false);

		}
		if (steps == 17) {
			anim.SetBool ("talking", false);
			SceneManager.LoadScene ("mainArea");
		}



	}
}
