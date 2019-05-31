using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStatHUD : MonoBehaviour {
    public TextMeshProUGUI Involvement;
    public TextMeshProUGUI Charisma;
    public TextMeshProUGUI Knowledge;

    // Use this for initialization
    void Start () {
        Involvement.SetText("Involvement: " + GameAndPlayerManager.Involvement);
    }
	
	// Update is called once per frame
	void Update () {
        Involvement.SetText("Involvement: " + GameAndPlayerManager.Involvement);
        Charisma.SetText("Charisma: " + GameAndPlayerManager.Charisma);
        Knowledge.SetText("Knowledge: " + GameAndPlayerManager.Knowledge);
    }
}
