using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paraScroll : MonoBehaviour {

    public GameObject scrollCamera;
    private Transform myTrans;
    public float speed;
    private float oldVal;

    void Awake()   
    {
        myTrans = this.transform;
    }
    void Update()
    {
        oldVal = scrollCamera.transform.position.x;
    }
    void FixedUpdate(){
        if (scrollCamera.transform.position.x < oldVal)
        {
            myTrans.position = new Vector2(myTrans.position.x + speed, myTrans.position.y);
        }
        if (scrollCamera.transform.position.x > oldVal)
        {
            myTrans.position = new Vector2(myTrans.position.x - speed, myTrans.position.y);
        }
    }

}
