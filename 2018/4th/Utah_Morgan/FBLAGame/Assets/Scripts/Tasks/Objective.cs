using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    ///<summary>
    ///The name of the objective
    ///</summary>        
    public string Name
    {
        get { return name; }
        private set { name = value; }
    }
    private string name;

    /// <summary>
    /// How much of the objective is finished
    /// </summary>
    public int Progress
    {
        get
        {
            return progress;
        }
        set
        {
            if (value < 0)
            {
                progress = 0;
            }
            else if (value > Max)
            {
                progress = Max;
            }
            else
            {
                progress = value;
            }
        }
    }
    private int progress;

    /// <summary>
    /// When will the objective be completed
    /// </summary>
    public int Max
    {
        get
        {
            return max;
        }

        set
        {
            max = value;
        }
    }
    private int max;

    /// <summary>
    /// Initializes
    /// </summary>
    /// <param name="name"></param>
    /// <param name="max"></param>
    /// <param name="progress"></param>
    public Objective (string name, int max, int progress = 0)
    {
        this.max = max;            
        this.name = name;
        this.progress = progress;
    }    

    /// <summary>
    /// For easy debugging
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Name;
    }    
}
