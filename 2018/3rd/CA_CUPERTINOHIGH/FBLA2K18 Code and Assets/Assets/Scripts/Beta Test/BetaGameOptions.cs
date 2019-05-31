using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaGameOptions : MonoBehaviour {
    public static bool pause;
    public static int state;
    //add more states here
    public static readonly int gameState = 0;
    public static readonly int menuState = 1;

    //public static bool IsFBLAMember = false;
    // Use this for initialization
    void Start()
    {
        state = gameState;
    }

}
