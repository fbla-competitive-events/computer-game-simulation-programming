using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// Called when there is a new state
    /// </summary>
    /// <param name="parent">The classmate that is in this state</param>
    void Enter(Classmate parent);

    /// <summary>
    /// Called in the classmate update function
    /// </summary>
    void Update();

    /// <summary>
    /// Called when the state changes to a new state
    /// </summary>
    void Exit();

    /// <summary>
    /// The enum version of the state
    /// </summary>
    Classmate.State State { get; }
}
