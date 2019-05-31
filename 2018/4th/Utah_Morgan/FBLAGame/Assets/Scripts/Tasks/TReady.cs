using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TReady : Task
{    
    /// <summary>
    /// The amount that is awarded upon completion of the task
    /// </summary>
    int rewardAmount;
    
    /// <summary>
    /// 1
    /// </summary>
    public static int IncrementAmount
    {
        get { return 1; }
    }
    /// <summary>
    /// Sell Baby Murphy's Cards
    /// </summary>
    public static string TaskName
    {
        get { return "Ready for Event"; }
    }
    /// <summary>
    /// Go around and ask people to see if they want to buy Baby Murphy's Pizza Cards.
    /// After selling 3 of them, you will earn fundraising money.
    /// </summary>
    public static string TaskDescription
    {
        get
        {
            return "Go to Fuller and ask him when the next event is to go to " + GameManager.Instance.Stage;
        }
    }
    /// <summary>
    /// Cards to Sell
    /// </summary>
    public static string ObjectiveName
    {
        get { return "Talk to Fuller"; }
    }

    /// <summary>
    /// The description of the task.
    /// </summary>
    public override string Description
    {
        get
        {
            return TaskDescription;
        }
    }
    /// <summary>
    /// The name of the task.
    /// </summary>
    public override string Name
    {
        get
        {
            return TaskName;
        }
    }

    /// <summary>
    /// Called when the task is completed. Does not need to do anything, so it is empty
    /// </summary>
    public override void TaskCompleted()
    {
        
    }

    /// <summary>
    /// Initializes. Makes a new objective
    /// </summary>
    public TReady()
    {
        objectives = new List<Objective>() { new Objective(ObjectiveName, 1) };                
    }
}
