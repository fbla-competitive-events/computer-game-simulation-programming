using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSplashScreen : MonoBehaviour {

    private Transform myTrans;
    public float speed;
    private bool startMove;
	private bool Stop;

    void Awake()
    {
        myTrans = this.transform;
		Stop = true;

    }
    void Update()
    {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Stop = false;
			startMove = true;
		}
		if (Stop) {
			Time.timeScale = 0;
		}
		if (!Stop) {
			Time.timeScale = 1;
		}
        if (startMove)
        {
            myTrans.position = new Vector2(myTrans.position.x - speed, myTrans.position.y);
        }

    }

}
