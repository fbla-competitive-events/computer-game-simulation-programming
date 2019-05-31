using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag",order =1)]
public class Bag : Item, IUseable
{
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    /// <summary>
    /// The script this bag item is associated with
    /// </summary>
    public BagScript BagScript { get; set; }

    /// <summary>
    /// The button the bag sits on
    /// </summary>
    public BagButton BagButton { get; set; }

    /// <summary>
    /// Returns the number of slots in this bag
    /// </summary>
    public int Slots
    {
        get
        {
            return slots;
        }
    }

    /// <summary>
    /// Initializes a bag
    /// </summary>
    /// <param name="slots">The number of slots to have</param>
    public void Initialize(int slots)
    {
        this.slots = slots;
    }
    
    /// <summary>
    /// Uses the bag: makes a new invetory for this bag
    /// </summary>
    public void Use()
    {
        if (InventoryManager.Instance.CanAddBag)
        {
            Remove();
            BagScript = Instantiate(bagPrefab, InventoryManager.Instance.transform).GetComponent<BagScript>();
            BagScript.AddSlots(slots);

            if (BagButton == null)
            {
                InventoryManager.Instance.AddBag(this);
            }
            else
            {
                InventoryManager.Instance.AddBag(this, BagButton);
            }

            InventoryManager.Instance.AddBag(this);
        }       
    }   
}
