using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerSpark : MonoBehaviour
{
    public GameObject Spark;
    public GameObject AntiSpark;

    private void Start()
    {
        Spark.SetActive(false);
        AntiSpark.SetActive(false);
    }

    public IEnumerator Click()
    {
        Spark.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        Spark.SetActive(false);
    }

    public IEnumerator AntiClick()
    {
        AntiSpark.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        AntiSpark.SetActive(false);
    }

    public void Activate()
    {
        StartCoroutine(Click());
    }

    public void DeActivate()
    {
        StartCoroutine(AntiClick());
    }
}
