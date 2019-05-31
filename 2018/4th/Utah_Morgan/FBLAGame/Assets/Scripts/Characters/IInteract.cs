using UnityEngine;
using System.Collections;

public interface IInteract
{
    /// <summary>
    /// Called when the player wants to interact with the object implementing this interface
    /// </summary>
    void Interact();

    /// <summary>
    /// What is displayed on screen when the player is close enough
    /// </summary>
    string Description { get; }
	
}
