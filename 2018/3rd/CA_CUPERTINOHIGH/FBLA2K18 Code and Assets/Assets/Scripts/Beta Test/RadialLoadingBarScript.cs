using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialLoadingBarScript : MonoBehaviour {
    public Image Center;
    //this is the ProgressBar GameObject under 'Radial Loading Bar Canvas'
    public GameObject ProgressBar;
    public float timer = 0;
    public bool heldDown = false;
    public float loadSpeed = 1;

    public GameObject obj;
    // Update is called once per frame
    void Update () {
		if (ProgressBar.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            heldDown = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            heldDown = false;
            timer = 0;
            Center.fillAmount = 0;
            GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = true;
        }

        if (heldDown)
        {
            timer++;
            Center.fillAmount = timer*loadSpeed / 100;
            GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = false;
        }
        if (Center.fillAmount == 1)
        {
            FinishLoading();
        }
    }

    public void FinishLoading()
    {
        obj.GetComponent<RepairableObject>().FinishLoading();
        gameObject.SetActive(false);
        timer = 0;
        heldDown = false;
        Center.fillAmount = 0;
        GameObject.Find("Main Player").GetComponent<PlayerMotor1>().CanPlayerMove = true;
    }

    public void SetGameObjectToCall(GameObject g)
    {

        obj = g;
        gameObject.SetActive(true);
    }

    
}
