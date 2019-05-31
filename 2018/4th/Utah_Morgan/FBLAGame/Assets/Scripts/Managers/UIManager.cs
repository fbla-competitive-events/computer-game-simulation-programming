using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// The single ton Instance of this class
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            return FindObjectOfType<UIManager>();
        }
    }

    /// <summary>
    /// The canvas group of the keybind menu used to turn on and off
    /// </summary>
    [SerializeField]
    CanvasGroup keybindMenu;

    /// <summary>
    /// The canvas group of the taskbox menu used to turn on and off
    /// </summary>
    [SerializeField]
    UIBox taskBoxMenu;

    /// <summary>
    /// An array of key bind buttons
    /// </summary>
    GameObject[] keybindButtons;

	/// <summary>
    /// Initialization
    /// </summary>
	void Awake ()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
	}
	
	/// <summary>
    /// Called once per frame
    /// </summary>
	void Update ()
    {
        try
        {
            //If the keybind manager is active, we do not want to update the UIs
            if (KeybindManager.Instance == null || KeybindManager.Instance.Active) return;

            //Checks if the PAUSE key is pressed. Toggles between Resume and Pause
            if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("PAUSE")))
            {
                if (PauseMenu.IsGamePaused)
                {
                    PauseMenu.Instance.Resume();
                }
                else
                {
                    PauseMenu.Instance.Pause();
                }
            }

            if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("QUIT")))
            {
                Quit();
            }

            //Checks to see if the TaskBox button is pressed. Pulls up the task box if so
            if (Input.GetKeyDown(KeybindManager.Instance.Keybinds("TASK")))
            {
                OpenClose(taskBoxMenu);
            }
        }
        catch (MissingReferenceException ex)
        {
            return;
        }
        
	}    
   
    /// <summary>
    /// Toggles between if the given UI element is active or not
    /// </summary>
    /// <param name="canvasGroup">The canvas group attached to the UI element</param>
    public void OpenClose(UIBox ui)
    {
        CanvasGroup canvasGroup = ui.CanvasGroup;

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
        canvasGroup.interactable = canvasGroup.blocksRaycasts;

        UpdateSelected(ui);

        //We only want to affect the player if the UI allows us
        if (!ui.AffectPlayerAction)
        {
            return;
        }
       
        if (KeybindManager.Instance != null)
        {
            //If there is a UI up, make it so we cannot revieve input
            KeybindManager.Instance.CanGetKeybind = !canvasGroup.blocksRaycasts;
        }
        
        if (Player.Instance != null)
        {
            //Player.CanWalk needs to be the opposite of the state of the menu
            Player.Instance.CanWalk = !canvasGroup.blocksRaycasts;
        }
                
        if (DialogueManager.Instance != null)
        {
            //If the dialogue is going, make sure the player cannot walk
            if (DialogueManager.Instance.IsDialogueGoing || GameManager.Instance.EndOfGame)
            {
                Player.Instance.CanWalk = false;
            }
        }        
    }

    /// <summary>
    /// Sets the focus of the ui
    /// </summary>
    /// <param name="ui">The ui box's First Selected to be focused</param>
    public void UpdateSelected(UIBox ui)
    {
        EventSystem.current.SetSelectedGameObject(ui.FirstSelected);        
    }

    /// <summary>
    /// Sets the focus of the ui
    /// </summary>
    /// <param name="gameObject">The game object to be focused</param>
    public void UpdateSelected(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.Count > 1)
        {
            clickable.StackText.text = clickable.Count.ToString();
            clickable.StackText.color = Color.white;
            clickable.Icon.color = Color.white;
        }
        else
        {
            clickable.StackText.color = new Color(0, 0, 0, 0);

            if (clickable.Count == 0)
            {
                clickable.Icon.color = new Color(0, 0, 0, 0);
            }
        }        
    }    

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
