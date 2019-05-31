using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
The following script hanlds ranking for competuive events. In the game top 3 is placing therefore only rank 1-4 is neccesary 
to evaluate. All the scrip does is detect rank of submitted game compares to 3 randomly generated values and places the game accordingly.
Then, the text of the award panel is set to refelect that rank
*/
public class results : MonoBehaviour {
	public TextMeshProUGUI text;
	public int rank;
	bool isTextAssigned=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (GameControl.control.eInput)) {
		
			Destroy (gameObject);
		}
		if (!isTextAssigned) {
			if (rank == 4) {
				text.text = "Sorry, you didn't win anything!";
				isTextAssigned = true;


			} 
			else{
				if (!GameControl.control.placingMedal) {
					GameControl.control.placingMedal = true;
					GameControl.control.awardPointCount += 1;
					GameControl.control.checkForGoalPoints();
					GameControl.control.awardUnlocked ();

				}
				if (rank == 1) {
				text.text = "Congratulations, you got 1st!";
				isTextAssigned = true;
				if(!GameControl.control.placeFirst)
						GameControl.control.placeFirst = true;{
						GameControl.control.awardPointCount += 2;
						GameControl.control.checkForGoalPoints();
						GameControl.control.awardUnlocked ();

					}

			} else if (rank == 2) {
				text.text = "Congratulations, you got 2nd!";
				isTextAssigned = true;

			} else if (rank == 3) {
				text.text = "Congratulations, you got 3rd!";
				isTextAssigned = true;
			}
			
			}
		}
	}
}
