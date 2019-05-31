using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TProgress : Task
{
    /// <summary>
    /// The amount to be awarded upon completition
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
    /// Work on your event progress.
    /// </summary>
    public static string taskName
    {
        get { return "Work on your event progress."; }
    }
    /// <summary>
    /// Go to the computer and work on your event progress.
    /// </summary>
    public static string TaskDescription
    {
        get
        {
            return "Go to the computer and work on your event progress.";
        }
    }
    /// <summary>
    /// Progress.
    /// </summary>
    public static string ObjectiveName
    {
        get { return "Progress."; }
    }
    private static int progressAmount = 2;

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
        StatCompleted(v => { Player.Instance.Progress += v; }, rewardAmount);
    }

    /// <summary>
    /// Initializes
    /// </summary>
    public TProgress()
    {
        objectives = new List<Objective>() { new Objective(ObjectiveName, ++progressAmount) };
        rewardAmount = (Player.Instance.Debug) ? 10000 : 150;
    }
}
