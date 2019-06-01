using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeIn : MonoBehaviour {

    public float fadeInTime = 1.0f;

    void Start()
    {
        StartCoroutine(SpriteFadeOut(GetComponent<SpriteRenderer>()));
    }

    IEnumerator SpriteFadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;

        while (tmpColor.a <= 0f)
        {
            tmpColor.a += Time.deltaTime * fadeInTime;
            _sprite.color = tmpColor;

            if (tmpColor.a >= 255.0f)
                tmpColor.a = 255;

            yield return null;
        }
        _sprite.color = tmpColor;
    }


}
