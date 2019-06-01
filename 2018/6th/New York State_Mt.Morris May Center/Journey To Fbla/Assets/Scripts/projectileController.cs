using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour {

    public float blastSpeed;
    public float aliveTime;

    private Rigidbody2D rigidBody;

    void Awake () {
        rigidBody = GetComponent<Rigidbody2D>();
        if(transform.localRotation.z < 0)
        {
            rigidBody.AddForce(new Vector2(-1, 0) * blastSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rigidBody.AddForce(new Vector2(1, 0) * blastSpeed, ForceMode2D.Impulse);
        }
        
        Destroy(gameObject, aliveTime);
    }
	
	
	void Update () {
		
	}

    public void removeForce()
    {
        rigidBody.velocity = new Vector2(0, 0);
    }



}
