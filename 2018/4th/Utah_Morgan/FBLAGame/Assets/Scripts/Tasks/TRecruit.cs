using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRecruit : Task
{    
    /// <summary>
    /// The amount to be awarded upon completion
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
    /// Recruit Membership Program members.
    /// </summary>
    public static string taskName
    {
        get { return "Recruit Membership Program members."; }
    }
    /// <summary>
    /// Go around and ask people to see if they want to buy Baby Murphy's Pizza Cards.
    /// After selling 3 of them, you will earn fundraising money.
    /// </summary>
    public static string TaskDescription
    {
        get
        {
            return "Go around to classmates and ask them if they want to join the membership program.";
        }
    }
    /// <summary>
    /// Recruited members.
    /// </summary>
    public static string ObjectiveName
    {
        get { return "Recruited members."; }
    }
    private static int amountToRecruit = 2;

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
    /// Called when the task is completed. Applys the reward to the stat "Membership" and plays the "Money" animation.
    /// </summary>
    public override void TaskCompleted()
    {
          StatCompleted(  v => { Player.Instance.Membership += v; } , rewardAmount);
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public TRecruit()
    {
        objectives = new List<Objective>() { new Objective(ObjectiveName, ++amountToRecruit) };
        rewardAmount = (Player.Instance.Debug) ? 10000 : 150;
    }
}
