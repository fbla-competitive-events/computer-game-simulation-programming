﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiddleLevelTransition : MonoBehaviour
{
    public bool isInteractable;
    public Transform Player;
    public float inBoundOfInteractions;

    public CurrentMissionObject CurrentMission; 

    void Start()
    {
        isInteractable = false;
        Player = GameObject.Find("Main Player").GetComponent<Transform>();
        inBoundOfInteractions = 15f;
    }

    public void Render()
    {
        if (!CurrentMission.MissionActive)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CurrentMission.MissionActive)
        {
            gameObject.SetActive(false);
        }
        if (gameObject.activeSelf)
        {
            var heading = Player.position - GetComponent<Transform>().position;
            heading.y = 0;

            float offset = GetComponent<Collider>().bounds.size.x * Player.localScale.x;

            if (heading.sqrMagnitude <= inBoundOfInteractions + offset * 1.5)
            {
                isInteractable = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //make transition
                    CurrentMission.SaveStatsToGameController();
                    GameAndPlayerManager.StartMiddleLevel();
                    
                }
            }
            else
            {
                isInteractable = false;
            }
        }
    }

    private void OnGUI()
    {
        
        if (isInteractable) {
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = Screen.width / 20;
            GUI.Box(new Rect(Screen.width / 2 - Screen.width / 6 / 2, Screen.height - Screen.height / 8, Screen.width / 6, Screen.height / 8), "Enter [E]", myStyle);
        }

    }
}