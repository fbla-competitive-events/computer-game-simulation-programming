using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour {
    public static bool pause;
    public static int state;
    //add more states here
    public static readonly int gameState = 0;
    public static readonly int menuState = 1;
	// Use this for initialization
	void Start () {
        state = gameState;
        pause = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(state);
		if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log(state);
            if (state == gameState)
            {
                //turning into menuState
                pause = true;
                state = menuState;

                //MenuController menu = new MenuController();
                MenuController.ToggleMenuPanel(true);

                //Don't use Time.timeScale 
                //after menu, Time.timeScale sets the inputs to whatever
                //the input was right after the menu closes
                //so the input stays on that value

                //Time.timeScale = 1f;
            } else if (state == menuState)
            {
                //turning into gameState
                pause = false;
                state = gameState;

                //MenuController menu = new MenuController();
                MenuController.ToggleMenuPanel(false);

                //Time.timeScale = 0f;
            }
        }
	}
}
