using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour {


    Animator anim;
    public float delay;
    bool pressed = true;
    public string nxScene;
    public string clip;



    void Start()
    {
        anim = gameObject.GetComponent<Animator>();


    }

    void Update()
    {
        if (Input.anyKeyDown && pressed)
        {
            anim.SetBool(clip, true);
            StartCoroutine("PlayAnim");
            pressed = false;
        }
    }

    IEnumerator PlayAnim()
    {
        var result = SceneManager.LoadSceneAsync(nxScene);
        result.allowSceneActivation = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + delay);
        result.allowSceneActivation = true;
    }

}

