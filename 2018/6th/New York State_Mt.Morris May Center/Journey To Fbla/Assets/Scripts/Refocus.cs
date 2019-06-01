using UnityEngine;
using UnityEngine.EventSystems;

public class Refocus : MonoBehaviour {

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null) 
		{
			Debug.Log ("reselecting first input");
			EventSystem.current.SetSelectedGameObject (EventSystem.current.firstSelectedGameObject);
		}
	}

}
