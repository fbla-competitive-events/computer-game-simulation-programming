using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedScript : MonoBehaviour {
    public bool pressed = false;
    public bool failed = false;
    private bool failedOnce = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (pressed && failed && !failedOnce)
        {
            Debug.Log("Failure");
            failedOnce = true;
        }
	}

   
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Keys")
        {
            failed = true;
            failedOnce = false;
        }
        if (pressed)
        {
            if (other.tag == "Keys")
            {
                failed = false;
                Destroy(other.gameObject);
                //mathy stuff here
                //like how far into the button has it gone? add a % of that distance
                var heading = other.transform.position - GetComponent<Transform>().position;
                heading.y = 0;
                RythmGameController.Score += 1;
                //Debug.Log(heading);
                /*
                if (heading.z >= 0.2)
                {
                    RythmGameController.Score += 1;
                    Debug.Log("A");
                }
                else if (heading.z >= 0)
                {
                    RythmGameController.Score += 1;
                    Debug.Log("B");
                }
                else
                {
                    Debug.Log("Failed!");
                }*/
            }
        }
    }

    

    public void OnPressed()
    {
        pressed = true;
    }

    public void OffPressed()
    {
        pressed = false;
    }
}
