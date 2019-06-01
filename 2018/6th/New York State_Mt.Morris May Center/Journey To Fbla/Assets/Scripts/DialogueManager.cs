using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text DialogueText;
	public static bool dialogueOn;
	public string loadScene;

	public Animator anim;

	private Queue<string> sentences;


	void Start () {
		sentences = new Queue<string> ();
	}

	public void StartDialogue (Dialogue dialogue)
	{
		anim.SetBool ("isOpen", true);
		dialogueOn = true;
		//Time.timeScale = 0;

		nameText.text = dialogue.name;

		sentences.Clear ();

		foreach (string sentence in dialogue.sentence) {
			sentences.Enqueue (sentence);
		}
		if (DialogueTrigger.skip == true) {
			DisplayNextSentence ();
			DialogueTrigger.skip = false;
		}


	}

	public void DisplayNextSentence(){
			if (sentences.Count == 0) {
				EndDialogue ();
				Debug.Log ("oops");
				//DialogueTrigger.contDlog = false;
				return;
		}
		string sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		StartCoroutine (TypeSentence (sentence));
	}

	IEnumerator TypeSentence (string sentence){
		DialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			DialogueText.text += letter;
			yield return null;
			DialogueTrigger.contDlog = true;
		}
	}

	void EndDialogue(){
		anim.SetBool ("isOpen", false);
		Debug.Log ("End of converastation:");
		dialogueOn = false;
		DialogueTrigger.dialogueOn = false;
		if (DialogueTrigger.nextScene) {
			SceneManager.LoadScene (loadScene);
		}
		//Time.timeScale = 1;
	}
}
