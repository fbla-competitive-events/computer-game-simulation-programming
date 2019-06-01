using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
	public GameObject spaceHudOn;
	public static bool dialogueOn;
	public static bool nextScene;
	public static bool contDlog;
	public static bool skip;
	public bool nxtScene;
	public bool teach;
	public bool allow;
	//public string loadScene;

	void Start(){
		dialogueOn = false;
			nextScene = false;
		contDlog = false;
	}

	void Update(){
		if (teach && Input.GetKeyDown(KeyCode.Space)) {
			skip = true;
			TriggerDialogue ();
			teach = false;

			if (nxtScene == true) {
				nextScene = true;
			} else if(!nxtScene){
				nextScene = false;
			}
		}
		//if //(Input.GetKeyDown (KeyCode.Return)) {
			//contDialogue ();
			//Debug.Log ("Hello");

		//}
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player"){
			if (!dialogueOn) {
				spaceHudOn.SetActive (true);
			} else {
				spaceHudOn.SetActive (false);
			}
			if (Input.GetKeyDown(KeyCode.Space) && !dialogueOn) {
				TriggerDialogue ();
				dialogueOn = true;
				if (nxtScene == true) {
					nextScene = true;
				} else if (nxtScene == false){
					nextScene = false;
				}
			}
		}
	}
	void OnTriggerExit2D(Collider2D other){
		spaceHudOn.SetActive (false);

	}

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		//if (nextScene && !DialogueManager.dialogueOn) {
			//SceneManager.LoadScene (loadScene);
		//}
	}
	//public void contDialogue ()
	//{
		//FindObjectOfType<DialogueManager> ().DisplayNextSentence ();
		//if (nextScene && !DialogueManager.dialogueOn) {
		//SceneManager.LoadScene (loadScene);
		//}
	//}
}
