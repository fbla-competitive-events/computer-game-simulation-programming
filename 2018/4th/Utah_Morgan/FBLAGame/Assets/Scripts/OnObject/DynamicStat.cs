using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DynamicStat : Stat
{

    int maxLevel;
    int level;
    int nextLevel;    

    /// <summary>
    /// The stat's current level.
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            //Clamps the level
            if (value < 0)
            {
                level = 0;
            }
            else if (value > MaxLevel)
            {
                level = MaxLevel;
            }
            else
            {  
                //If the value is going up, then display a scrolling text
                if (value > level && value != 1)
                {
                    ScrollingTextManager.Instance.CreatText(Player.Instance.transform.position, Color.white, string.Format("+{0} {1} Lv.", value - level, Text), true);
                }                    
                    
                level = value; 
            }
        }
    }
    /// <summary>
    /// The stat's current value. Changes the level if the current value equals the max value
    /// </summary>
    public override float CurrentValue
    {
        get
        {
            return base.CurrentValue;
        }

        set
        {
            //Displays a scrolling text if there is added stat                
            if (value - CurrentValue > 0)
                ScrollingTextManager.Instance.CreatText(Player.Instance.transform.position, Color.white, string.Format("+{0} {1}", value - CurrentValue, name), false);

            if (value >= MaxValue && Level != MaxLevel)
            {
                //Gets what the max value was before it changed
                float temp = MaxValue;

                //Increments the level and max value
                Level += 1;
                MaxValue += nextLevel;

                //If the amount rewarded is over the max value, subtract the max value from the amount rewarded
                //For example, if we are awarded 150 but the max is 100, we should now have 50 with an upgraded level
                CurrentValue = value - temp;
            }
            else
            {
                base.CurrentValue = value;
            }
        }
    }

    /// <summary>
    /// The max level that the stat can achieve. Should only be changed by GameManager
    /// </summary>
    public int MaxLevel
    {
        get
        {
            return maxLevel;
        }

        set
        {
            if (value < 0)
            {
                maxLevel = 0;
            }
            else
            {
                maxLevel = value;
            }            
        }
    }


    /// <summary>
    /// Initializes the class with the given values.
    /// </summary>
    /// <param name="currentValue">The value the stat starts at.</param>
    /// <param name="MaxValue">The max value the stat can achieve until a level up.</param>
    /// <param name="Level">The current level the stat is at.</param>
    /// <param name="MaxLevel">The max level the stat can achieve.</param>
    /// <param name="NextLevel">At what increment will the max value have per level up.</param>
    public void Initialize(float currentValue, float MaxValue, int Level, int MaxLevel, int NextLevel)
    {
        base.Initialize(currentValue, MaxValue);
        this.MaxLevel = MaxLevel;
        this.Level = Level;
        this.nextLevel = NextLevel;
    }

    /// <summary>
    /// Checks to see if the given amount will make the stat change levels.
    /// </summary>
    /// <param name="amount">The amount to check.</param>
    /// <returns>True if a new level will occur, false if a new level will not occur.</returns>
    public bool WillChangeLevel(float amount)
    {
        return (amount >= MaxValue && Level != MaxLevel);            
    }

    ///<summary>
    ///Sets how the text is displayed
    ///</summary>  
    protected override void SetNameText()
    {
        Text = "Lv " + Level + "  " + CurrentValue + " / " + MaxValue;
    }
    
    ///<summary>
    ///Two stats are equal if their text is equal
    ///</summary>   
    public static bool operator ==(DynamicStat o, string s)
    {
        return (o.ToString() == s);
    }
    public static bool operator !=(DynamicStat o, string s)
    {
        return (o.ToString() == s);
    }
}
