using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableObject : MonoBehaviour {
    public Transform Player;
    public float inBoundOfInteractions;

    public bool isRepaired = false;
    public GameObject RadialLoadingBar;

    public static int Contacts = 0;
    public void FinishLoading()
    {
        //sparkle animation
        GetComponentInParent<RepairObjectsHandler>().AddPoint();
        Debug.Log("YAY!");
        isRepaired = true;
        if (inContact) Contacts--;
    }

    void Start()
    {
        Player = GameObject.Find("Main Player").GetComponent<Transform>();
        inBoundOfInteractions = 10f;

    }

    public void Render()
    {
        gameObject.SetActive(true);
        isRepaired = false;
    }
    private bool inContact = false;
    void Update()
    {
        
        if (gameObject.activeSelf && !isRepaired)
        {
            var heading = Player.position - GetComponent<Transform>().position;
            heading.y = 0;

            float offset = GetComponent<Collider>().bounds.size.x * Player.localScale.x;

            if (heading.sqrMagnitude <= inBoundOfInteractions + offset * 1.5)
            {
                if (!inContact) Contacts++;
                inContact = true;
                
                if (!isRepaired)
                {                    
                    RadialLoadingBar.GetComponent<RadialLoadingBarScript>().SetGameObjectToCall(gameObject);
                    
                }
            } else
            {
                if (inContact) Contacts--;
                inContact = false;
                if (!isRepaired && RadialLoadingBar.activeSelf)
                {
                    if (Contacts==0) RadialLoadingBar.SetActive(false);
                }
            }
        }

    }
}
