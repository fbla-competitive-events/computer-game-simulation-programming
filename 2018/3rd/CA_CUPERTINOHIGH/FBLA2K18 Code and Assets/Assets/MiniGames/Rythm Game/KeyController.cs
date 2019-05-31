using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KeyController : MonoBehaviour {
    public GameObject HUD;
    public GameObject key;
    private bool makeKey = false;
    //public float tempo = 1f;
    public float BPM = 120;
    //stores all the beats
    List<GameObject> beats = new List<GameObject>();

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MissedText;

	// Update is called once per frame
	void Update () {
        if (makeKey && !RythmGameController.Pause)
        {
            makeKey = false;
            StartCoroutine(SpawnKey());
        }
    }

    public void Play()
    {
        HUD.SetActive(true);
        ScoreText.gameObject.SetActive(true);
        MissedText.gameObject.SetActive(true);
        foreach (GameObject g in beats)
        {
            Destroy(g);
        }
        makeKey = true;
        RythmGameController.Pause = false;
        RythmGameController.Reset();
    }

    IEnumerator SpawnKey()
    {
        if (!RythmGameController.Pause)
        {
            yield return new WaitForSeconds(60 / BPM);
            
            float xPos = -0.5f;
            int num = Random.Range(0, 3);
            xPos += 0.5f * num;
            GameObject a = Instantiate(key, new Vector3(xPos, 1.708f, -3.976f), key.transform.rotation);
            a.AddComponent<KeyObject>();
            makeKey = true;
            if (!RythmGameController.Pause) beats.Add(a);
            else Destroy(a);
            
        }
    }
}
