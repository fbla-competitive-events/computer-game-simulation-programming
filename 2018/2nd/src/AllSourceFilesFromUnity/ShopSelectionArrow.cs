using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSelectionArrow : MonoBehaviour {

	// Use this for initialization
	public GameObject layout;
	public GameObject gameActionBar;
	public float maxY;
	public float minY=-70;
	void Start () {
		maxY = -gameActionBar.transform.localPosition.y;
		//if (GameControl.control.selectedItem != null)
	}

	// Update is called once per frame
	void Update () {
		if (GameControl.control.isShopItselfOpen) {

			if (Input.GetKeyDown (GameControl.control.downInput) && GameControl.control.selectedItemIndex < GameControl.control.gameShopItems.Count-1) {
				GameControl.control.selectedItemIndex++;
				/*if (-transform.localPosition.y > maxY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y += GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;
					layout.transform.localPosition = newPos;
					maxY += GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;
					minY += GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;

				}*/
				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);

			}
			if (Input.GetKeyDown (GameControl.control.upInput) && GameControl.control.selectedItemIndex> 0) {
				GameControl.control.selectedItemIndex--;

				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);
			/*	if (-transform.localPosition.y < minY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y -= GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;
					layout.transform.localPosition = newPos;
					minY -= GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;
					maxY -= GameControl.control.selectedItem.GetComponent<RectTransform> ().rect.height;

				}*/
				//	if (!screenRect.Contains (gameObject))
			}
			if (GameControl.control.selectedItem != null)
				transform.position = new Vector3 (13, GameControl.control.selectedItem.transform.position.y);
			else {
				GetComponent<CanvasGroup> ().alpha = 0;
			}
		}
	}
}
