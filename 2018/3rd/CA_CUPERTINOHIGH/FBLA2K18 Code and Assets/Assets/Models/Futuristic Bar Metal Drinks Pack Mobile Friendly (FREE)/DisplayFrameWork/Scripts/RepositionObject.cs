using UnityEngine;
using System.Collections;

public class RepositionObject : MonoBehaviour
{
    public GameObject[] topBottom = new GameObject[2];

    public GameObject item;

    [Range(0,1)]
    public float Ypos;
    [Range(-1, 1)]
    public float Xpos;

    public float currYPos;
    public float distBetweenTopBottom;
    public float normalised;

    void Start () {
        item.transform.position = Vector3.Lerp(topBottom[1].transform.position,
                                                topBottom[0].transform.position,
                                                Ypos);
    }
}
