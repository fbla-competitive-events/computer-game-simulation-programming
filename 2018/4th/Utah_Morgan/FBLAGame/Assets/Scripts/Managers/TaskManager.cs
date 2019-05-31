using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : UIBox
{
    /// <summary>
    /// The single ton instance of the task manager
    /// </summary>
    public static TaskManager Instance
    {
        get
        {
            return FindObjectOfType<TaskManager>();
        }
    }

    /// <summary>
    /// The list of tasks currently set
    /// </summary>
    private List<Task> tasks;
    
    /// <summary>
    /// The object pool for the task buttons
    /// </summary>
    [SerializeField]
    private ObjectPool buttonPool;

    /// <summary>
    /// The parent of the task buttons
    /// </summary>
    [SerializeField]
    private Transform contentPanel;

    /// <summary>
    /// A reference to the description text
    /// </summary>
    [SerializeField]
    private Text descriptionText;

    /// <summary>
    /// Used to update the scroll position of the description scroll bar
    /// </summary>
    [SerializeField]
    private Scrollbar descriptionScrollBar;

    /// <summary>
    /// Used to update the scroll position of the task scroll bar
    /// </summary>
    [SerializeField]
    private Scrollbar taskScrollBar;


    /// <summary>
    /// Tells if the task box is active or not
    /// </summary>
    private bool Active
    {
        get { return GetComponent<CanvasGroup>().blocksRaycasts; }        
    }

    /// <summary>
    /// Which task has its description being displayed
    /// </summary>
    public Task Focus
    {
        get
        {
            return focus;
        }
        set
        {
            if (focus != null)
            {
                focus.TaskButton.GetComponent<Image>().color = focus.TaskButton.GetComponent<Button>().colors.normalColor;
            }
            focus = value;
            focus.TaskButton.GetComponent<Image>().color = focus.TaskButton.GetComponent<Button>().colors.highlightedColor;

            //Set the scroll bar to the top
            descriptionScrollBar.value = 1;

            //Updates the position of the task scrollbar based on what is selected
            taskScrollBar.value = (tasks.Count - tasks.IndexOf(value) - 1.0f) / (tasks.Count - 1.0f);

            //Set the description               
            setDescription(value);
        }
    }
    private Task focus;

    /// <summary>
    /// Used so the tasks are initialized before any start function is called to prevent other classes trying to access it before it is initialized
    /// </summary>
    private void Awake()
    {
        tasks = new List<Task>();        
    }

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {                      
        refreshDisplay();        
    }

    /// <summary>
    /// Sets the progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The type of the task to set the progress.</param>
    /// <param name="objectiveName">The name of the objective to set the progress.</param>
    /// <param name="amount">How much progress the objective needs to be set.</param>
    public void SetProgress(Type task, string objectiveName, int amount)
    {
        SetProgress(getTask(task), objectiveName, amount);
    }

    /// <summary>
    /// Sets the progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The type of the task to set the progress.</param>
    /// <param name="objective">The objective to set the progress.</param>
    /// <param name="amount">How much progress the objective needs to be set.</param>
    public void SetProgress(Type task, Objective objective, int amount)
    {
        SetProgress(task, objective.Name, amount);
    }

    /// <summary>
    /// Sets the progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The task to set the progress.</param>
    /// <param name="objective">The objective to set the progress.</param>
    /// <param name="amount">How much progress the objective needs to be set.</param>
    public void SetProgress(Task task, Objective objective, int amount)
    {
        if (objective != null)
        {
            SetProgress(task, objective.Name, amount);
        }        
    }

    /// <summary>
    /// Sets the progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The task to set the progress.</param>
    /// <param name="objectiveName">The name of the objective to set the progress.</param>
    /// <param name="amount">How much progress the objective needs to be set.</param>
    public void SetProgress(Task task, string objectiveName, int amount)
    {
        if (task != null)
        {
            SetProgress(task.Name, objectiveName, amount);
        }        
    }

    /// <summary>
    /// Sets the progress to a given objective in a given task.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="objectiveName">The name of the objective in the task.</param>
    /// <param name="amount">How much progress the objective needs to be set.</param>
    public void SetProgress(string taskName, string objectiveName, int amount)
    {
        //Gets the task associated with 'name'
        Task task = getTask(taskName);

        //If the given name is actually a task
        if (task != null)
        {
            //Add the progress, and if the task is completed, then call the completed method associated with the task
            if (task.SetProgress(objectiveName, amount))
            {                
                removeTask(task);
            }
        }
    }

    /// <summary>
    /// Increments progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The type of the task to increment progress.</param>
    /// <param name="objectiveName">The name of the objective to increment progress.</param>
    /// <param name="amount">How much progress the objective needs to be incremented.</param>
    public void AddProgress(Type task, string objectiveName, int amount)        
    {
        AddProgress(getTask(task), objectiveName, amount);
    }

    /// <summary>
    /// Increments progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The type of the task to increment progress.</param>
    /// <param name="objective">The objective to increment progress.</param>
    /// <param name="amount">How much progress the objective needs to be incremented.</param>
    public void AddProgress(Type task, Objective objective, int amount)
    {
        AddProgress(task, objective.Name, amount);
    }

    /// <summary>
    /// Increments progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The task to increment progress.</param>
    /// <param name="objective">The objective to increment progress.</param>
    /// <param name="amount">How much progress the objective needs to be incremented.</param>
    public void AddProgress(Task task, Objective objective, int amount)
    {
        if (objective != null)
        {
            AddProgress(task.Name, objective.Name, amount);
        }
        
    }

    /// <summary>
    /// Increments progress to a given objective in a given task.
    /// </summary>
    /// <param name="task">The task to increment progress.</param>
    /// <param name="objectiveName">The name of the objective to increment progress.</param>
    /// <param name="amount">How much progress the objective needs to be incremented.</param>
    public void AddProgress(Task task, string objectiveName, int amount)
    {
        if (task != null)
        {
            AddProgress(task.Name, objectiveName, amount);
        }        
    }

    /// <summary>
    /// Increments progress to a given objective in a given task.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="objectiveName">The name of the objective in the task.</param>
    /// <param name="amount">How much progress the objective needs to be incremented.</param>
    public void AddProgress(string name, string objectiveName, int amount)
    {
        //Gets the task associated with 'name'
        Task task = getTask(name);

        //If the given name is actually a task
        if (task != null)
        {                        
            //Add the progress, and if the task is completed, then call the completed method associated with the task
            if (task.AddProgress(objectiveName, amount))
            {                
                removeTask(task);
            }
        }
        
    }



    /// <summary>
    /// Sets a new task with objectives.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="objectives">A list of the objectives needed to complete a task.</param>
    /// <param name="statName">The name of the stat that the reward will go to.</param>
    /// <param name="rewardAmount">The amount of the reward upon completion of the task.</param>
    /// <param name="rewardAnimation">
    /// The animation that will play upon completion of the task. DO NOT PUT NULL: if there is no animation, put " ";
    /// </param>
    public void NewTask(string name, string description, List<Objective> objectives, string statName, int rewardAmount, string rewardAnimation)
    {
        NewTask(new Task(name, description, objectives, statName, rewardAmount, rewardAnimation));
    }

    /// <summary>
    /// Sets a new task with objectives.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="objective">
    /// A objective needed to complete a task. If there is more than one objective, pass in a list of objectives.
    /// </param>
    /// <param name="statName">The name of the stat that the reward will go to.</param>
    /// <param name="rewardAmount">The amount of the reward upon completion of the task.</param>
    /// <param name="rewardAnimation">
    /// The animation that will play upon completion of the task. DO NOT PUT NULL: if there is no animation, put " ";
    /// </param>
    public void NewTask(string name, string description, Objective objective, string statName, int rewardAmount, string rewardAnimation)
    {       
        NewTask(name, description, new List<Objective>() { objective }, statName, rewardAmount, rewardAnimation);        
    }

    /// <summary>
    /// Sets a new task with objectives.
    /// </summary>
    /// <param name="task">Reference to the new task.</param>
    public void NewTask(Task task)
    {
        //Only add the task if it does not already have it
        if (!ContainsTask(task.GetType()))
        {
            tasks.Add(task);
            if (task.IsTaskComplete())
            {
                removeTask(task);
            }            
            refreshDisplay();
        }
    }

    /// <summary>
    /// Returns the progress of a given objective in a given task.
    /// </summary>
    /// <param name="name">Name of the task.</param>
    /// <param name="objective">Name of the objective.</param>
    /// <param name="progress">The progress of the called objective.</param>
    /// <returns>Bool: true if an objective was found, and false if an objective was not found.</returns>
    public bool GetProgress(string name, string objective, out int progress)
    {
        progress = 0;
        if (!ContainsTask(name))
            return false;
        Task task = getTask(name);                
        progress = task.GetProgress(objective);
        return true;               
    }    

    /// <summary>
    /// Gets the amount of progress the player needs to do before completion of an objective
    /// </summary>
    /// <param name="name">The name of the task to check</param>
    /// <param name="objective">The name of the objective to check</param>
    /// <param name="progressLeft">How much progress is left</param>
    /// <returns>True if the task exists</returns>
    public bool GetProgressLeft(string name, string objective, out int progressLeft)
    {
        progressLeft = 0;
        if (!ContainsTask(name))
            return false;
        Task task = getTask(name);
        progressLeft = task.GetMax(objective) - task.GetProgress(objective);
        return true;
    }

    /// <summary>
    /// Checks to see if a task with the given name is contained in the list of active tasks.
    /// </summary>
    /// <param name="name">The name of the class to be checked</param>
    /// <returns>True if the list 'tasks' does contain the task, false if not</returns>
    public bool ContainsTask(string name)
    {
        return (ContainsTask(getTask(name)));
    }
    /// <summary>
    /// Checks to see if a given task is contained in the list of active tasks.
    /// </summary>
    /// <param name="task">The task to check</param>
    /// <returns>True if the list 'tasks' does contain the task, false if not</returns>
    public bool ContainsTask(Task task)
    {
        return (tasks.Contains(task));
    }

    /// <summary>
    /// Checks to see if a given type of a task is contained in the list of active tasks.
    /// </summary>
    /// <param name="task">The task to check</param>
    /// <returns>True if the list 'tasks' does contain the task, false if not</returns>
    public bool ContainsTask(Type task)
    {
        return (ContainsTask(getTask(task)));
    }

    /// <summary>
    /// Gets a task given a name
    /// </summary>
    /// <param name="name">The name of the desired task</param>
    /// <returns>The task</returns>
    Task getTask(string name)
    {
        Predicate<Task> match = (x => x.ToString() == name);
        return getTask(match);
        
    }

    /// <summary>
    /// Gets a task given the type of a task
    /// </summary>
    /// <param name="type">The type of the desired task</param>
    /// <returns>The task</returns>
    Task getTask(Type type)
    {
        //The exception says it all
        if (type == typeof(Task))
        {
            throw new Exception("Can only use getTask(Type type) if type is not a type of 'Task'");
        }

        Predicate<Task> match = x => x.GetType() == type;
        return getTask(match);
    }

    /// <summary>
    /// Gets a task given where the task is in the list
    /// </summary>
    /// <param name="match">Where the task is in the list</param>
    /// <returns>The task</returns>
    Task getTask(Predicate<Task> match)
    {
        if (!tasks.Exists(match))
            return null;
        return tasks.Find(match);
    }

    /// <summary>
    /// Refreshes the buttons and descriptions
    /// </summary>
    private void refreshDisplay()
    {
        removeButtons();
        addButtons();
        setKeyboardStrokeForButtons();
        clearDescription();
        
        if (tasks.Count > 0)
        {
            //If we have a task, make the first one the focus
            Focus = tasks[0];

            //Update the keyboard stroke for the description scroll bar
            Navigation navagation = new Navigation();
            navagation.mode = Navigation.Mode.Explicit;
            navagation.selectOnLeft = tasks[0].TaskButton.GetComponent<Button>();
            navagation.selectOnRight = descriptionScrollBar.navigation.selectOnRight;
            descriptionScrollBar.navigation = navagation;
        }
            
    }

    /// <summary>
    /// Add the buttons of all of the current tasks
    /// </summary>
    private void addButtons()
    {
        foreach (Task task in tasks)
        {            
            GameObject newButton = buttonPool.GetObject();            

            newButton.transform.SetParent(contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.GetComponent<TaskButton>().Setup(task);
        }
        
    }

    /// <summary>
    /// Sets the keyboard stroke for the task buttons
    /// </summary>
    private void setKeyboardStrokeForButtons()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            //Allows for pure, genuine keyboard stroke
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;

            //Wraps selection around to the last item
            if (i == 0)
            {
                navigation.selectOnUp = tasks[tasks.Count - 1].TaskButton.GetComponent<Button>();
            }
            else
            {
                navigation.selectOnUp = tasks[i - 1].TaskButton.GetComponent<Button>();
            }

            //Wraps selection around to the first item
            if (i == tasks.Count - 1)
            {
                navigation.selectOnDown = tasks[0].TaskButton.GetComponent<Button>();

            }
            else
            {
                navigation.selectOnDown = tasks[i + 1].TaskButton.GetComponent<Button>();
            }

            navigation.selectOnRight = navigation.selectOnLeft = descriptionScrollBar;

            tasks[i].TaskButton.GetComponent<Button>().navigation = navigation;
        }
    }

    /// <summary>
    /// Remove all of the task buttons
    /// </summary>
    private void removeButtons()
    {
        while (contentPanel.childCount > 0)
        {
            buttonPool.ReturnObject(contentPanel.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Removes a given task from the list of tasks
    /// </summary>
    /// <param name="task">Task to remove</param>
    private void removeTask(Task task)
    {
        tasks.Remove(task);
        if (task == Focus)
            Focus = null;
        refreshDisplay();
    }

    /// <summary>
    /// Sets the description text given a task
    /// </summary>
    /// <param name="task">The task of the wanted set description</param>
    private void setDescription(Task task)
    {
        if (task == null) return;
        descriptionText.text = String.Format("{0}\n\n{1}", task.Description, task.Objectives);
    }

    /// <summary>
    /// Clears the description text
    /// </summary>
    private void clearDescription()
    {
        descriptionText.text = null;
    }

    /// <summary>
    /// Called when the user clicks the abandon button
    /// </summary>
    public void AbandonButtonClick()
    {
        if (Focus != null && Active && Focus.CanAbandon)
            removeTask(Focus);
    }
    	
	/// <summary>
    /// Called once per frame
    /// </summary>
	void Update ()
    {
        setDescription(Focus);
	}
}
