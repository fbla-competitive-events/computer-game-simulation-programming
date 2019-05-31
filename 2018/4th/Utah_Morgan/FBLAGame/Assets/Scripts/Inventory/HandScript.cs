using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the handscript
    /// </summary>
    private static HandScript instance;

    public static HandScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    /// <summary>
    /// The current moveable
    /// </summary>
    public IMoveable MyMoveable { get; set; }

    /// <summary>
    /// The icon of the item, that we acre moving around atm.
    /// </summary>
    private Image icon;

    /// <summary>
    /// An offset to move the icon away from the mouse
    /// </summary>
    [SerializeField]
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        //Creates a reference to the image on the hand
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Makes sure that the icon follows the hand
        icon.transform.position = Input.mousePosition + offset;
    }

    /// <summary>
    /// Take a moveable in the hand, so that we can move it around
    /// </summary>
    /// <param name="moveable">The moveable to pick up</param>
    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    /// <summary>
    /// Drops the selected item and returns it;
    /// </summary>
    /// <returns></returns>
    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        Drop();
        return tmp;
    }

    /// <summary>
    /// Deselects the current item in hand
    /// </summary>
    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }
}
