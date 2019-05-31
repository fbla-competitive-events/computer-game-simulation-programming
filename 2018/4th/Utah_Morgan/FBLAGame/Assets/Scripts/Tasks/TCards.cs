using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCards : Task
{
    /// <summary>
    /// The amount awarded upon completion of the task
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
    public static string taskName
    {
        get { return "Sell Baby Murphy's Cards"; }
    }
    /// <summary>
    /// Go around and ask people to see if they want to buy Baby Murphy's Pizza Cards.
    /// After selling 3 of them, you will earn fundraising money.
    /// </summary>
    public static string TaskDescription { get { return String.Format("Go around and ask people to see if they want to buy Baby Murphy's Pizza cards." +
            "After selling {0} of them, you will earn fundraising money.", 3); } }
    /// <summary>
    /// Cards to Sell
    /// </summary>
    public static string ObjectiveName
    {
        get { return "Cards to Sell"; }
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
            return taskName;
        }        
    }
    /// <summary>
    /// Called when the task is completed. Applys the reward to the stat "Fundraiser" and plays the "Money" animation.
    /// </summary>
    public override void TaskCompleted()
    {
        StatCompleted(value => Player.Instance.Fundraising += value, rewardAmount);
    }    

    /// <summary>
    /// Constructor used for initialization
    /// </summary>
    public TCards()
    {                        
        objectives = new List<Objective>() { new Objective(ObjectiveName, 3) };
        rewardAmount = (Player.Instance.Debug) ? 10000 : 150;        
    }   
}
