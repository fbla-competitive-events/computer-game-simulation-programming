using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectleHit : MonoBehaviour {

    //public float weaponDamage;

    projectileController myPC;

    //public GameObject explosionEffect;
	
	void Awake () {
        myPC = GetComponentInParent<projectileController>();
	}
	
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
            myPC.removeForce();
            //Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
            myPC.removeForce();
            //Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        
    }



}
