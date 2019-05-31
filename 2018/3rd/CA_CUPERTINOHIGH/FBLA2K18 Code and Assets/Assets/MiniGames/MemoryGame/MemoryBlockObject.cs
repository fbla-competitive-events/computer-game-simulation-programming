using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBlockObject : MonoBehaviour {
    public Material Mat;
    public Material UserMat;
    private Material orgMat;
    public float ButtonPressedSpeed = 0.1f;
    public void Pressed()
    {
        StartCoroutine(Press());
    }

    public void Shake()
    {
        StartCoroutine(Shaking());
    }
    IEnumerator Press()
    {
        MemoryGameController.Pause = true;
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = UserMat;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 0.2f);
        yield return new WaitForSeconds(ButtonPressedSpeed);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 0.5f);
        MemoryGameController.Pause = false;
    }

    IEnumerator Shaking()
    {
        MemoryGameController.Pause = true;
        gameObject.transform.Translate(new Vector3(0.1f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.Translate(new Vector3(-0.2f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.Translate(new Vector3(0.2f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.Translate(new Vector3(-0.2f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.Translate(new Vector3(0.2f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.Translate(new Vector3(-0.1f, 0f, 0f));
        yield return new WaitForSeconds(0.01f);
        MemoryGameController.Pause = false;
    }

    public void UserSelect()
    {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = UserMat;
    }

    public void UserDeselect()
    {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = orgMat;
    }


    public void Select()
    {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = Mat;
    }

    public void Deselect()
    {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = orgMat;
    }

	// Use this for initialization
	void Start () {
        orgMat = GetComponent<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
