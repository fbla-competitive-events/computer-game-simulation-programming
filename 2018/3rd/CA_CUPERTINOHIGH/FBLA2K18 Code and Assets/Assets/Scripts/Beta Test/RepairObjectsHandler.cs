using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObjectsHandler : MonoBehaviour {
    public List<RepairableObject> repairableObjects = new List<RepairableObject>();
    public int progress;

    //this is the 'Current Mission' so that it can track the completetion
    public GameObject Mission;


    public void Shuffle<RepairableObject>(List<RepairableObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            RepairableObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }



    private void Start()
    {
        //repairableObjects = GetComponentsInChildren<RepairableObject>();
    }

    void Update()
    {
        if (!Mission.GetComponent<CurrentMissionObject>().MissionActive)
        {
            CancelMission();
        }
    }

    public void CancelMission()
    {
        foreach (RepairableObject r in repairableObjects) r.gameObject.SetActive(false);
    }

    public void Render(int p)
    {
        if (!Mission.GetComponent<CurrentMissionObject>().MissionActive)
        {
            progress = p;

            Shuffle(repairableObjects);
            for (int i = 0; i < p; i++)
            {
                repairableObjects[i].Render();
            }
        }

        /*
        foreach (RepairableObject t in repairableObjects)
        {
            t.Render();
        }*/
    }

    public void AddPoint()
    {
        progress--;
        //update progress in player profile
        Mission.GetComponent<CurrentMissionObject>().UpdateProgress(1);
        if (progress < 1)
        {
            foreach (RepairableObject t in repairableObjects)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}
