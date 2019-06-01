using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enterCoin : MonoBehaviour
{

    Animator anim;
    public float delay;
    bool pressed = true;
    

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        
    }

    void Update()
    {
        if (Input.anyKeyDown && pressed)
        {
            anim.SetBool("enteredCoin", true);
            StartCoroutine("PlayAnim");
            pressed = false;
        }
    }

    IEnumerator PlayAnim()
    {
        var result = SceneManager.LoadSceneAsync("titleScreen");
        result.allowSceneActivation = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + delay);
        result.allowSceneActivation = true;
    }

}
