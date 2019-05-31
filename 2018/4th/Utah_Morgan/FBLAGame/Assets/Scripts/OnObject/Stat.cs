using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Stat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ///<summary>
    ///Our reference to the stat image that we need to adjust fill amount
    ///</summary>
    private Image content;  
    
    ///<summary>
    ///The text of the state
    ///</summary>      
    protected string Text
    {
        get
        {
            return statValue.text;
        }
        set
        {
            statValue.text = value;
        }
    }
    private Text statValue;

    /// <summary>
    /// How much of the stat is filled
    /// </summary>
    private float currentFill;

    /// <summary>
    /// The stat's max value.
    /// </summary>
    public float MaxValue { get; set; }

    /// <summary>
    /// The stat's current value.
    /// </summary>
    public virtual float CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MaxValue)
            {
                CurrentValue = MaxValue;
            }
            else if (value < 0)
            {
                CurrentValue = 0;
            }
            else
            {                
                currentValue = value;

                //updates the fill of the fill bar
                currentFill = currentValue / MaxValue;
                
            }
                       
        }
    }
    private float currentValue;

    /// <summary>
    /// If the stat is being displayed or not
    /// </summary>
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();            
            if (value)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
            }
            canvasGroup.blocksRaycasts = value;
        }
    }    

    private bool active;



    /// <summary>
    /// Initialization
    /// </summary>
    protected virtual void Start ()
    {
        Active = true;
        Text = name;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        if (content != null)
        {
            //Only update if the fill amount was changed and if we have a content to change
            if (currentFill != content.fillAmount && content != null)
            {
                content.fillAmount = currentFill;
            }
            
        }                        
    }    

    /// <summary>
    /// Initializes the stat
    /// </summary>
    /// <param name="currentValue">The current value of the stat</param>
    /// <param name="MaxValue">The max value of the stat</param>
    public void Initialize(float currentValue, float MaxValue)
    {  
        if (content == null)
            content = GetComponent<Image>();
        if (statValue == null)
            statValue = GetComponentInChildren<Text>();
        this.MaxValue = MaxValue;
        CurrentValue = currentValue;
    }

    /// <summary>
    /// Sets the text to be the value of the stat
    /// </summary>
    protected virtual void SetNameText()
    {
        Text = currentValue + " / " + MaxValue;
    }

    /// <summary>
    /// Toggles between active and not active
    /// </summary>
    public void ToggleActive()
    {
        Active = !Active;
    }

    /// <summary>
    /// How the stat is converted to a string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return gameObject.name;
    }

    /// <summary>
    /// When the user hovers over the stat, change the text to the value of the stat
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetNameText();
    }

    /// <summary>
    /// When the user hovers out of the text, change the text to its name
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Text = name;
    }
}
