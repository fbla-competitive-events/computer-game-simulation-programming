using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEvent : Task
{    
    /// <summary>
    /// The current stage in the game
    /// </summary>
    private Stage stage;

    /// <summary>
    /// Prepare for the next event by leveling up on different tasks. Go to Fuller to know how to level up these tasks.
    /// </summary>
    public static string TaskDescription
    {
        get
        {
            return "Prepare for the next event by leveling up on different tasks. Go to Fuller to know how to level up these tasks.";
        }
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
            string sStage = null;
            if (stage == Stage.Region)
                sStage = "Region";
            else if (stage == Stage.State)
                sStage = "State";
            else
                sStage = "Nationals";
            return "Prepare for " + sStage;
        }
    }

    /// <summary>
    /// Called when the task is completed. Have the game manager decide what to do next
    /// </summary>
    public override void TaskCompleted()
    {
        GameManager.Instance.EventTaskComplete();
    }

    /// <summary>
    /// Initializes the data
    /// </summary>
    /// <param name="stage">The current stage</param>
    public TEvent(Stage stage) 
    {        
        this.stage = stage;
        objectives = getObjectives(stage);                
        CanAbandon = false;
    }
    
    /// <summary>
    /// Makes a list of objectives given the stage we are in
    /// </summary>
    /// <param name="stage">The current stage</param>
    /// <returns>List of objectives to complete before allowed to compete</returns>
    private List<Objective> getObjectives(Stage stage)
    {
        List<Objective> l = new List<Objective>();
        switch ((int)stage)
        {
            case 1:
                l.Add(new Objective("Fundraising Level", 2, Player.Instance.FundraisingLevel));
                l.Add(new Objective("Service Level", 2, Player.Instance.ServiceLevel));
                l.Add(new Objective("Progress Level", 2, Player.Instance.ProgressLevel));
                l.Add(new Objective("Education Level", 2, Player.Instance.EducationLevel));
                break;
            case 2:
                l.Add(new Objective("Fundraising Level", 3, Player.Instance.FundraisingLevel));
                l.Add(new Objective("Service Level", 3, Player.Instance.ServiceLevel));
                l.Add(new Objective("Progress Level", 3, Player.Instance.ProgressLevel));
                l.Add(new Objective("Membership Level", 2, Player.Instance.MembershipLevel));
                l.Add(new Objective("Education Level", 3, Player.Instance.EducationLevel));
                break;
            case 3:
                l.Add(new Objective("Fundraising Level", 4, Player.Instance.FundraisingLevel));
                l.Add(new Objective("Service Level", 4, Player.Instance.ServiceLevel));
                l.Add(new Objective("Progress Level", 4, Player.Instance.ProgressLevel));
                l.Add(new Objective("Membership Level", 3, Player.Instance.MembershipLevel));
                l.Add(new Objective("Education Level", 4, Player.Instance.EducationLevel));
                break;
            default: break;
        }
        
        return l;
    }
}