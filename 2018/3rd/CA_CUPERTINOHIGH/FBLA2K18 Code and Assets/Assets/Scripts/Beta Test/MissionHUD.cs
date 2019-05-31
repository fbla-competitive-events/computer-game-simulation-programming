using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MissionHUD : MonoBehaviour {
    public TextMeshProUGUI MissionInfo;
    public GameObject CurrentMission;
    // Use this for initialization
    void Start () {
        MissionInfo.SetText("No Mission Yet!");
	}

    // Update is called once per frame
    bool collectedReward = false;

    void Update () {
        if (CurrentMission.GetComponent<CurrentMissionObject>().finishObjective)
        {
            MissionInfo.fontSize = 10;
            MissionInfo.color = Color.red;
            MissionInfo.text = "[TAB] to collect Reward!";
            collectedReward = true;
        } else if (CurrentMission.GetComponent<CurrentMissionObject>().MissionActive)
        {
            MissionInfo.fontSize = 15;
            MissionInfo.color = Color.black;
            
            MissionInfo.text = (CurrentMission.GetComponent<CurrentMissionObject>().MissionNTExt);
        } else 
        {
            collectedReward = false;
            MissionInfo.fontSize = 15;
            MissionInfo.color = Color.black;
            MissionInfo.SetText("No Mission Yet!");
        }
	}
}
