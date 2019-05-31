using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    /// <summary>
    /// A reference to the bag that this slot belongs to
    /// </summary>
    public BagScript Bag { get; set; }

    /// <summary>
    /// Returns true if the slot is empty and false if an item is in the slot
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return Items.Count == 0;
        }
    }

    /// <summary>
    /// Returns true if there cannot be anymore items stacked
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (IsEmpty || Count < Item.StackSize)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// An item on the slot
    /// </summary>
    public Item Item
    {
        get
        {
            if (!IsEmpty)
            {
                return Items.Peek();
            }
            return null;
        }
    }

    /// <summary>
    /// The icon of an item on the slot
    /// </summary>
    public Image Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    /// <summary>
    /// The amount of stacked items on the slot
    /// </summary>
    public int Count
    {
        get
        {
            return Items.Count;
        }
    }

    /// <summary>
    /// The text that displays the number of items on the stack
    /// </summary>
    public Text StackText
    {
        get
        {
            return stackSize;
        }
    }

    /// <summary>
    /// All of the items on the slot
    /// </summary>
    public ObservableStack<Item> Items
    {
        get
        {
            return items;
        }        
    }

    private void Awake()
    {        
        Items.OnPop += UpdateSlot;
        Items.OnPush += UpdateSlot;
        Items.OnClear += UpdateSlot;
    }

    /// <summary>
    /// Adds an item to this slot
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <returns>True if the item can be added</returns>
    public bool AddItem(Item item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.Slot = this;
        return true;
    }

    /// <summary>
    /// Adds multiple items on the slot
    /// </summary>
    /// <param name="newItems">The items to add</param>
    /// <returns>True if the items can be added</returns>
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == Item.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes an item from the slot
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            Items.Pop();            
        }
    }

    /// <summary>
    /// Selects or places an item when clicked on this slot
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //If we don't have something to move
            if (InventoryManager.Instance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.Instance.MyMoveable != null && HandScript.Instance.MyMoveable is Bag)
                {
                    if (Item is Bag)
                    {
                        InventoryManager.Instance.SwapBags(HandScript.Instance.MyMoveable as Bag, Item as Bag);
                    }
                }
                else
                {
                    HandScript.Instance.TakeMoveable(Item as IMoveable);
                    InventoryManager.Instance.FromSlot = this;
                }                
            }
            else if (InventoryManager.Instance.FromSlot == null && IsEmpty && (HandScript.Instance.MyMoveable is Bag))
            {
                Bag bag = HandScript.Instance.MyMoveable as Bag;

                if (bag.BagScript != Bag && InventoryManager.Instance.EmptySlotCount - bag.Slots > 0)
                {
                    AddItem(bag);
                    bag.BagButton.RemoveBag();
                    HandScript.Instance.Drop();
                }                
            }
            //If we have something to move
            else if (InventoryManager.Instance.FromSlot != null)
            {
                if (PutItemBack() || SwapItems(InventoryManager.Instance.FromSlot) || AddItems(InventoryManager.Instance.FromSlot.Items))
                {
                    HandScript.Instance.Drop();
                    InventoryManager.Instance.FromSlot = null;
                }
            }            
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    /// <summary>
    /// Call the Use function on an item that is on the slot
    /// </summary>
    public void UseItem()
    {
        if (Item is IUseable)
        {
            (Item as IUseable).Use();
        }
    }

    /// <summary>
    /// Place an item on top of another same item
    /// </summary>
    /// <param name="item">Item to stack</param>
    /// <returns>True if the stack was successful</returns>
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == Item.name && Items.Count < Item.StackSize)
        {
            Items.Push(item);
            item.Slot = this;
            return true;
        }

        return false;   
    }

    /// <summary>
    /// Used to put item in hand back into its own slot
    /// </summary>
    /// <returns>True if put back successfully</returns>
    private bool PutItemBack()
    {
        //Put item back to same slot
        if (InventoryManager.Instance.FromSlot == this)
        {
            InventoryManager.Instance.FromSlot.icon.color = Color.white;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Swap this slot's item with a selcted item
    /// </summary>
    /// <param name="from">The items that are selected</param>
    /// <returns>True if the swap was successful</returns>
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.Item.GetType() != Item.GetType() || from.Count + Count > Item.StackSize)
        {
            //Copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.Items);

            //Clear Slot A
            from.Items.Clear();
            //All items from Slot B and copy them into A
            from.AddItems(Items);

            //Clear B
            Items.Clear();
            //Move the items from A Copy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates the slot size
    /// </summary>
    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }
}
