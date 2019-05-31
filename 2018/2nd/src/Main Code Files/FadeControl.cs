using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnitySampleAssets._2D;
/*
This class contains the functions used for the fade animations and transitions between and within scenes.
It is attached to a panel object for the animations
*/
public class FadeControl : MonoBehaviour {
	//The actual animator componenet of the panel
	public Animator animator;
	// Use this for initialization
	void Start () {
		animator.SetBool ("fadeIn", false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
/*The floor change function must be handled through and Ienumerator because it
 requires specific timing and waiting, so it is called through this function
 */
	public void floorChange(Vector3 pos, GameObject character)
	{
		StartCoroutine (floorChangeAnim (pos,character));
	}
	/*
	This functions handles changing floor. Vector 3 pos is equal to the position that the character should be moved to
	First, character movement is disabled, then the animator is played. There is a yield return null until the animation
	fade panel is fully opaque. Once it reaches an alpha of 1 and is longer transparent, the character direction is flipped,
	his position is changed, and the camera is slowed so that the animation is not awkward. Then the panel is set to fade out
	and character movement and the camera is restored.
	*/
	IEnumerator floorChangeAnim(Vector3 pos,GameObject character)
	{
		PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = false;

		animator.SetBool ("fadeIn", true);
		while (gameObject.GetComponent<CanvasGroup> ().alpha < 1) {
			yield return null;
		}
		GameControl.control.isCharFlipCorrect = false;

		character.transform.position = pos;
			Camera.main.GetComponent<Camera2DFollow> ().damping=0.1f;
		animator.SetBool ("fadeIn", false);
		yield return new WaitForSeconds (1);
		PlatformerCharacter2D.control.GetComponent<Platformer2DUserControl> ().isMovementEnabled = true;

		Camera.main.GetComponent<Camera2DFollow> ().damping=1f;


	}
	/*
	Similiar to floor change, level change requires timing so an Ienumertor is used and this function calls it.
	*/
	public void levelChange(string scene, GameObject p)
	{
		StartCoroutine(loadScene (scene,p));
	}
	/*
	String scene is the name of the destianation scene. p is the panel for animation. 
	The animator is set to fade in, and return null until completely opaque, once opaque,
	scene is changed.
	*/
	IEnumerator loadScene(string scene,GameObject p)
	{
		animator.SetBool ("fadeIn", true);

		while(p.GetComponent<CanvasGroup>().alpha<1)
		{
			yield return null;
		}
		SceneManager.LoadScene (scene, LoadSceneMode.Single);
	}
}
