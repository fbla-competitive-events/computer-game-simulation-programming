using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenuBoard : MonoBehaviour {
    public bool isInteractable;
    public Transform Player;
    public float inBoundOfInteractions;

    public GameObject TutorialBox;
    public TextBoxManager textManager;
    // Use this for initialization
    void Start()
    {
        isInteractable = false;
        Player = GameObject.Find("Main Player").GetComponent<Transform>();
        inBoundOfInteractions = 50f;
    }

    void Update()
    {
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
                    textManager.SetText("Tutorial OBJ Text");
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

        if (isInteractable) GUI.Box(new Rect(Screen.width / 2 - Screen.width / 6 / 2, Screen.height - Screen.height / 8, Screen.width / 6, Screen.height / 8), ("Interact [E]"));

    }
}
