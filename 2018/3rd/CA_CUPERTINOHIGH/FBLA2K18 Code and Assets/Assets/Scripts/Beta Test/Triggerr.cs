using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerr : MonoBehaviour {
    private bool trig = false;
    // Use this for initialization
    public GameObject enemy;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (trig)
        {
            enemy.GetComponent<EnemyPursue>().enabled = true;
            trig = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        trig = true;
    }
}
