using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionArrow : MonoBehaviour {
	public GameObject layout;
	public RectTransform mask;
	public GameObject gameActionBar;
	public float maxY;
	public float minY=0;
	void Start () {
		maxY =- gameActionBar.transform.localPosition.y;	

	}

	// Update is called once per frame
	void Update () {
		if (GameControl.control.isPCOpen && GameControl.control.isClearBugsClicked) {
			
			if (Input.GetKeyDown (GameControl.control.downInput) && GameControl.control.allGames.Count>0&& GameControl.control.currentGameSelectionIndex!=GameControl.control.allGames.Count-1) {
				GameControl.control.currentGameSelectionIndex++;

				//Debug.Log(	IsPointInRT (transform.localPosition, boundingBox));

				/*if (-transform.localPosition.y > maxY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y += GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height;
					layout.transform.localPosition = newPos;
					maxY += GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height;
				}*/
				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);

				}
			if (Input.GetKeyDown (GameControl.control.upInput)&&  GameControl.control.allGames.Count>0&& GameControl.control.currentGameSelectionIndex!=0) {
				GameControl.control.currentGameSelectionIndex--;		
				transform.position = new Vector3 (transform.position.x, GameControl.control.selectedGame.transform.position.y);
			
				//transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);
			/*	if (-transform.localPosition.y < maxY) {
					Vector3 newPos = layout.transform.localPosition;
					newPos.y -= GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height;
					layout.transform.localPosition = newPos;
					maxY -= GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height;
				}*/
			//	if (!screenRect.Contains (gameObject))
			}
			if (transform.localPosition.y <= -172) {
				Vector3 newPos = layout.transform.localPosition;
				newPos.y += GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height+6;
				layout.transform.localPosition = newPos;
			}
			if (transform.localPosition.y >= 172) {
				Vector3 newPos = layout.transform.localPosition;
				newPos.y -= GameControl.control.selectedGame.GetComponent<RectTransform>().rect.height+6;
				layout.transform.localPosition = newPos;
			}
			if(GameControl.control.selectedGame!=null)
			transform.position = new Vector3 (GameControl.control.selectedGame.transform.position.x-5, GameControl.control.selectedGame.transform.position.y);
			
		}
	}
}
