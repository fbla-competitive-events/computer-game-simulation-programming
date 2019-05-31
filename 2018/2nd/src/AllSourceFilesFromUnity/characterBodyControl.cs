using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;
using UnityEngine.SceneManagement;
public class characterBodyControl : MonoBehaviour {
	public GameObject lowerarmS=null;
	public GameObject lowerarmleftS = null;
	public GameObject headS = null;
	public GameObject handS=null;
	public GameObject handleftS=null;
	public GameObject neckS=null;
	public GameObject hair=null;
	public GameObject body=null;
	public GameObject upperarm = null;
	public GameObject upperarm2 = null;
	public Transform[] bodyParts;
	public EntityRenderer spriteRenderer;
	void Awake()
	{

	}
	public void assignParts()
	{
		bodyParts = transform.GetComponentsInChildren<Transform> ();
		foreach (Transform part in bodyParts) {
			if (part.name == "head") {
				headS = part.gameObject;
			} else if (part.name == "lower_002") {
				lowerarmS = part.gameObject;
			} else if (part.name == "lower_001") {
				lowerarmleftS = part.gameObject;
			} else if (part.name == "neck") {
				neckS = part.gameObject;
			} else if (part.name == "hand") {
				handS = part.gameObject;
			} else if (part.name == "hand_000") {
				handleftS = part.gameObject;
			} else if (part.name == "hair") {
				hair = part.gameObject;
			} else if (part.name == "body") {
				body = part.gameObject;
			} else if (part.name=="upper_002") {
				upperarm = part.gameObject;
			}
			else if(part.name=="upper_003") {
				upperarm2 = part.gameObject;
			}
		}
		hair.GetComponent<SpriteRenderer> ().sprite = GameControl.control.hairSpriteList [GameControl.control.selectedHair];
		headS.GetComponent<SpriteRenderer> ().sprite = GameControl.control.faceSpriteList [GameControl.control.selectedFace];
		body.GetComponent<SpriteRenderer> ().sprite = GameControl.control.bodySpriteList [GameControl.control.selectedBody];
		upperarm.GetComponent<SpriteRenderer> ().sprite = GameControl.control.upperSpriteList [GameControl.control.selectedBody];
		upperarm2.GetComponent<SpriteRenderer> ().sprite = GameControl.control.upperSpriteList [GameControl.control.selectedBody];
		//hair.GetComponents<SpriteRenderer> ().sprite = GameControl.control.bodylist [selectedBody];

	}

	public void changeHair(float hairS)
	{
		GameControl.control.selectedHair = (int)hairS;
		hair.GetComponent<SpriteRenderer> ().sprite = GameControl.control.hairSpriteList [(int)hairS];
	}
	public void changeFace(float face)
	{
		GameControl.control.selectedFace = (int)face;
		headS.GetComponent<SpriteRenderer> ().sprite = GameControl.control.faceSpriteList [GameControl.control.selectedFace];

	}
	public void changeBody(float B)
	{
		GameControl.control.selectedBody = (int)B;
		body.GetComponent<SpriteRenderer> ().sprite = GameControl.control.bodySpriteList [GameControl.control.selectedBody];
		upperarm.GetComponent<SpriteRenderer> ().sprite = GameControl.control.upperSpriteList [GameControl.control.selectedBody];
		upperarm2.GetComponent<SpriteRenderer> ().sprite = GameControl.control.upperSpriteList [GameControl.control.selectedBody];

	}
	public  void changeColorVals()
	{

		hair.GetComponent<SpriteRenderer> ().color = GameControl.control.hairColor;
		headS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;
		lowerarmS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;
		lowerarmleftS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;
		neckS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;
		handS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;
		handleftS.GetComponent<SpriteRenderer> ().color = GameControl.control.skintone;

	}
	public void changeSkinColor(float newskintone)
	{
		GameControl.control.skintone= new Color (0.5F*newskintone,0.3F*newskintone,0.2F*newskintone,1.0F);
		changeColorVals ();
	}
	void Start () {
		//GameControl.control.SaveLoad ();

		assignParts ();
		changeColorVals ();

		if(SceneManager.GetActiveScene().name=="main") 
			transform.parent.position = GameControl.control.latestCharPositionOutdoors;
		else if(SceneManager.GetActiveScene().name!="bugcheck" && SceneManager.GetActiveScene().name!="customize")
		transform.parent.position = GameControl.control.latestCharPositionIndoors;
	
		
		Color tmp= spriteRenderer.Color;
		tmp.a = 1f;
		spriteRenderer.Color = tmp;
		changeColorVals ();

	}

	// Update is called once per frame
	void Update () {
		
	}
}
