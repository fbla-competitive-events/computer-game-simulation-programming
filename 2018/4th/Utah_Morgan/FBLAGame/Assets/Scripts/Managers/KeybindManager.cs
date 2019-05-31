using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindManager : UIBox
{
    /// <summary>
    /// A reference to the singleton instance
    /// </summary>
    private static KeybindManager instance;

    /// <summary>
    /// The array of key bind buttons
    /// </summary>
    [SerializeField]
    private GameObject[] keybindButtons;

    /// <summary>
    /// The current button selected
    /// </summary>
    private GameObject focusButton;

    /// <summary>
    /// Selects a new button
    /// </summary>
    /// <param name="toFocus">The button to focus</param>
    public void SetFocus(GameObject toFocus)
    {
        scrollbar.value = (keybindButtons.Length - Array.FindIndex(keybindButtons, x => x == toFocus) - 1.0f) / (keybindButtons.Length - 1.0f);
        toFocus = focusButton;
    }
        
    /// <summary>
    /// A reference to the task box scroll bar
    /// </summary>
    [SerializeField]
    private Scrollbar scrollbar;

    /// <summary>
    /// Property for accessing the singleton instance
    /// </summary>
    public static KeybindManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    /// <summary>
    /// Tells if the 
    /// </summary>
    public bool CanGetKeybind { get; set; }

    /// <summary>
    /// A dictionary for all keybinds
    /// </summary>
    private Dictionary<string, KeyCode> keybinds;

    /// <summary>
    /// The dictionary for the menu binds
    /// </summary>
    private Dictionary<string, KeyCode> menuBinds;

    /// <summary>
    /// Tells if the Key Binds Manager is being used
    /// </summary>
    public bool Active
    {
        get
        {
            return GetComponent<CanvasGroup>().blocksRaycasts;
        }
    }

    /// <summary>
    /// The name of the keybind we are trying to set or change
    /// </summary>
    private string bindName;

    // Use this for initialization
    void Start()
    {
        CanGetKeybind = true;

        keybinds = new Dictionary<string, KeyCode>();
        menuBinds = new Dictionary<string, KeyCode>();

        //keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

        //Generates the default keybinds
        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("PAUSE", KeyCode.P, menuBinds);
        BindKey("TASK", KeyCode.E, menuBinds);
        BindKey("QUIT", KeyCode.Escape);

        BindKey("INTERACT", KeyCode.Q);

        BindKey("RETURN", KeyCode.Return);
        BindKey("UPARROW", KeyCode.UpArrow);
        BindKey("DOWNARROW", KeyCode.DownArrow);
        BindKey("LEFTARROW", KeyCode.LeftArrow);
        BindKey("RIGHTARROW", KeyCode.RightArrow);
    }

    /// <summary>
    /// Binds a specific key using the keybinds dictionary
    /// </summary>
    /// <param name="key">Key to bind</param>
    /// <param name="keyBind">Keybind to set</param>
    public void BindKey(string key, KeyCode keyBind)
    {
        BindKey(key, keyBind, keybinds);
    }

    /// <summary>
    /// Binds a specific key
    /// </summary>
    /// <param name="key">Key to bind</param>
    /// <param name="keyBind">Keybind to set</param>
    /// <param name="currentDictionary">The dictionary to bind to</param>
    public void BindKey(string key, KeyCode keyBind, Dictionary<string, KeyCode> currentDictionary)
    {
        if (!currentDictionary.ContainsKey(key))//Checks if the key is new
        {
            //If the key is new we add it
            currentDictionary.Add(key, keyBind);

            //We update the text on the button
            UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind)) //If we already have the keybind, then we need to change it to the new bind
        {
            //Looks for the old keybind
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            //Unassigns the old keybind
            currentDictionary[myKey] = KeyCode.None;
            UpdateKeyText(key, keyBind);
        }

        //Makes sure the new key is bound
        currentDictionary[key] = keyBind;

        UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    /// <summary>
    /// Gets the name of the KeyCode given the name of the Key
    /// </summary>
    /// <param name="Key">The name of the Key</param>
    /// <returns>The name of the KeyCode</returns>
    public string GetKeyBindName(string Key)
    {
        Dictionary<string, KeyCode> currentDictionary = keybinds;

        return currentDictionary[Key].ToString();
    }

    /// <summary>
    /// Returns the keyvalue given a key
    /// </summary>
    /// <param name="Key">The key</param>
    /// <returns>The key value</returns>
    public KeyCode Keybinds(string Key)
    {
        //Return the menu bind key, if it is one        
        if (menuBinds.ContainsKey(Key))
        {
            return menuBinds[Key];
        }

        //If it is not a menu, make sure we can actually return the key
        if (!CanGetKeybind) return 0;
        return keybinds[Key];
    }

    /// <summary>
    /// Updates the text on a keybindbutton after the key has been changed
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
    public void UpdateKeyText(string key, KeyCode code)
    {
        GameObject t = Array.Find(keybindButtons, x => x.name.Substring(1) == key);

        //If there is no button for the keybind, its ok
        if (t == null) return;

        Text tmp = t.GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    /// <summary>
    /// Function for setting a keybind, this is called whenever a keybind button is clicked on the keybind menu
    /// </summary>
    /// <param name="bindName"></param>
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }
   
    private void OnGUI()
    {
        //Checks if we are going to save a keybind
        if (bindName != string.Empty)
        {
            //Listens for an event
            Event e = Event.current;

            //If the event is a key, then we change the keybind
            if (e.isKey && e.keyCode != KeyCode.Return)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
