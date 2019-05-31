using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class changeScene : MonoBehaviour {
	public GameObject panel;
	public string placeToGo;
	public int whichDoorId;
	void OnTriggerEnter2D()
	{if (SceneManager.GetActiveScene ().name == "playerhome" ){//||SceneManager.GetActiveScene ().name == "fblabuilding" ||SceneManager.GetActiveScene ().name == "shop") {
			GameControl.control.latestCharPositionIndoors = new Vector3 (PlatformerCharacter2D.control.transform.position.x - 3, PlatformerCharacter2D.control.transform.position.y);
		}
		panel.GetComponent<FadeControl>().levelChange(placeToGo,panel);

	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
