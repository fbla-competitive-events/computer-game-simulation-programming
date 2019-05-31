using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsSetup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        //adding mesh collider to those all objects under a GameObject called 'Buildings'
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.GetComponent<MeshRenderer>())
            {
                child.gameObject.AddComponent<AddMeshCollider>();
            }
        }
	}
}
