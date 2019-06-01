using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bugPointManager : MonoBehaviour {

	public static int bugPoints;
	public int goalPoints;
	public string nxtScene;

	void Start(){
		bugPoints = 0;
		PlayerController.champ = false;
		//SceneManager.LoadSceneAsync (nxtScene);
		//var result = SceneManager.LoadSceneAsync(nxtScene);
		//result.allowSceneActivation = false;


	}

	void Update(){
		if (bugPoints == goalPoints) {
			StartCoroutine (GlitchDone ());
		}
	}

	IEnumerator GlitchDone(){
		var result = SceneManager.LoadSceneAsync(nxtScene);
		result.allowSceneActivation = false;
		PlayerController.champ = true;
		yield return new WaitForSeconds (3);
		//SceneManager.LoadScene (nxtScene);
		result.allowSceneActivation = true;
	}
}
