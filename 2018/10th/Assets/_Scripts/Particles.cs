using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    GameObject orb;

    private void Start()
    {
        orb = this.gameObject;
        orb.SetActive(false);
    }

    public IEnumerator Click()
    {
        orb.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        orb.SetActive(false);
    }
}
