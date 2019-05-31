using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour {

	public WordManager wordManager;


	public int wordNum;
	public float timer = 0f;

	public float standardtime=2f;
	private void Start()
	{
		/*for(int i=0; i<wordNum;i++){
			wordManager.AddWord();

		}*/
	}
	void Update()
	{
		if (GameControl.control.wordStarted) {
			timer -= .05f;
			if (timer <= 0) {
				wordManager.AddWord ();
		
				timer = 2f;

			}
		}
	}
}
