using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// The single ton instance of the dialogue manager
    /// </summary>
    public static DialogueManager Instance
    {
        get
        {
            return FindObjectOfType<DialogueManager>();
        }
    }

    /// <summary>
    /// A reference to the dialogue text
    /// </summary>
    [SerializeField]
    Text dialogueText;

    /// <summary>
    /// A reference to the name text
    /// </summary>
    [SerializeField]
    Text nameText;

    /// <summary>
    /// A reference to the response object pool
    /// </summary>
    [SerializeField]
    ObjectPool objectPool;

    /// <summary>
    /// The parent of where these new response objects are childs of
    /// </summary>
    [SerializeField]
    Transform parentTransform;

    /// <summary>
    /// Used for determining is the dialogue is opening or closing
    /// </summary>
    Animator animator;

    /// <summary>
    /// The response box Game Object
    /// </summary>
    GameObject responseBox;

    /// <summary>
    /// A list of dialogue sentences
    /// </summary>
    private List<string> sentences;

    /// <summary>
    /// A list of respones objects
    /// </summary>
    private List<ResponseButton> responses;

    /// <summary>
    /// Which response the response pointer is pointing to
    /// </summary>
    int responsePosition;

    /// <summary>
    /// Which sentence the dialogue is currently displaying
    /// </summary>
    int dialoguePosition;
    public int DialoguePosition
    {
        get
        {
            return dialoguePosition;
        }
    } 
    
    ///<summary>
    ///If the dialogue can display the next sentence when the player wants it to 
    ///</summary>    
    public bool CanDisplayNext { get; set; }
   
    /// <summary>
    /// The position of the dialogue sentence at which it ends
    /// </summary>
    public int EndDialoguePosition { get; set; }

    /// <summary>
    /// The position of the dialogue sentence at which it brings up the responses
    /// </summary>
    public int ContinueToResponses { get; set; }

    /// <summary>
    /// An event that is called after the dialogue box closes
    /// </summary>
    public event Event OnClose;
    public delegate void Event();

    /// <summary>
    /// Tells if the dialogue box is going
    /// </summary>
    public bool IsDialogueGoing { get; set; }

    /// <summary>
    /// The current Dialogue being displayed
    /// </summary>
    public Dialogue CurrentDialogue { get; private set; }

    /// <summary>
    /// An enum determining if the dialogue box is opening or closing
    /// </summary>
    enum AnimationState { Opening, Closing }
    bool isAnimationGoing(AnimationState state)
    {
        string name = "";
        //If the Dialogue Box is opening, then the animation "DialogueBox_Close" will be playing
        //Vice versa
        switch (state)
        {
            case AnimationState.Opening: name = "DialogueBox_Close"; break;
            case AnimationState.Closing: name = "DialogueBox_Open"; break;
        }
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);

    }

    /// <summary>
    /// A list of OnFinishSentence (events) that are called when a certain sentence is finished
    /// </summary>
    public List<OnFinishSentence> OnFinishSentence_ { get; set; }
    public class OnFinishSentence
    {
        /// <summary>
        /// Delagate that returns bool if the dialogue should continue or not
        /// </summary>
        private Func<bool> OnFinish;

        /// <summary>
        /// Where this delegate needs to be called; which sentence number
        /// </summary>
        public int SentenceNumber { get; private set; }   
        
        /// <summary>
        /// Called when the sentence number equals "SentenceNumber." Invokes the delegate
        /// </summary>
        /// <returns>True if the dialogue can keep displaying sentences</returns>
        public bool Invoke()
        {
            //We want to only call this delegate once, so call it and set it equal to null
            if (OnFinish != null)
            {
                bool flag = OnFinish();
                OnFinish = null;
                return flag;
            }
            return false;
        }        

        /// <summary>
        /// Constructor used for Initialization
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="SentenceNumber"></param>
        public OnFinishSentence(Func<bool> Event, int SentenceNumber)
        {
            this.OnFinish = Event;
            this.SentenceNumber = SentenceNumber;
        }
    }

    /// <summary>
    /// Called when a new dialogue needs to be displayed. Does all of the initialization
    /// </summary>
    /// <param name="dialogue">The dialogue to display</param>
    /// <param name="caller">Who is talking</param>
    /// <param name="startingDialoguePosition">Which sentence to start at</param>
    public void StartDialogue(Dialogue dialogue, Movement caller, int startingDialoguePosition = 0)
    {
        //Initializes values
        CanDisplayNext = true;
        IsDialogueGoing = true;
        animator.SetBool("isOpen", true);        
        responseBox.SetActive(false);                
        dialogueText.text = "";
        RemoveButtons();
        sentences.Clear();
        responses.Clear();
        responsePosition = 0;

        //Sets the name and caller of the dialogue to whomever is calling
        dialogue.Caller = caller;
        if (caller != null)
        {
            dialogue.Name = caller.Name;
        }

        //The name text is just the name of the dialogue
        nameText.text = dialogue.Name;


        CurrentDialogue = dialogue;


        //Minus one because when the DisplayNextSentence is called, it increments the dialoguePosition
        dialoguePosition = startingDialoguePosition - 1;

        //Gets all of the dialogue text and stores it into an array
        foreach (string sentence in dialogue.DialogueText)
        {
            sentences.Add(sentence);
        }
       
        //Used to get the response that has the most amount of letters
        int maxLetters = 0;

        //Gets a reference to the dialogue responses
        List<Response> refr = dialogue.Responses;

        //Loops through all of the responses and makes a result button for each of them
        for (int i = 0; i < refr.Count; i++)
        {
            // Spawn an AnswerButton from the object pool            
            GameObject responseButtonObject = objectPool.GetObject();         
            responseButtonObject.transform.SetParent(parentTransform);
            responseButtonObject.transform.localScale = Vector3.one;

            ResponseButton responseButton = responseButtonObject.GetComponent<ResponseButton>();
            responses.Add(responseButton);
            responseButton.SetUp(refr[i]);

            //Figures out how big the response box should be based on the size of each text
            if (responseButton.TextCount() > maxLetters)
            {
                maxLetters = responseButton.TextCount();
            }
        }

        //For every one letter, there is 10 cellspacing, unless the maxletters is above the max variable, then limit the cell width to 40 letters
        //and times the cell height by how many 40's the max letters is above 40. 
        int max = 40;
        int height = (maxLetters / max) + 1;
        Vector2 cellSize = responseBox.GetComponent<GridLayoutGroup>().cellSize;
        responseBox.GetComponent<GridLayoutGroup>().cellSize = new Vector2(10 * Math.Min(maxLetters, max), height * 30);    

        //Displays the next dialogue sentence
        DisplayNextSentence();
    }

    /// <summary>
    /// Initializes things
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        sentences = new List<string>();
        responses = new List<ResponseButton>();
        EndDialoguePosition = ContinueToResponses = -1;
        OnFinishSentence_ = new List<OnFinishSentence>();               
        responseBox = transform.GetChild(1).gameObject;
    }

    /// <summary>
    /// Gets the inputs
    /// </summary>
    private void Update()
    {
        if (!IsDialogueGoing)
            return;        
        getInputs();
    }

    /// <summary>
    /// Gets the inputs of the player
    /// </summary>
    private void getInputs()
    {
        //If the player wants to dispaly the next sentence
        if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("RETURN")))
        {
            DisplayNextSentence();
        }        
        else if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("UPARROW")) || Input.GetKeyDown(KeybindManager.Instance.Keybinds("UP")))
        {
            displayLastResponse();
        }
        else if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("DOWNARROW")) || Input.GetKeyDown(KeybindManager.Instance.Keybinds("DOWN")))
        {
            displayNextResponse();
        }
        else if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("RIGHTARROW")) || Input.GetKeyDown(KeybindManager.Instance.Keybinds("RIGHT")))
        {
            displayRightResponse();
        }
        else if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("LEFTARROW")) || Input.GetKeyDown(KeybindManager.Instance.Keybinds("LEFT")))
        {
            displayLeftResponse();
        }
    }

    /// <summary>
    /// Determines if a sentence is being typed out
    /// </summary>
    /// <returns></returns>
    private bool isCoroutineRunning()
    {        
        return (dialogueText.text != sentences[dialoguePosition]);
    }

    /// <summary>
    /// When the user selects his or her response
    /// </summary>
    /// <param name="responsePosition">What response was selected</param>
    public void EnterResponse(int responsePosition)
    {
        Dialogue dialogue = responses[responsePosition].HandleClick();          
        
        if (dialogue == null)
        {
            //When there are still things to be said in the dialogue
            if (ContinueToResponses != -1)
            {
                ContinueToResponses = -1;
                responseBox.SetActive(false);
                DisplayNextSentence();
                return;
            }

            //If the dialogue is over
            EndDialogue();
            return;
        }

        StartDialogue(dialogue, CurrentDialogue.Caller);
    }

    /// <summary>
    /// Displays all of the responses
    /// </summary>
    /// <param name="responsePosition">The currently selected response</param>
    void displayResponses(int responsePosition)
    {
        //If there is still dialogue being displayed, or we are not at the end of the dialogue, we don't want to display the responses yet
        if (!CanDisplayNext || (dialoguePosition != sentences.Count - 1 && dialoguePosition != ContinueToResponses))
        {
            return;
        }

        //We want to display the resonpse box if there are responses to be displayed
        if (!responseBox.activeSelf && responses.Count != 0)
        {
            responseBox.SetActive(true);
        }
        
        //Loop through all of the responses and set the one that is selected
        for (int i = 0; i < responses.Count; i++)
        {
            ResponseButton r = responses[i];            
            r.SetIsSelected(i == responsePosition);  
                    
        }        
    }

    /// <summary>
    /// Moves the pointer down
    /// </summary>
    void displayNextResponse()
    {
        CrementResponsePosition(1);
        displayResponses(responsePosition);
    }

    /// <summary>
    /// Moves the pointer up
    /// </summary>
    void displayLastResponse()
    {
        CrementResponsePosition(-1);
        displayResponses(responsePosition);
    }

    /// <summary>
    /// Moves the pointer Left
    /// </summary>
    void displayLeftResponse()
    {
        displayRightResponse();
    }

    /// <summary>
    /// Moves the pointer right. Uses an algorithm to determine which value will acheive this
    /// </summary>
    void displayRightResponse()
    {
        int count = responses.Count;
        if (!(count % 2 == 1 && responsePosition == (int)(count / 2)))
        {
            int increment = (int)Math.Ceiling((decimal)((count + 1) / 2));
            int direction = (responsePosition >= increment) ? -1 : 1;
            responsePosition += direction * increment;
        }
        displayResponses(responsePosition);
    }

    /// <summary>
    /// An algorithm what what response position will make the pointer either go up or down
    /// </summary>
    /// <param name="i">1 for down and -1 for up</param>
    void CrementResponsePosition(int i)
    {
        int value = responsePosition + i;
        int c1i = 0;
        int c1f = (responses.Count-1) / 2;
        int c2i = c1f + 1;
        int c2f = responses.Count - 1;
        int max;
        int min;
        if (responsePosition <= c1f)
        {
            min = c1i;
            max = c1f;
        }
        else
        {
            min = c2i;
            max = c2f;
        }
        if (value < min)
        {
            responsePosition = max;
        }
        else if (value > max)
        {
            responsePosition = min;
        }
        else
        {
            responsePosition = value;
        }
    }

    /// <summary>
    /// Displays the next dialogue sentence. Called whenever the user presses enter
    /// </summary>
    /// <param name="CanDisplay">Enter true if trying to make sure the next sentence gets displayed, regardless of what the value of CanDisplayNext is</param>
    public void DisplayNextSentence(bool CanDisplay = false)
    {
        //The CanDisplay parameter is a hard code to actually display the next sentence, regardless if CanDisplayNext says we can or not
        CanDisplayNext = (CanDisplay) ? true : CanDisplayNext;

        //If we cannot display the next sentence, just finish it
        if (!CanDisplayNext)
        {
            FinishSentence();
            return;
        }

        //If either we are at the end of the sentence and there are no responses, or we are at the end dialogue position
        if (dialoguePosition == EndDialoguePosition && EndDialoguePosition != -1 || (sentences.Count - 1 == dialoguePosition && responses.Count == 0))
        {
            EndDialogue();
            return;
        }

        //If the responses are already displayed and the user presses enter (we are in this function don't ya know)
        //then handle the click event for the selected response
        if (sentences.Count - 1 == dialoguePosition || (dialoguePosition == ContinueToResponses && ContinueToResponses != -1))
        {
            EnterResponse(responsePosition);         
            return;
        }

        //Display the next sentence
        string sentence = sentences[++dialoguePosition];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    
    /// <summary>
    /// Stops the typing sentence coroutine and just displays the sentence in its full for
    /// </summary>
    private void FinishSentence()
    {       
        string sentence = sentences[dialoguePosition];

        StopAllCoroutines();
        dialogueText.text = sentence;    
        
        //Loops through all of the OnFinishSentences to see if a delegate needs to be calles    
        for (int i = 0; i < OnFinishSentence_.Count; i++)
        {
            OnFinishSentence s = OnFinishSentence_[i];
            if (s.SentenceNumber == dialoguePosition)
            {
                CanDisplayNext = s.Invoke();                    
                return;
            }
        }        
        CanDisplayNext = true;

        //If we are at the last dialogue, then display the responses
        if (dialoguePosition == sentences.Count - 1 || dialoguePosition == ContinueToResponses)
        {
            displayResponses(responsePosition);
        }
    }

    /// <summary>
    /// Adds an on finish sentence to the list
    /// </summary>
    /// <param name="s">The OnFinishSentence to add</param>
    public void AddOnFinishSentence(OnFinishSentence s)
    {
        //Makes sure there is not two on finish sentences for a given sentence position
        foreach (OnFinishSentence r in OnFinishSentence_)
        {
            if (r.SentenceNumber == s.SentenceNumber)
            {
                throw new Exception("Cannot have two of the same sentence numbers");
            }
        }
        OnFinishSentence_.Add(s);
    }

    /// <summary>
    /// Types the sentence out letter by letter
    /// </summary>
    /// <param name="sentence">The sentence to type out</param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        CanDisplayNext = false;
        dialogueText.text = "";    
        
        //Wait until the dialogue is done playing its open animation    
        yield return new WaitWhile(() => { return isAnimationGoing(AnimationState.Opening); });  
                           
        foreach (char letter in sentence.ToCharArray())
        {            
            dialogueText.text += letter;
            yield return null;
        }
        FinishSentence();
    }

    /// <summary>
    /// Ends and closes the dialogue box
    /// </summary>
    public void EndDialogue()
    {
        IsDialogueGoing = false;
        animator.SetBool("isOpen", false);
        Player.Instance.CanWalk = true;

        //Sets the Classmate (if it is the caller) back to being able to walk
        if (Player.Instance.InteractObject is Classmate)
        {
            ((Classmate)Player.Instance.InteractObject).CanWalk = true;
        }

        //So the messagebox won't keep updating
        sentences.Clear();
        OnFinishSentence_.Clear();
        RemoveButtons();
        EndDialoguePosition = -1;
        StartCoroutine(OnAnimationComplete());      
    }

    /// <summary>
    /// Removes all of the response buttons
    /// </summary>
    private void RemoveButtons()
    {
        while (responses.Count > 0)
        {
            ResponseButton r = responses[0];
            objectPool.ReturnObject(r.gameObject);
            responses.Remove(r);
        }
    }

    /// <summary>
    /// Waits until the message box swiddles its way to the bottom, then calls any method assigned to OnClose
    /// </summary>
    private IEnumerator OnAnimationComplete()
    {
        while (isAnimationGoing(AnimationState.Closing))
        {
            yield return null;
        }
        if (OnClose != null)
        {
            OnClose();
            OnClose = null;
        }
    }    
}
