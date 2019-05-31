using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskButton : MonoBehaviour, ISelectHandler
{
    /// <summary>
    /// The name of the task
    /// </summary>
    private Text nameText;

    /// <summary>
    /// The task that is being displayed
    /// </summary>
    private Task task;

    /// <summary>
    /// The description of the task
    /// </summary>
    [SerializeField]
    private Text descriptionText;

    /// <summary>
    /// The objectives of the task
    /// </summary>
    [SerializeField]
    private Text objectivesText;    
       	
    /// <summary>
    /// Initializes the task button
    /// </summary>
    /// <param name="task">The task to display</param>    
	public void Setup(Task task)
    {
        nameText = transform.GetChild(0).GetComponent<Text>();
        GetComponent<Button>().onClick.AddListener(HandleCick);
        descriptionText = TaskManager.Instance.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

        task.TaskButton = this;
        this.task = task;
        nameText.text = task.Name;        
    }

    /// <summary>
    /// Called when the task button is clicked
    /// </summary>
    public void HandleCick()
    {        
        TaskManager.Instance.Focus = task;
    }

    /// <summary>
    /// Clears the description text
    /// </summary>
    public void ClearDescription()
    {
        Debug.Log("Clearing Text");
        descriptionText.text = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleCick();        
    }
}
