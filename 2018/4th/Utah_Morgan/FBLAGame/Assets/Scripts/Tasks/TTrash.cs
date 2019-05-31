using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTrash : Task
{
    /// <summary>
    /// The amount to be awarded upon completion
    /// </summary>
    int rewardAmount;
   
    /// <summary>
    /// Pick up trash
    /// </summary>
    public static string TaskName { get { return "Pick up trash"; } }
    /// <summary>
    /// Go out into the field and pick up trash
    /// </summary>
    public static string TaskDescription { get { return "Go out into the field and pick up trash"; } }
    /// <summary>
    /// Trash to pick up
    /// </summary>
    public static string ObjectiveName { get { return "Trash to pick up"; } }
    /// <summary>
    /// 1
    /// </summary>
    public static int IncrementAmount { get { return 1; } }

    /// <summary>
    /// The amount of trash to pick up
    /// </summary>
    public static int TotalAmount { get { return 3; } }

    /// <summary>
    /// The name of the task
    /// </summary>
    public override string Name
    {
        get
        {
            return TaskName;
        }
    }

    /// <summary>
    /// The description of the task
    /// </summary>
    public override string Description
    {
        get
        {
            return TaskDescription;
        }
    }

    /// <summary>
    /// Called when the task is completed. Updates stat
    /// </summary>
    public override void TaskCompleted()
    {
        StatCompleted(value => Player.Instance.Service += value, rewardAmount);
    }    

    /// <summary>
    /// Initializes
    /// </summary>
    public TTrash()
    {
        objectives = new List<Objective>() { new Objective(ObjectiveName, TotalAmount) };
        rewardAmount = (Player.Instance.Debug) ? 10000 : 150;
    }
}
