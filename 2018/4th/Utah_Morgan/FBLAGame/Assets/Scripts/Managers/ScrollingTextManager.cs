using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingTextManager : MonoBehaviour
{
    /// <summary>
    /// The Scrolling text prefab
    /// </summary>
    [SerializeField]
    private GameObject textPrefab;

    /// <summary>
    /// The parent of the new texts
    /// </summary>
    [SerializeField]
    private Transform canvasTransform;

    /// <summary>
    /// The speed that the text float at
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// The time it takes for the text to fade
    /// </summary>
    [SerializeField]
    private float fadeTime;

    /// <summary>
    /// The direction the text is traveling
    /// </summary>
    [SerializeField]
    private Vector3 direction;
   
    /// <summary>
    /// The list of all of the texts
    /// </summary>
    List<ScrollingText> scrList = new List<ScrollingText>();

    /// <summary>
    /// Says if a new text can be displayed
    /// </summary>
    bool OnCoolDown;

    /// <summary>
    /// The single ton instance of this class
    /// </summary>
    public static ScrollingTextManager Instance
    {
        get
        {
            return GameObject.FindObjectOfType<ScrollingTextManager>();
        }       
    }

    /// <summary>
    /// A reference to the notification text, if there is one
    /// </summary>
    public ScrollingText NotificationText
    {
        get
        {
            return notificationText;
        }

        private set
        {
            notificationText = value;
        }
    }
    private ScrollingText notificationText;

    /// <summary>
    /// Called one per frame
    /// </summary>
    private void Update()
    {
        //If the text can be displayed, display the next one in the list
        if (!OnCoolDown)
        {
            if (scrList.Count > 0)
            {
                ScrollingText text = scrList[0];
                text.Activate();
                scrList.Remove(text);
                StartCoroutine(CoolDown());
            }                
        }
    }

    /// <summary>
    /// Adds a new text to the list of texts to be displayed. We be displayed when the cool down is done
    /// </summary>
    /// <param name="Position">The position of the text</param>
    /// <param name="color">The color of the text</param>
    /// <param name="text">What the text says</param>
    /// <param name="bulge">If the text bulges</param>
    /// <param name="notification">If the text is a notification</param>
    public void CreatText(Vector3 Position, Color color, string text, bool bulge, bool notification = false)
    {
        //Creates a new text, sets its parent, and adds it to the list of texts
        GameObject stext = (GameObject)Instantiate(textPrefab, Position, Quaternion.identity);
        stext.transform.position = new Vector3(Position.x, Position.y + .5f, Position.z);
        stext.transform.SetParent(canvasTransform);
        stext.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        ScrollingText t = stext.GetComponent<ScrollingText>();
        if (notification) NotificationText = t;
        scrList.Add(t);

        //Initialize the text
        t.Initialize(speed, direction, fadeTime, bulge, notification);
        stext.GetComponent<Text>().text = text;
        stext.GetComponent<Text>().color = color;
    }

    /// <summary>
    /// Allows the text to only be displayed every one second
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoolDown()
    {
        OnCoolDown = true;
        yield return new WaitForSeconds(1f);
        OnCoolDown = false;
    }
}
