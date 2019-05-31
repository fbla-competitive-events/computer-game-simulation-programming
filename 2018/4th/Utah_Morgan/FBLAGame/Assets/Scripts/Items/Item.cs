using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;

    /// <summary>
    /// The icon of the item
    /// </summary>
    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    /// <summary>
    /// The amount of items that can be stacked
    /// </summary>
    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    /// <summary>
    /// The slot the item sits on 
    /// </summary>
    public SlotScript Slot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    /// <summary>
    /// Removes the item
    /// </summary>
    public void Remove()
    {
        if (Slot != null)
        {
            slot.RemoveItem(this);
        }
    }
}
