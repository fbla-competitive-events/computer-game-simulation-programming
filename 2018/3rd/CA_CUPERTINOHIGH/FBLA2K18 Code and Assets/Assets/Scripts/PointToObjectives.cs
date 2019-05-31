using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToObjectives : MonoBehaviour {
    public GameObject[] gameObj;
    public GameObject pointer;
    public GameObject pointerParent;
    public float rangeOfInBoundsGameObject = 5f;
	// Use this for initialization
	void Start () {
        gameObj = GameObject.FindGameObjectsWithTag("Objective");
        pointer = GameObject.FindGameObjectWithTag("PointToTarget");
        pointerParent = GameObject.FindGameObjectWithTag("PointerParent");
    }
	
	// Update is called once per frame
	void Update () {
        //there should only be one objective active during the game
        if (gameObj.Length == 0) pointerParent.SetActive(false);
        else pointerParent.SetActive(true);
		foreach(GameObject g in gameObj)
        {
            Transform target = g.GetComponent<Transform>();
    
            Vector3 targetDir = target.position - transform.position;
            float step = 50 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step , 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);

            //Camera miniCam = GameObject.Find("MiniMap Camera").GetComponent<Camera>();
            //Vector3 screenPoint = miniCam.WorldToViewportPoint(target.position);
            //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            //Debug.Log(onScreen);

            var heading = target.position - pointerParent.GetComponent<Transform>().position;
            heading.y = 0;

            if (heading.sqrMagnitude < rangeOfInBoundsGameObject*rangeOfInBoundsGameObject)
            {
                pointer.SetActive(false);
            } else
            {
                pointer.SetActive(true);
            }
        }

        

    }
}
