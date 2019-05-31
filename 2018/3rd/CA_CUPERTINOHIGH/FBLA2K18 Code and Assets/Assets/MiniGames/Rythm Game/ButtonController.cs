using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public float ButtonPressSpeed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (!RythmGameController.Pause)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Pressed(Button1));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Pressed(Button2));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Pressed(Button3));
            }
        }
    }

    IEnumerator Pressed(GameObject g)
    {
        g.GetComponent<ButtonPressedScript>().OnPressed();
        g.transform.localScale = new Vector3(g.transform.localScale.x, 0.2f, g.transform.localScale.z);
        yield return new WaitForSeconds(ButtonPressSpeed);
        g.transform.localScale = new Vector3(g.transform.localScale.x, 0.3f, g.transform.localScale.z);
        g.GetComponent<ButtonPressedScript>().OffPressed();
    }
}
