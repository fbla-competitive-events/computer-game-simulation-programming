using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meetings : MonoBehaviour
{
    public Text meet;
    int numMeet;

    public void Fund()
    {
        numMeet++;
        meet.GetComponent<Text>().text = "" + numMeet;
    }
}
