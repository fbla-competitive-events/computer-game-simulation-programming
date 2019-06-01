using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour {

    //public int targetWidth = 400;
    //public int targetheight = 240;
    public float pixelsToUnits = 100;
   // public int height = 240;
    public float aspectHeight = 240f;
    public float aspectWidth = 400f;

	void Update () {
        //int height = 240; //Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
        //int width = 400;
        GetComponent<Camera>().aspect = aspectWidth / aspectHeight;
        GetComponent<Camera>().orthographicSize = aspectHeight / pixelsToUnits / 2;
        
        //GetComponent<Camera>().orthographicSize = width / pixelsToUnits / 2;

        //int width = Mathf.RoundToInt(targetheight / (float)Screen.height * Screen.width);

        //GetComponent<Camera>().orthographicSize = width / pixelsToUnits / 2;
    }
}
