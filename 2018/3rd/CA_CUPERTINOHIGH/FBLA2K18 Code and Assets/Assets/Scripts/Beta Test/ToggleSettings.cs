using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleSettings : MonoBehaviour {

    public GameObject menuPanel;
    public Slider defaultSlider;
    // Use this for initialization
    void Start () {
        menuPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab))
        {
            //bool p = GameOptions.pause;
            
            BetaGameOptions.pause = !BetaGameOptions.pause;
            //Debug.Log(BetaGameOptions.pause);
            if (BetaGameOptions.pause)
            {
                menuPanel.SetActive(true);
                defaultSlider.Select();

            } else
            {
                menuPanel.SetActive(false);
            }
            
        }
	}
}
