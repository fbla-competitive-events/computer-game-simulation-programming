using UnityEngine;
using System.Collections;

public class lockFrameRate : MonoBehaviour {

	//Target FrameRate
    public int target = 60;


	void Start () {
		//turns vSync off
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
	
	}
	

	void Update () {


        if (target != Application.targetFrameRate)
        {
			//makes frame rate stay arund our preferred frame rate keeps game running smooth
            Application.targetFrameRate = target;
        }
        quit();


    }


    void quit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
