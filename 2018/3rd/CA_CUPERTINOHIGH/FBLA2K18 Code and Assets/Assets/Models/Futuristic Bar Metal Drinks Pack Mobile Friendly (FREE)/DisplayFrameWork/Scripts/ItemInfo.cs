using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInfo : MonoBehaviour
{
    public Text txtItemName;
    public Text txtPolyCount;

    void Start () {
        
    }
	
	// Update is called once per frame
	/*void Update () {
	
	}*/

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Beer")
        {
            DisplayInfo(col.gameObject, "Beer Glass");
        }
        if (col.gameObject.tag == "BeerTankard")
        {
            DisplayInfo(col.gameObject, "Beer Tankard");
        }
        if (col.gameObject.tag == "Wine")
        {
            DisplayInfo(col.gameObject, "Wine Glass");
        }
        if (col.gameObject.tag == "Whiskey")
        {
            DisplayInfo(col.gameObject, "Whiskey Glass");
        }
        if (col.gameObject.tag == "Martini")
        {
            DisplayInfo(col.gameObject, "Martini Glass");
        }
        if (col.gameObject.tag == "Absinthe")
        {
            DisplayInfo(col.gameObject, "Absinthe Dispender");
        }
        if (col.gameObject.tag == "TiaMaria")
        {
            DisplayInfo(col.gameObject, "TiaMaria Dispender");
        }
        if (col.gameObject.tag == "CafeLiqueur")
        {
            DisplayInfo(col.gameObject, "CafeLiqueur Dispender");
        }
        if (col.gameObject.tag == "Tequila")
        {
            DisplayInfo(col.gameObject, "Tequila Dispender");
        }
        if (col.gameObject.tag == "Vermouth")
        {
            DisplayInfo(col.gameObject, "Vermouth Dispender");
        }
    }

    void DisplayInfo(GameObject go, string s)
    {
        int polygons = (go.GetComponent<MeshFilter>().mesh.triangles.Length / 3) / 2;
        //int polygons = go.GetComponent<MeshFilter>().mesh.triangles.Length / 6;
        //gameObject.GetComponent<MeshFilter>().mesh.triangles.Length / 3;

        txtItemName.text = s;
        txtPolyCount.text = "Polygons: " + polygons;

    }
}
