using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Movement
{
    /// <summary>
    /// Single ton instance of the player
    /// </summary>
    public static Player Instance
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    /// <summary>
    /// The fundraising stat
    /// </summary>
    [SerializeField]
    private DynamicStat fundraising;

    /// <summary>
    /// The membership stat
    /// </summary>
    [SerializeField]
    private DynamicStat membership;

    /// <summary>
    /// The service stat
    /// </summary>
    [SerializeField]
    private DynamicStat service;

    /// <summary>
    /// The education stat
    /// </summary>
    [SerializeField]
    private DynamicStat education;

    /// <summary>
    /// The progress stat
    /// </summary>
    [SerializeField]
    private DynamicStat progress;

    /// <summary>
    /// The experience stat
    /// </summary>
    [SerializeField]
    private Stat experience;

    /// <summary>
    /// Used for debugging
    /// </summary>
    [SerializeField]
    public bool Debug;
    
    /// <summary>
    /// The object currently interacting with
    /// </summary>
    public IInteract InteractObject { get; set; }
       
    /// <summary>
    /// The min tile that the player can walk on
    /// </summary>
    Vector3 minTile;

    /// <summary>
    /// The max tile the player can walk on
    /// </summary>
    Vector3 maxTile;
    
    /// <summary>
    /// The interact description text
    /// </summary>
    [SerializeField]
    GameObject interactText;       

    /// <summary>
    /// The amount of fundraising the player has
    /// </summary>
    public float Fundraising
    {
        get
        {
            return fundraising.CurrentValue;
        }
        set
        {
            updateEventTask(ref fundraising, value);
        }
    }

    /// <summary>
    /// The amount of membership the player has
    /// </summary>
    public float Membership
    {
        get
        {
            return membership.CurrentValue;
        }
        set
        {
            updateEventTask(ref membership, value);
        }
    }

    /// <summary>
    /// The amount of education the player has
    /// </summary>
    public float Education
    {
        get
        {
            return education.CurrentValue;
        }
        set
        {
            updateEventTask(ref education, value);
        }
    }

    /// <summary>
    /// The amount of service the player has
    /// </summary>
    public float Service
    {
        get
        {
            return service.CurrentValue;
        }
        set
        {
            updateEventTask(ref service, value);
        }
    }

    /// <summary>
    /// The amount of progress the player has
    /// </summary>
    public float Progress
    {
        get
        {
            return progress.CurrentValue;
        }
        set
        {
            updateEventTask(ref progress, value);
        }
    }

    /// <summary>
    /// Sets the value of a task and updates the Event Task
    /// </summary>
    /// <param name="stat">The stat to update</param>
    /// <param name="value">The value to update</param>
    private void updateEventTask(ref DynamicStat stat, float value)
    {
        if (stat.WillChangeLevel(value))
        {
            stat.CurrentValue = value;
            TaskManager.Instance.SetProgress(typeof(TEvent), stat.ToString() + " Level", stat.Level);
            Xp += 10;

            if (GameManager.Instance.IsMultiplayer)
            {
                DialogueTrigger.SetEndDialoguePosition(13);
                DialogueTrigger.TriggerDialogue("OfficerDialogue.json", 13);
            }
        }
        else
        {
            stat.CurrentValue = value;
        }
    }

    /// <summary>
    /// What level the player's fundraising is at
    /// </summary>
    public int FundraisingLevel
    {
        get
        {
            return fundraising.Level;
        }
        set
        {
            fundraising.Level = value;
        }
    }
    /// <summary>
    /// The Membership level the Player is at.
    /// </summary>
    public int MembershipLevel
    {
        get
        {
            return membership.Level;
        }
        set
        {
            membership.Level = value;
        }
    }

    /// <summary>
    /// The Membership level the Player is at.
    /// </summary>
    public int ServiceLevel
    {
        get
        {
            return service.Level;
        }
        set
        {
            service.Level = value;
        }
    }
    /// <summary>
    /// The Membership level the Player is at.
    /// </summary>
    public int EducationLevel
    {
        get
        {
            return education.Level;
        }
        set
        {
            education.Level = value;
        }
    }
    /// <summary>
    /// The Membership level the Player is at.
    /// </summary>
    public int ProgressLevel
    {
        get
        {
            return progress.Level;
        }
        set
        {
            progress.Level = value;
        }
    }
   
    /// <summary>
    /// The amount of experience of the player. XP is used to determine what place the player gets at events
    /// </summary>
    public float Xp
    {
        get
        {
            return experience.CurrentValue;
        }

        set
        {
            experience.CurrentValue = value;
            
        }
    }        

    /// <summary>
    /// Initializes all of the stats
    /// </summary>
    //This must come before any Start functions, so Awake is usesd
    private void Awake()
    {        
        fundraising.Initialize(currentValue: 0, MaxValue: 100, Level: 1, MaxLevel: 3, NextLevel: 100);
        membership.Initialize(currentValue: 0, MaxValue: 100, Level: 1, MaxLevel: 3, NextLevel: 100);
        education.Initialize(currentValue: 0, MaxValue: 100, Level: 1, MaxLevel: 3, NextLevel: 100);
        progress.Initialize(currentValue: 0, MaxValue: 100, Level: 1, MaxLevel: 3, NextLevel: 100);
        service.Initialize(currentValue: 0, MaxValue: 100, Level: 1, MaxLevel: 3, NextLevel: 100);
        experience.Initialize(currentValue: 0, MaxValue: 1000);
    }

    /// <summary>
    /// Initializes everything other than the stats
    /// </summary>
    protected override void Start () 
    {
        base.Start();                
        CanWalk = true;        
        Name = "You";
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        //If the player is outside, clamp his position
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(1) == SceneManager.GetSceneByBuildIndex(2))
        {
            //Makes sure the camera doesn't go further than our world
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minTile.x, maxTile.x), Mathf.Clamp(transform.position.y, minTile.y, maxTile.y), transform.position.z);
        }   
        
        //The object colliding with     
        Collider2D i;       
        
        //Checks to see if the player is colliding with an interact. Desplays its description                 
        if (collidedWithTag(Triggers, "Interact", out i) && i.gameObject.GetComponent<IInteract>() != null)
        {                
            interactText.GetComponent<Text>().text = string.Format("Press {0} to {1}", KeybindManager.Instance.GetKeyBindName("INTERACT"), i.gameObject.GetComponent<IInteract>().Description);
            interactText.SetActive(true);
        }
        else
            interactText.SetActive(false);        
        
        GetInputs(i);        
	}   

    /// <summary>
    /// Gets the inputs of the player 
    /// </summary>
    /// <param name="i">The interact object</param>
    void GetInputs(Collider2D i)
    {
        MoveVector = Vector3.zero;
        if (Input.GetKey(KeybindManager.Instance.Keybinds("LEFT")))
            MoveVector.x = -1;
        if (Input.GetKey(KeybindManager.Instance.Keybinds("DOWN")))
            MoveVector.y = -1;
        if (Input.GetKey(KeybindManager.Instance.Keybinds("RIGHT")))
            MoveVector.x = 1;
        if (Input.GetKey(KeybindManager.Instance.Keybinds("UP")))
            MoveVector.y = 1;

        //If the player is colliding with an interact object and press the interact button, do its interact method
        if (interactText.activeSelf && Input.GetKeyDown(KeybindManager.Instance.Keybinds("INTERACT")) && !DialogueManager.Instance.IsDialogueGoing)
        {
            InteractObject = i.gameObject.GetComponent<IInteract>();
            
            InteractObject.Interact();            
        }
    }

    /// <summary>
    /// Sets the limits of where the player can go
    /// </summary>
    /// <param name="minTile">The minimum tile that the player can walk on</param>
    /// <param name="maxTile">The maximum tile that the player can walk onparam>
    public void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        this.minTile = minTile;
        this.maxTile = maxTile;
    } 

    /// <summary>
    /// Checks to see if the player is colliding with a certain tag
    /// </summary>
    /// <param name="array">The array of all of the objects that the player is colliding with</param>
    /// <param name="tag">The tag to check</param>
    /// <param name="i">The with the tag</param>
    /// <returns>True if colliding</returns>
    bool collidedWithTag(List<Collider2D> array, string tag, out Collider2D i)
    {        
        i = null;
        try
        {
            foreach (Collider2D collider in array)
            {
                if (collider.gameObject.tag == tag)
                {
                    i = collider;
                    return true;
                }
            }           
        } 
        //This exception occurs when the player goes from scene to scene       
        catch (MissingReferenceException ex)
        {
            array.Clear();
        }  
        catch(NullReferenceException ex)
        {

        }      
        return false;
        
    }           
}
