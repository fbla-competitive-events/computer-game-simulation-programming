using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
	public int offsetX= 2;
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;
	public bool reverseScale=false; //used if object isn't tileable
	private float spriteWidth=0f;
	private Camera cam;
	private Transform myTransform;
	// Use this for initialization
	void Awake()
	{
		cam = Camera.main;
		myTransform = transform;
	}
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;

	}
	
	// Update is called once per frame
	void Update () {
		if (!hasALeftBuddy || !hasARightBuddy) {
			// calculated camera's extent. Half width of what camera can see in word coardinates instead of pixels
			float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

			//calculate x position where camera can see sprite edge
			float edgeVisiblePosRight=(myTransform.position.x+spriteWidth/2)-camHorizontalExtent;
			float edgeVisiblePosLeft=(myTransform.position.x-spriteWidth/2)+camHorizontalExtent;
			if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && !hasARightBuddy) {
				makeNewBuddy (1);
				hasARightBuddy = true;
			} else if (cam.transform.position.x <= edgeVisiblePosLeft + offsetX && !hasALeftBuddy) {
				makeNewBuddy (-1);
				hasALeftBuddy = true;
			}
		}
	}

	void makeNewBuddy(int rightOrLeft)
	{
		//Position for new body
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y,myTransform.position.z);
		//instantiating newBuddy and storing him in a variable
		Transform newBuddy= Instantiate(myTransform,newPosition,myTransform.rotation) as Transform;
		if (reverseScale == true) {
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1,newBuddy.localScale.y, newBuddy.localScale.z);
		}
			
		newBuddy.parent = myTransform.parent;
		if (rightOrLeft > 0) {
		
			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;

		} else {
			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true;

		}
	}
}
