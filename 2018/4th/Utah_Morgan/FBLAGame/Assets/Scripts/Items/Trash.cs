using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IInteract
{    
    /// <summary>
    /// The description when the player gets close enough
    /// </summary>
    public string Description
    {
        get
        {
            return "pick up trash";
        }
    }

    /// <summary>
    /// Updates the trash task
    /// </summary>
    public void Interact()
    {
        TaskManager.Instance.AddProgress(TTrash.TaskName, TTrash.ObjectiveName, TTrash.IncrementAmount);
        GameObject.Find("TrashManager").GetComponent<ObjectPool>().ReturnObject(gameObject);
    }
    
}
