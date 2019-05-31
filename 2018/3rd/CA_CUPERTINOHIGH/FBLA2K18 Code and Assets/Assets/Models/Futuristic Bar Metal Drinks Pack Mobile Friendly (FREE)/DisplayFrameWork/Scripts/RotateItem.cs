using UnityEngine;
using System.Collections;

public class RotateItem : MonoBehaviour
{
    GameObject currentGO;

    public float rotateSpeed = 0.5f;

    /*void Start () {
	
	}*/

    void Update ()
    {
        currentGO.transform.Rotate(0, 0, rotateSpeed * Time.smoothDeltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Beer")
        {
            currentGO = col.gameObject;
            //DisplayInfo(col.gameObject, "Beer Glass");
        }
        if (col.gameObject.tag == "BeerTankard")
        {
            currentGO = col.gameObject;
            //DisplayInfo(col.gameObject, "Beer Tankard");
        }
        if (col.gameObject.tag == "Wine")
        {
            currentGO = col.gameObject;
            //DisplayInfo(col.gameObject, "Wine Glass");
        }
        if (col.gameObject.tag == "Whiskey")
        {
            currentGO = col.gameObject;
            //DisplayInfo(col.gameObject, "Whiskey Glass");
        }
        if (col.gameObject.tag == "Martini")
        {
            currentGO = col.gameObject;
            //DisplayInfo(col.gameObject, "Martini Glass");
        }
    }
}
