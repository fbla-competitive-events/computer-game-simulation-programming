using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPosition : MonoBehaviour
{
    public Text spot;
    public string[] positions = new string[] { "President", "Vice President", "Secretary", "Treasurer", "Committee Leader", "Member"};
    int position;
    string Spot;

    public void Fund()
    {
        position = Random.Range(1, 7);
        if (position == 1)
        {
            Spot = "President";
        }
        if (position == 2)
        {
            Spot = "Vice President";
        }
        if (position == 3)
        {
            Spot = "Secretary";
        }
        if (position == 4)
        {
            Spot = "Treasurer";
        }
        if (position == 5)
        {
            Spot = "Committee Leader";
        }
        if (position == 6)
        {
            Spot = "Member";
        }

        spot.GetComponent<Text>().text = "" + Spot;
    }
}
