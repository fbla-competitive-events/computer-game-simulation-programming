using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : UIBox
{
    /// <summary>
    /// The singleton of the GameManager
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            return FindObjectOfType<GameManager>();
        }
    }

    /// <summary>
    /// Field that the property Stage is referring to
    /// </summary>
    private Stage stage;

#if UNITY_EDITOR
    ///<summary>
    ///A reference to the question scene
    ///</summary>     
    [SerializeField]
    private UnityEditor.SceneAsset questionScene;
    [SerializeField]
    public UnityEditor.SceneAsset[] Scenes;
#endif

    /// <summary>
    /// A reference to the result object pool used when the player is done with an event
    /// </summary>
    [SerializeField]
    private ObjectPool resultObjectPool;

    /// <summary>
    /// The parent of all of the result objects
    /// </summary>
    [SerializeField]
    Transform resultContent;

    /// <summary>
    /// The text of the result box
    /// </summary>
    [SerializeField]
    Text resultsMainText;

    /// <summary>
    /// The Canvas group for the completion box used to display
    /// </summary>
    [SerializeField]
    private UIBox completitionCanvasGroup;
    
    /// <summary>
    /// An array that consists of red and green objects
    /// </summary>
    [SerializeField]
    private Sprite[] ResultImages;
    
    /// <summary>
    /// A list of all of the result objects
    /// </summary>
    private List<GameObject> resultButtons;

    /// <summary>
    /// A list of all the stats
    /// </summary>
    private List<DynamicStat> stats;

    /// <summary>
    /// If the player is currently ready for his or her event
    /// </summary>
    private bool readyForEvent;

    /// <summary>
    /// If the game is over
    /// </summary>
    public bool EndOfGame { get; set; }

    /// <summary>
    /// If there are two players playing the game
    /// </summary>
    public bool IsMultiplayer { get; set; }

    

    /// <summary>
    /// At what point in the game the player is. Either Region, State, or Nationals
    /// </summary>
    public Stage Stage
    {
        get
        {
            return stage;
        }
        private set
        {
            if (!(value <= stage) && value > 0)
            {
                stage = value;
                prepareEvent();
            }
            
        }
    }
    /// <summary>
    /// Tells if the player has completed the event task for an event
    /// </summary>
    public bool ReadyForEvent
    {
        get
        {
            return readyForEvent;
        }
        private set
        {
            readyForEvent = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        //Gets the list of stats
        List<GameObject> g = new List<GameObject>(GameObject.FindGameObjectsWithTag("DynamicStat"));
        stats = new List<DynamicStat>();
        resultButtons = new List<GameObject>();
        stats.Add(g.Find(x => x.name == "Fundraising").GetComponent<DynamicStat>());
        stats.Add(g.Find(x => x.name == "Service").GetComponent<DynamicStat>());
        stats.Add(g.Find(x => x.name == "Progress").GetComponent<DynamicStat>());
        stats.Add(g.Find(x => x.name == "Education").GetComponent<DynamicStat>());
        stats.Add(g.Find(x => x.name == "Membership").GetComponent<DynamicStat>()); 
                       
        Stage = 0;
        EndOfGame = false;



    }

    /// <summary>
    /// When ever an Event task is completed, that task should call this function in its 'TaskCompleted' function
    /// </summary>
    public void EventTaskComplete()
    {
        ReadyForEvent = true;
        //If there is a dialogue going, wait until it ends to call mReadyForEvent. If there is not a dialogue going, call it right now
        if (DialogueManager.Instance.IsDialogueGoing)
        {
            DialogueTrigger.AddOnCloseEvent(mReadyForEvent);
        }
        else
        {
            mReadyForEvent();
        }      
       
    }

    void mReadyForEvent()
    {
        ScrollingTextManager.Instance.CreatText(Vector3.zero, Color.red, "You are ready for " + stage.ToString(), true, true);
        TaskManager.Instance.NewTask(new TReady());
        if (Stage == Stage.Region)
        {
            DialogueTrigger.SetEndDialoguePosition(11);
            DialogueTrigger.TriggerDialogue("OfficerDialogue.json", 9);
        }
    }

    /// <summary>
    /// Called when ever the player chooses to attend either a Region, State, or National event
    /// </summary>
    /// <returns>The place that the player got.</returns>
    public void GoToEvent()
    {
        //Deactivates the notification text
        if (ScrollingTextManager.Instance.NotificationText != null)
        {
            ScrollingTextManager.Instance.NotificationText.Deactivate();
        }        

        //Starts the questions
        QuestionTrigger.StartQuestionGame(Stage.ToString() + "Data.json", EndofJudging);                            
    }

    
    /// <summary>
    /// Called when the judging is over
    /// </summary>
    /// <param name="score">The presentation score</param>
    public void EndofJudging(int score)
    {
        
        Player.Instance.Xp += score;

        //Gets the placing of the opponents and the player
        List<KeyValuePair<string, float>> competitors = awardCeremonyCalculater();  
        
        //Draws the result objects and displays it, unless it is the end of the game, then displays the completion box      
        removeResultObjects();
        addResultObjects(competitors);
        resultsMainText.text = Stage.ToString() + " Results";
        if (Stage == Stage.Nationals)
        {
            UIManager.Instance.OpenClose(completitionCanvasGroup);
            EndOfGame = true;
        }
        else
        {
            UIManager.Instance.OpenClose(this);            
            Stage++;
        }        
        
    }

    /// <summary>
    /// Removes all of the result objects
    /// </summary>
    private void removeResultObjects()
    {
        foreach (GameObject o in resultButtons)
        {
            resultObjectPool.ReturnObject(o);
        }
        resultButtons.Clear();
    }

    /// <summary>
    /// Adds the result objects based on the given list of competitors
    /// </summary>
    /// <param name="competitors"></param>
    private void addResultObjects(List<KeyValuePair<string, float>> competitors)
    {
        for (int i = 0; i < competitors.Count; i++)
        {

            GameObject result = resultObjectPool.GetObject();
            result.transform.parent = resultContent;
            result.transform.localScale = Vector3.one;

            result.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}. {1} {2}", i + 1, competitors[i].Key, competitors[i].Value);
            resultButtons.Add(result);

            //If the result object is the player, then make it be the red image (ResultImages[1] is red and ResultImages[0] is green)
            result.GetComponent<Image>().sprite = (competitors[i].Key == "You") ? ResultImages[1] : ResultImages[0];
        }
    }
    
    /// <summary>
    /// Gets the ranking of the competitors
    /// </summary>
    /// <returns></returns>
    private List<KeyValuePair<string, float>> awardCeremonyCalculater()
    {
        int numOpponents;
        float maxXP;
        System.Random rnd = new System.Random();        

        //Based on the stage, assign a maximum amount of XP that a opponent can earn, and assign a number of opponents
        switch ((int)Stage)
        {
            case 1: numOpponents = 5; maxXP = 150; break;
            case 2: numOpponents = 10; maxXP = 300; break;
            case 3: numOpponents = 15; maxXP = 500; break;
            default: numOpponents = 0; maxXP = 0; break;
        }

        //Make a list of opponents with a random amount of XP chosen and then sort the list
        List<KeyValuePair<string, float>> opponents = new List<KeyValuePair<string, float>>();
        List<string> names = new List<string>() { "Bob", "Jessie", "Joe", "Johnny", "Billy", "Jimbo", "Dr. D", "Floyd Muffin", "Myles", "Brock", "Nash", "Braydon", "Sanford", "Maxwell"
        , "Carson", "Jackson", "Kailey", "Lynnsie", "Brook", "Cassandra", "Bart"};

        for (int i = 0; i < numOpponents; i++)
        {
            float xpAmount = rnd.Next((int)(maxXP / 2), (int)maxXP);
            string name = names[rnd.Next(0, names.Count - 1)];
            names.Remove(name);
            opponents.Add(new KeyValuePair<string, float>(name, xpAmount));                                  
        }                
        opponents.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        //See what place the player got by comparing him/her to the rest of the opponents
        int mark = 1;
        for (int i = 0; i < opponents.Count; i++)        
        {
            float t = opponents[i].Value;
            if (Player.Instance.Xp < t)
            {
                mark++;
            }
            else
            {                
                break;
            }
        }
        //insert the player in the list of competitors
        opponents.Insert(mark - 1, new KeyValuePair<string, float>(Player.Instance.Name, Player.Instance.Xp));        

        //Based on the player's place, award XP.
        float xpAwarded;
        switch (mark)
        {
            case 1: xpAwarded = 50; break;
            case 2: xpAwarded = 30; break;
            case 3: xpAwarded = 20; break;
            default: xpAwarded = 10; break;
        }
        Player.Instance.Xp += xpAwarded;                
        ReadyForEvent = false;
        return opponents;
    }
   
    /// <summary>
    /// Called when the player enters a new stage
    /// </summary>
    private void prepareEvent()
    {
        //Creates a new event task        
        TaskManager.Instance.NewTask(new TEvent(Stage));

        //Sets all of the stats to the max level for the current stage
        foreach (DynamicStat stat in stats)
        {
            stat.MaxLevel = getStageMaxLevel(Stage);
        }
    }

    /// <summary>
    /// Gets the max level that a dynamic stat can have based on a certain stage.
    /// </summary>
    /// <param name="stage">The stage to use to return the max level.</param>
    /// <returns>0 if stage=Region, 4 if stage=State, 7 if stage=2, 10 if stage = National</returns>
    private int getStageMaxLevel(Stage stage)
    {
        
        return ((int)stage == 0) ? 0 : (int)stage * 3 + 1;
    }

    /// <summary>
    /// Increments the stage
    /// </summary>
    public void SetNextStage()
    {
        Stage++;
    }
   
}

/// <summary>
/// An enum telling what stage the player is in
/// </summary>
public enum Stage
{
   // Default = 0,
    Region = 1,
    State = 2,
    Nationals = 3
}
