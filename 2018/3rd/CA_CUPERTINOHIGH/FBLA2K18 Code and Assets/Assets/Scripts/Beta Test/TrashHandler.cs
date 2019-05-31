using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashHandler : MonoBehaviour {
    /*
     * What this script does is to spawn the objects that could be picked up
     *  - Once an object is picked up, it calls AddPoint() which also calls 
     *    "Current Mission"'s UpdateProgress()
     *    
     * */
    public List<TrashObject> trash = new List<TrashObject>();
    public float progress;

    //this is the 'Current Mission' so that it can track the completetion
    public GameObject Mission;

    public void Shuffle<TrashObject>(List<TrashObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            TrashObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    void Update () {
		if (!Mission.GetComponent<CurrentMissionObject>().MissionActive)
        {
            CancelMission();
        }
	}

    public void CancelMission()
    {
        foreach (TrashObject t in trash) t.gameObject.SetActive(false);
    }

    public void RenderTrash(float p)
    {
        if (!Mission.GetComponent<CurrentMissionObject>().MissionActive)
        {
            progress = p;
            Shuffle(trash);
            for (int i = 0; i < p; i++)
            {
                trash[i].gameObject.SetActive(true);
            }
        }
        /*foreach (TrashObject t in trash)
        {
            t.gameObject.SetActive(true);
        }*/
    }

    public void AddPoint()
    {
        progress--;
        //update progress in player profile
        Mission.GetComponent<CurrentMissionObject>().UpdateProgress(1);
        if (progress < 1)
        {
            foreach(TrashObject t in trash)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}
