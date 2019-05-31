using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArrow : MonoBehaviour {

	// Use this for initialization
	public GameObject layout;
	public float maxY;
	public float minY=-70;
	void Start () {
		maxY =Screen.height;	

	}

	// Update is called once per frame
	void Update () {
		if (GameControl.control.isDonutShopOpen) {

			if (Input.GetKeyDown (GameControl.control.downInput) && GameControl.control.currentDonutShopSelection != 4) {
				GameControl.control.currentDonutShopSelection++;
			/*	if (-transform.localPosition.y > maxY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y += GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;
					layout.transform.localPosition = newPos;
					maxY += GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;
					minY += GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;

				}*/
				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);

			}
			if (Input.GetKeyDown (GameControl.control.upInput) && GameControl.control.currentDonutShopSelection > 0) {
				GameControl.control.currentDonutShopSelection--;
				transform.position = new Vector3 (transform.position.x, GameControl.control.selectedDonutShopButton.transform.position.y);
				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);
				/*if (-transform.localPosition.y < minY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y -= GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;
					layout.transform.localPosition = newPos;
					minY -= GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;
					maxY-=GameControl.control.selectedDonutShopButton.GetComponent<RectTransform> ().rect.height;
				}*/
				//	if (!screenRect.Contains (gameObject))
			}
			if (GameControl.control.selectedDonutShopButton != null)
				transform.position = new Vector3 (GameControl.control.selectedDonutShopButton.transform.localPosition.x + 5, GameControl.control.selectedDonutShopButton.transform.position.y);

		}
	}
}
