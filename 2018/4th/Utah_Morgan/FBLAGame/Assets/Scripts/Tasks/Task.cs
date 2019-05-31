using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task
{
#region Fields only pertanent to a custom built task

    /// <summary>
    /// The stat that the task will update upon completion
    /// </summary>
    private DynamicStat stat;

    

    
    /// <summary>
    /// The amount of stat that the task awards
    /// </summary>
    int rewardAmount;    

    /// <summary>
    /// The name of the task
    /// </summary>
    string statName;
    #endregion

    /// <summary>
    /// The name of the task
    /// </summary>
    public virtual string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }    
    string name;

    /// <summary>
    /// The description of the task
    /// </summary>
    public virtual string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }
    string description;

    /// <summary>
    /// A reference to the button this task is associated with
    /// </summary>
    public TaskButton TaskButton { get; set; }

    /// <summary>
    /// A list of objectives needed to complete the task
    /// </summary>
    protected List<Objective> objectives { get; set; }


    /// <summary>
    /// The objective text
    /// </summary>
    public string Objectives
    {
        get
        {
            string str = null;
            foreach (Objective ob in objectives)
            {
                str += String.Format("{0} {1}/{2}\n", ob.Name, ob.Progress, ob.Max);
            }
            return str;
        }
    }

    /// <summary>
    /// If the user can abandon this task
    /// </summary>
    public virtual bool CanAbandon { get; protected set; }   

    /// <summary>
    /// This constructer is called if Task is being inherited.
    /// </summary>
    public Task()
    {        
        CanAbandon = true;
    }

    /// <summary>
    /// Called whenever a custom task is made.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="objectives">A list of the objectives needed to complete a task.</param>
    /// <param name="statName">The name of the stat that the reward will go to.</param>
    /// <param name="rewardAmount">The amount of the reward upon completion of the task.</param>
    /// <param name="rewardAnimation">
    /// The animation that will play upon completion of the task.
    /// </param>
    public Task(string name, string description, List<Objective> objectives, string statName, int rewardAmount, string rewardAnimation)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("DynamicStat");
        List<DynamicStat> statList = new List<DynamicStat>();
        foreach (GameObject go in array)
        {
            statList.Add(go.GetComponent<DynamicStat>());
        }

        
        Predicate<DynamicStat> match = x => x == statName;
        if (!statList.Exists(match)) 
        {
            throw new System.Exception(string.Format("The stat name {0} does not exist.", statName));
        }
        stat = statList.Find(match);


        stat = new List<GameObject>(GameObject.FindGameObjectsWithTag("DynamicStat")).Find(x => x.name == statName).GetComponent<DynamicStat>();        
        Name = name;
        Description = description;
        this.objectives = objectives;
        this.rewardAmount = rewardAmount;        
        this.statName = statName;

    }

    /// <summary>
    /// Gets the current progress of the task
    /// </summary>
    /// <param name="objective">The objective to check progress</param>
    /// <returns></returns>
    public int GetProgress(string objective)
    {
        Objective _objective = getObjective(objective);
        if (_objective != null)
        {
            return _objective.Progress;
        }
        throw new Exception();
    }

    /// <summary>
    /// Gets the amount needed to complete an objective
    /// </summary>
    /// <param name="objective">The objective to check</param>
    /// <returns></returns>
    public int GetMax(string objective)
    {
        Objective _objective = getObjective(objective);
        if (_objective != null)
        {
            return _objective.Max;
        }
        else
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// Sets the progress of an objective
    /// </summary>
    /// <param name="objectiveName">The objective to set</param>
    /// <param name="amount">The amount to set it to</param>
    /// <returns></returns>
    public bool SetProgress(string objectiveName, int amount)
    {
        Objective objective = getObjective(objectiveName);
        if (objective != null)
        {            
            objective.Progress = amount;
            return IsTaskComplete();
        }
        return false;
    }

    /// <summary>
    /// Adds progress of an objective 
    /// </summary>
    /// <param name="objectiveName">The objective to add</param>
    /// <param name="amount">The amount to add</param>
    /// <returns></returns>
    public virtual bool AddProgress(string objectiveName, int amount)
    {
        Objective objective = getObjective(objectiveName);
        if (objective != null)
        {            
            objective.Progress += amount;
            return IsTaskComplete();
        }

        //If the objective is null, return false
        return false;
    }

    /// <summary>
    /// Gets an objective given a name
    /// </summary>
    /// <param name="name">The name of an objective</param>
    /// <returns>The objective</returns>
    Objective getObjective(string name)
    {
        Predicate<Objective> match = x => x.ToString() == name;
        if (!objectives.Exists(match))
            return null;
        return objectives.Find(match);
    }

    /// <summary>
    /// When the task is completed
    /// </summary>
    public virtual void TaskCompleted()
    {
        StatCompleted(value => 
        {
            if (statName == "Membership")
                Player.Instance.Membership += value;
            else if (statName == "Fundraising")
                Player.Instance.Fundraising += value;
            else if (statName == "Education")
                Player.Instance.Education += value;
            else if (statName == "Service")
                Player.Instance.Service += value;
            else
                Player.Instance.Progress += value;
        }, rewardAmount);
    }

    /// <summary>
    /// This is the generic method that is called when a task is completed. 
    /// It increments a stat on the player.
    /// </summary>
    /// <param name="stat">The stat that needs to be incremented.</param>
    /// <param name="rewardAmount">The amount to be incremented.</param>    
    protected void StatCompleted(Action<float> stat, int rewardAmount)
    {
        updateReward(stat, rewardAmount);        
    }

    /// <summary>
    /// Updates the reward of a stat
    /// </summary>
    /// <param name="stat">A delegate having the reward to update</param>
    /// <param name="amount">The amount to update</param>
    void updateReward(Action<float> stat, int amount)
    {
        stat(amount);            
    }

    /// <summary>
    /// Sees if the task is completed by looping through all of the objectives
    /// </summary>
    /// <returns></returns>
    public bool IsTaskComplete()
    {
        foreach(Objective obj in objectives)
        {
            if (obj.Progress < obj.Max)
                return false;
        }
        TaskCompleted();
        return true;
    }

    /// <summary>
    /// Converts this to a string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Name;
    }

}
