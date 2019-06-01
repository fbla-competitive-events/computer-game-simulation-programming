using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catPoints : MonoBehaviour {

	static public int cat_Points = 0;
	public int speechPoints;
	public GameObject catHud;
	public GameObject speech;
	public GameObject speech2;
	//public GameObject speech3;

	void Start(){
		catHud.SetActive (false);
		//speech3.SetActive (false);
		speech2.SetActive (false);
	}
	//void Update(){
		//if (speechPoints == 5) {
			//speech3.SetActive (true);
		//}
	//}
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			speech.SetActive (false);
			speech2.SetActive (true);
			cat_Points = cat_Points + 1;
			catPointManager.speechPoints = catPointManager.speechPoints + 1;
			Destroy(gameObject);
			catHud.SetActive (true);
		}
	}
}
