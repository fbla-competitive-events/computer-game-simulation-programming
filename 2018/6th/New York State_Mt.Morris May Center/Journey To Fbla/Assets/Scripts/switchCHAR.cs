using UnityEngine;
using System.Collections;

public class switchCHAR : MonoBehaviour {

    public GameObject switchCH;
    GameObject switchCHClone;
    public Transform challengesSpawnPoint;
    //GameObject player;
    //private bool On;

    void Awake() {
        //playerStuff();


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.S))
        {
            GenerateSwitchCHAR();
            Destroy(transform.parent.gameObject);
            //if (On)
            //{
                //player.SetActive(false);
                //On = false;
            //}           
        }
	}
    void GenerateSwitchCHAR()
    {
        switchCHClone = Instantiate(switchCH, challengesSpawnPoint.position, Quaternion.identity) as GameObject;
    }
    //void playerStuff()
    //{
        //player = GameObject.Find("Player");
        //On = true;
    //}
}
