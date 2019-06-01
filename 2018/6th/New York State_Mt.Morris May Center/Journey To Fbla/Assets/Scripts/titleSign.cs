using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleSign : MonoBehaviour {
    public Animator anim;
    GameObject whiteFade;
    GameObject backGround;
    public string placeHolder;
    public string nextScene;
    //^^^whatever the background needs to be

    void Awake()
    {
        whiteFade = GameObject.Find("NewSprite");
        whiteFade.SetActive(false);
        backGround = GameObject.Find(placeHolder);
        backGround.SetActive(false);
    }

    void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("title"))
        {
            anim.SetBool("signLightOn", true);
            
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("title1"))
        {
            anim.SetBool("boltStrike", true);
            whiteFade.SetActive(true);
            backGround.SetActive(true);
        }
    }


}
