using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DaveTalk : MonoBehaviour
{
    public string[] words;

    int Index = 0;

    public Text Stuff;
    public GameObject Dave;

    public FirstPersonController Player;

	// Use this for initialization
	void Start ()
    {
        Stuff = Stuff.GetComponent<Text>();
        Player = Player.GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Stuff.text = words[Index];
	}

    public void Next()
    {
        Index++;

        if (Index == words.Length)
        {
            Index = 0;
            Dave.SetActive(false);
            Player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked; 
        }
    }
}
