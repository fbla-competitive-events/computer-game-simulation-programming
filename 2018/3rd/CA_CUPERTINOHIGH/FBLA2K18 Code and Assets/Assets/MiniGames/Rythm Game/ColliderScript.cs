using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Missed");
        RythmGameController.MissedBeats++;

        Destroy(other.gameObject);
    }

}
