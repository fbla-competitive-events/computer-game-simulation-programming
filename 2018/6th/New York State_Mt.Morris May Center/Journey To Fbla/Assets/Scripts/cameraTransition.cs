using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CameraControl cameraScript = Camera.main.GetComponent<CameraControl>();
		Vector3 target = new Vector3 (1, 1, 0);
		cameraScript.MoveCamera( target, 5f);
		cameraScript.ActivateLimits( -40, 40, -40, 40 );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
