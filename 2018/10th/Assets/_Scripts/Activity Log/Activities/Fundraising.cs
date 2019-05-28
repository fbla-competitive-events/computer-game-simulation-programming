using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fundraising : MonoBehaviour
{
    public Text money;
    int Cash;

    public void Fund()
    {
        Cash += Random.Range(5, 100);
        money.GetComponent<Text>().text = "$" + Cash;
    }
}
