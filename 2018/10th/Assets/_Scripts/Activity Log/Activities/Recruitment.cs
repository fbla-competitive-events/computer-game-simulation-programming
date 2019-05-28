using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recruitment : MonoBehaviour
{
    public Text members;
    int m;

    public void Fund()
    {
        m += Random.Range(0, 2);
        members.GetComponent<Text>().text = "" + m;
    }
}
