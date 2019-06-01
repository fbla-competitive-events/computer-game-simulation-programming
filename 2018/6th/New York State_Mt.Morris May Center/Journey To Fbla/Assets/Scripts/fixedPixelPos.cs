using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedPixelPos : MonoBehaviour {

	public float pixPerUnit = 16;
	void FixedUpdate () {
		transform.position = new Vector3 (
			Mathf.Round (transform.position.x * pixPerUnit) / pixPerUnit,
			Mathf.Round (transform.position.y * pixPerUnit) / pixPerUnit,
			transform.position.z);
		
	}
}
