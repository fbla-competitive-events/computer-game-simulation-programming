using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    /// <summary>
    /// The bag item associated with this script
    /// </summary>
    public Bag Bag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty; 
            }
            bag = value;
        }
    }

    /// <summary>
    /// Called when the user clicks on the back
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryManager.Instance.FromSlot != null && HandScript.Instance.MyMoveable != null && HandScript.Instance.MyMoveable is Bag)
            {
                if (Bag != null)
                {
                    InventoryManager.Instance.SwapBags(Bag, HandScript.Instance.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.Instance.MyMoveable;
                    tmp.BagButton = this;
                    tmp.Use();
                    Bag = tmp;
                    HandScript.Instance.Drop();
                    InventoryManager.Instance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.Instance.TakeMoveable(Bag);
            }
            else if (bag != null)
            {
                bag.BagScript.OpenClose();
            }
        }        
    }

    /// <summary>
    /// Removes the bag from the button
    /// </summary>
    public void RemoveBag()
    {
        InventoryManager.Instance.RemoveBag(Bag);
        Bag.BagButton = null;

        foreach (Item item in Bag.BagScript.GetItems())
        {
            InventoryManager.Instance.AddItem(item);
        }
        Bag = null;
    }

}
