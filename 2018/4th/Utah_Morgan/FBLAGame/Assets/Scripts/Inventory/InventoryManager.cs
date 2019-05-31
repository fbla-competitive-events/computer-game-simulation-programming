using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    /// <summary>
    /// Singleton instance of the InventorManager class
    /// </summary>
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }

    private SlotScript fromSlot;

    [SerializeField]
    private BagButton[] bagButtons;

    //For debugging
    [SerializeField]
    private Item[] items;

    private List<Bag> bags = new List<Bag>();

    /// <summary>
    /// returns true if there are empty slots for a bag
    /// </summary>
    public bool CanAddBag
    {
        get { return bags.Count < bagButtons.Length; }
    }

    /// <summary>
    /// Gets the amount of empty slots in all of the bags combined
    /// </summary>
    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.BagScript.EmptySlotCount;
            }

            return count;
        }
    }

    /// <summary>
    /// The amount of slots that have items in them
    /// </summary>
    public int FullSlotCount
    {
        get
        {
            return TotalSlotCount - EmptySlotCount;
        }
    }

    /// <summary>
    /// The total amount of slots
    /// </summary>
    public int TotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.BagScript.Slots.Count;
            }

            return count;
        }
    }

    /// <summary>
    /// The slot that the currently selected item is from
    /// </summary>
    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.Icon.color = Color.grey;
            }
        }
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
    }

    /// <summary>
    /// Adds a bag to the bag button
    /// </summary>
    /// <param name="bag">The bag to add</param>
    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.Bag == null)
            {
                bagButton.Bag = bag;
                bags.Add(bag);
                bag.BagButton = bagButton;
                break;
            }
        }
        
    }

    /// <summary>
    /// Adds a bag to the bag button
    /// </summary>
    /// <param name="bag">The bag to add</param>
    /// <param name="bagButton">The button to add the bag to </param>
    public void AddBag(Bag bag, BagButton bagButton)
    {
        bags.Add(bag);
        bagButton.Bag = bag;
    }

    /// <summary>
    /// Removes a bag from a bag button
    /// </summary>
    /// <param name="bag">The bag to remove</param>
    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.BagScript.gameObject);
    }

    /// <summary>
    /// Swaps two bags
    /// </summary>
    /// <param name="oldBag">The old bag</param>
    /// <param name="newBag">The new bag</param>
    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = TotalSlotCount - oldBag.Slots + newBag.Slots;

        if (newSlotCount - FullSlotCount >= 0)
        {
            //Do swap
            List<Item> bagItems = oldBag.BagScript.GetItems();

            RemoveBag(oldBag);
            newBag.BagButton = oldBag.BagButton;
            newBag.Use();

            foreach (Item item in bagItems)
            {
                if (item != newBag)
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);
            HandScript.Instance.Drop();
            instance.fromSlot = null;
        }
    }

    /// <summary>
    /// Add an item to a bag
    /// </summary>
    /// <param name="item">The item to add</param>
    public void AddItem(Item item)
    {
        if (item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }

        PlaceInEmpty(item);
    }

    /// <summary>
    /// Places a item in an empty slot
    /// </summary>
    /// <param name="item">The item to place</param>
    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.BagScript.AddItem(item))
            {
                return;
            }
        }
    }

    /// <summary>
    /// Place an item in a slot that already has its own item in it
    /// </summary>
    /// <param name="item">The item to place</param>
    /// <returns>True if it was successfully placed</returns>
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slots in bag.BagScript.Slots)
            {
                if (slots.StackItem(item))
                {
                    return true;
                }
            }
        }

        return false;
    }    
    
    /// <summary>
    /// Opens or closes a bag
    /// </summary>
    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.BagScript.IsOpen);

        foreach (Bag bag in bags)
        {
            if (bag.BagScript.IsOpen != closedBag)
            {
                bag.BagScript.OpenClose();
            }
        }
    }

    /// <summary>
    /// Gets all of the items of given type
    /// </summary>
    /// <param name="type">The type of item to return</param>
    /// <returns>The list of IUseables</returns>
    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> usables = new Stack<IUseable>();

        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.BagScript.Slots)
            {
                if (!slot.IsEmpty && slot.Item.GetType() == type.GetType())
                {
                    foreach (Item item in slot.Items)
                    {
                        usables.Push(item as IUseable);
                    }
                }
            }
        }

        return usables;
    }
}
