using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeFloor : MonoBehaviour {
	public Vector3 newPosition;
	public GameObject panel;
	public GameObject objectToMove;
	void OnTriggerEnter2D(){
	
		panel.GetComponent<FadeControl> ().floorChange (newPosition,objectToMove);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
