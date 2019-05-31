using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Creates a scrolling text, like when the player earns XP and stuff
/// </summary>
public class ScrollingText : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The speed that the text floats up
    /// </summary>
    private float speed;

    /// <summary>
    /// The direction that it floats
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// How long it takes for the text to fade
    /// </summary>
    private float fadeTime;

    /// <summary>
    /// The animation clip that allows the text to bulge
    /// </summary>
    [SerializeField]
    private AnimationClip bulgeAnim;

    /// <summary>
    /// If the text will bulge or not
    /// </summary>
    private bool bulge;

    /// <summary>
    /// If the text can move
    /// </summary>
    private bool canMove;

    /// <summary>
    /// If the text is a notification
    /// </summary>
    private bool notification;

    /// <summary>
    /// The starting position of the text
    /// </summary>
    private Vector3 initPosition;
	
	/// <summary>
    /// Called once per frame. 
    /// </summary>
	void Update ()
    {
        if (!bulge && canMove)
        {
            float translation = speed * Time.deltaTime;

            transform.Translate(direction * translation);
        }        
	}

    /// <summary>
    /// Initializes the text
    /// </summary>
    /// <param name="speed">The speed of scrolling</param>
    /// <param name="direction">The direction it scrolls</param>
    /// <param name="fadeTime">The time it takes to fade</param>
    /// <param name="bulge">If the text bulges</param>
    /// <param name="notification">If the text is a notification</param>
    public void Initialize(float speed, Vector3 direction, float fadeTime, bool bulge, bool notification = false)
    {
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        this.bulge = bulge;
        initPosition = transform.position;
        this.notification = notification;

        //if it is a notification, put it at the top of the screen
        if (notification)
        {
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(23, 170);
        }

        canMove = notification;
        GetComponent<Text>().enabled = false;          
    }

    /// <summary>
    /// Allows the text to start scrolling
    /// </summary>
    public void Activate()
    {
        canMove = true;
        GetComponent<Text>().enabled = true;
        if (bulge)
        {
            //Starts the bulge animation
            StartCoroutine(Bulge(notification));
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    /// <summary>
    /// Gets rid of the text
    /// </summary>
    public void Deactivate()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Activates the bulge animation
    /// </summary>
    /// <param name="repeat">If the text should keep bulging or not</param>
    /// <returns></returns>
    private IEnumerator Bulge(bool repeat = false)
    {
        do
        {
            GetComponent<Animator>().SetTrigger("Bulge");
            yield return new WaitForSeconds(bulgeAnim.length);
        } while (repeat);

        //After the text is done bulging, make it start fading
        bulge = false;
        StartCoroutine(FadeOut());
    }
    
    /// <summary>
    /// Starts the fading out process
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        float startAplpha = GetComponent<Text>().color.a;

        float rate = 1.0f / fadeTime;
        float progress = 0.0f;

        //Slowly lessen the alpha
        while (progress < 1.0)
        {
            Color tmpColor = GetComponent<Text>().color;

            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAplpha, 0, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }

        //Once the text is done fading, deactivate it
        Deactivate();
    }    

    /// <summary>
    /// When the user clicks on the text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Deactivate();
    }
}
