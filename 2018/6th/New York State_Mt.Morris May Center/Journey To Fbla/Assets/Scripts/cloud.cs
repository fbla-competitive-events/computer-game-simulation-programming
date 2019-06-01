using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour {
    private Transform myTrans;
    public float speed;

    void Awake()
    {
        myTrans = this.transform;
    }
    void FixedUpdate()
    {
        myTrans.position = new Vector2(myTrans.position.x - speed, myTrans.position.y);
    }

}
