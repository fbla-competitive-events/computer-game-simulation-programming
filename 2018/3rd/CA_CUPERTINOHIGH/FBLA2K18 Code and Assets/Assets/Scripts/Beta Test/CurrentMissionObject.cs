using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CurrentMissionObject : MonoBehaviour {
    public bool MissionActive;

    public float involvement;
    public float charisma;
    public float knowledge;

    public TextMeshProUGUI MissionType;
    public TextMeshProUGUI MissionName;
    public TextMeshProUGUI MissionInfo;
    public TextMeshProUGUI MissionRewards;
    public TextMeshProUGUI MissionProgress;
    public string MissionNTExt;
    public GameObject MissionText;
    public GameObject NoMissionText;

    public float currentProgress = 0;
    public float totalProgress;

    public Button finishButton;
    public bool finishObjective = false;

    public GameObject involvementValue;
    public GameObject charismaValue;
    public GameObject knowledgeValue;

    public Button MissionButton;
    public Button ProfileButton;
    public Button CancelMissionButton;
    public TextMeshProUGUI CancelMissionText;

    Navigation MissionButtonNavNF = new Navigation();
    Navigation MissionButtonNavIF = new Navigation();
    //TODO 
    /*
     * Make sure to set ALL TEXT to 'Auto Size'!! 
     * 
     */
    public void Start()
    {
        MissionButtonNavNF.mode = Navigation.Mode.Explicit;
        MissionButtonNavNF.selectOnDown = CancelMissionButton;
        MissionButtonNavNF.selectOnRight = ProfileButton;

        MissionButtonNavIF.mode = Navigation.Mode.Explicit;
        MissionButtonNavIF.selectOnDown = finishButton;
        MissionButtonNavIF.selectOnRight = ProfileButton;

        involvementValue.GetComponent<StatObject>().SetStat(GameAndPlayerManager.Involvement);
        charismaValue.GetComponent<StatObject>().SetStat(GameAndPlayerManager.Charisma);
        knowledgeValue.GetComponent<StatObject>().SetStat(GameAndPlayerManager.Knowledge);
    }

    public void SaveStatsToGameController()
    {
        GameAndPlayerManager.AddInvolvement = (int)involvement;
        GameAndPlayerManager.AddCharisma = (int)charisma;
        GameAndPlayerManager.AddKnowledge = (int)knowledge;
    }

    public void UpdateProgress(float n)
    {
        currentProgress += n;
        MissionProgress.SetText(currentProgress + "/" + totalProgress);
        if (currentProgress >= totalProgress)
        {
            finishObjective = true;
        }
    }
    
    public void CancelMission()
    {
        MissionActive = false;
        MissionButton.Select();
        //reset mission
    }

    public void CompleteMission()
    {
        //add stats to the player's stats
        involvementValue.GetComponent<StatObject>().AddStat(involvement);
        charismaValue.GetComponent<StatObject>().AddStat(charisma);
        knowledgeValue.GetComponent<StatObject>().AddStat(knowledge);
        MissionActive = false;
        finishObjective = false;
        MissionButton.Select();
        GameAndPlayerManager.Involvement += (int)involvement;
        GameAndPlayerManager.Charisma += (int)charisma;
        GameAndPlayerManager.Knowledge += (int)knowledge;

    }

    //returns false if you can't add a new mission
    //return true if mission is added successfully
    public bool AddMission(MissionObject m)
    {
        if (MissionActive)
        {
            
            return false;
        } else
        {
            //fill in attributes
            MissionInfo.SetText(m.MissionInfo.text);
            MissionName.SetText(m.MissionName.text);
            MissionRewards.SetText(m.MissionRewards.text);
            MissionType.SetText(m.MissionType.text);
            involvement = m.Involvement;
            charisma = m.Charisma;
            knowledge = m.Knowledge;

            totalProgress = m.totalProgress;
            currentProgress = 0;
            MissionProgress.SetText(currentProgress + "/" + totalProgress);

            MissionActive = true;
            finishObjective = false;
            MissionNTExt = m.MissionName.text;
            return true;
        }
    }
    
	// Update is called once per frame
	void Update () {
		if (MissionActive)
        {
            MissionText.SetActive(true);
            NoMissionText.SetActive(false);
        } else
        {
            MissionText.SetActive(false);
            NoMissionText.SetActive(true);
        }

        //check if progress is finished
        if (finishObjective)
        {
            finishButton.gameObject.SetActive(true);
            CancelMissionButton.gameObject.SetActive(false);
            CancelMissionText.gameObject.SetActive(false);

            MissionButton.navigation = MissionButtonNavIF;
        } else
        {
            finishButton.gameObject.SetActive(false);
            CancelMissionButton.gameObject.SetActive(true);
            CancelMissionText.gameObject.SetActive(true);
            MissionButton.navigation = MissionButtonNavNF;
        }
	}

    
}
