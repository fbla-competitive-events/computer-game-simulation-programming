using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBox : MonoBehaviour
{
    /// <summary>
    /// The Canvas group for the UI Box
    /// </summary>
    public CanvasGroup CanvasGroup;
    
    /// <summary>
    /// The first selected gameobject when opened
    /// </summary>
    public GameObject FirstSelected;

    /// <summary>
    /// If activating this UI will change the players behavior
    /// </summary>
    public bool AffectPlayerAction = true;    
}
