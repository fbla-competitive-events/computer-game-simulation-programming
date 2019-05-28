using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunityServiceHours : MonoBehaviour
{

    public Text Hours;
    int time;

    public void Fund()
    {
        time += Random.Range(2, 14);
        Hours.GetComponent<Text>().text = "" + time;
    }
}
