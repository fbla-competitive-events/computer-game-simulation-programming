using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    Animator anim;
    public float delay;

	void Start () {
        anim = gameObject.GetComponent<Animator>();

        StartCoroutine("PlayAnim");
	}
	
    IEnumerator PlayAnim()
    {
        var result = SceneManager.LoadSceneAsync("firstIntoCinematic");
        result.allowSceneActivation = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + delay);
        result.allowSceneActivation = true;
    }
	
}
