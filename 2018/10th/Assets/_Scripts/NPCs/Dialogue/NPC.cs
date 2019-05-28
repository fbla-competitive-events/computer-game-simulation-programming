using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class NPC : MonoBehaviour
{
    public FirstPersonController Player;

    public GameObject interact;

    public bool triggering;
    bool d;
    bool n;
    bool j;
    bool k;
    bool de;
    bool c;

    public GameObject dave;
    public GameObject necro;
    public GameObject jones;
    public GameObject kevin;
    public GameObject debby;
    public GameObject chad;

	// Use this for initialization
	void Start ()
    {
        Player = Player.GetComponent<FirstPersonController>();
	}

    private void Update()
    {
        if (triggering)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interact.SetActive(false);

                if (d)
                {
                    dave.SetActive(true);
                }
                if (n)
                {
                    necro.SetActive(true);
                }
                if (j)
                {
                    jones.SetActive(true);
                }
                if (k)
                {
                    kevin.SetActive(true);
                }
                if (de)
                {
                    debby.SetActive(true);
                }
                if (c)
                {
                    chad.SetActive(true);
                }

                if (dave.activeInHierarchy == true || necro.activeInHierarchy == true || jones.activeInHierarchy == true || kevin.activeInHierarchy == true || debby.activeInHierarchy == true || chad.activeInHierarchy == true)
                {
                    Player.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggering = true;
        
        
        if (triggering)
        {
            if (other.tag == "Dave")
            {
                d = true;
                interact.SetActive(true);
            }

            if (other.tag == "Necro")
            {
                n = true;
                interact.SetActive(true);
            }
            if (other.tag == "Jones")
            {
                j = true;
                interact.SetActive(true);
            }
            if (other.tag == "Kevin")
            {
                k = true;
                interact.SetActive(true);
            }
            if (other.tag == "Debby")
            {
                de = true;
                interact.SetActive(true);
            }
            if (other.tag == "Chad")
            {
                c = true;
                interact.SetActive(true);
            }
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        Continue();
    }

    public void Continue()
    {
        triggering = false;
        d = false;
        n = false;
        j = false;
        k = false;
        de = false;
        c = false;
        dave.SetActive(false);
        necro.SetActive(false);
        jones.SetActive(false);
        kevin.SetActive(false);
        debby.SetActive(false);
        chad.SetActive(false);
        Player.enabled = true;
        interact.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
