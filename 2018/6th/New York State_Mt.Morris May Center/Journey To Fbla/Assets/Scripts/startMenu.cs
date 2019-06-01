using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour {
	//First menu screen
	public GameObject menu1;
	//Second menu screen
	public GameObject menu2;
	//First option on the menu screen
	public GameObject pt1;
	//Second option on the menu screen
	public GameObject pt2;
	//Third option on the menu screen
	public GameObject pt3;
	//Last option on the menu screen
	//public GameObject pt4;
	//Boolean variable to determine if second menu has been open
	private bool secondMenu;
	//interger count to keep track of which menu option you are on
	private int menuOpt;

	void Awake(){
		//First menu screen
		menu1.SetActive (true);
		//Second menu screen
		menu2.SetActive (false);
		//First option on the menu screen
		pt1.SetActive (false);
		//Second option on the menu screen
		pt2.SetActive (false);
		//Third option on the menu screen
		pt3.SetActive (false);
		//Last option on the menu screen
		//pt4.SetActive (false);
		//Boolean variable to determine if second menu has been open
		secondMenu = false;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Return) && menu1.activeInHierarchy) {
			StartCoroutine (menu ());
		}
		if (secondMenu) {
			menu1.SetActive (false);
			menu2.SetActive (true);
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				menuOpt = menuOpt + 1;
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				menuOpt = menuOpt - 1;
			}
			if (menuOpt > 2) {
				menuOpt = 2;
			}
			if (menuOpt < 0) {
				menuOpt = 0;
			}
			if (menuOpt == 0) {
				pt1.SetActive (true);
				pt2.SetActive (false);
				pt3.SetActive (false);
				//pt4.SetActive (false);
				if (Input.GetKeyDown (KeyCode.Return)) {
					SceneManager.LoadScene ("rules");
				}
			}
			if (menuOpt == 1) {
				pt1.SetActive (false);
				pt2.SetActive (true);
				pt3.SetActive (false);
				if (Input.GetKeyDown (KeyCode.Return)) {
					SceneManager.LoadScene ("Controls");
				}
			}
			if (menuOpt == 2) {
				pt1.SetActive (false);
				pt2.SetActive (false);
				pt3.SetActive (true);
				//pt4.SetActive (false);
				if (Input.GetKey(KeyCode.Return))
				{
					Application.Quit();
				}
			}
			/*if (menuOpt == 3) {
				pt1.SetActive (false);
				pt2.SetActive (false);
				pt3.SetActive (false);
				pt4.SetActive (true);
			}*/
		}
	}

	IEnumerator menu(){
		
		yield return new WaitForSeconds (1);
		secondMenu = true;
	}
}
