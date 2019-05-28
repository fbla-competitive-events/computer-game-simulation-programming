using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityLog : MonoBehaviour
{
    public GameObject activityLog;

    void Start()
    {
        activityLog.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LogScreen();
        }
	}

    void LogScreen()
    {
        if (activityLog.activeInHierarchy == false)
        {
            activityLog.SetActive(true);
        }
        else
        {
            activityLog.SetActive(false);
        }
    }
}
