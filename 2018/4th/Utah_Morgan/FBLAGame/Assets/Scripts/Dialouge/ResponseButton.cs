using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseButton : MonoBehaviour
{
    /// <summary>
    /// The button text
    /// </summary>
    [SerializeField]
    private Text text;

    /// <summary>
    /// The sprite of the pointer for when the response is selected
    /// </summary>
    [SerializeField]
    private Image pointer;

    /// <summary>
    /// The response that this button represents
    /// </summary>
    private Response response;	

    /// <summary>
    /// How many letters is displayed as text on this button
    /// </summary>
    /// <returns></returns>
    public int TextCount()
    {
        return text.text.Length;
    }

    /// <summary>
    /// Set this button to being selected
    /// </summary>
    /// <param name="value">True if this is selected</param>
    public void SetIsSelected(bool value)
    {
        pointer.enabled = value;
    }

    /// <summary>
    /// Used for Initialization
    /// </summary>
    /// <param name="response">The response that this button represents</param>
    public void SetUp(Response response)
    {
        this.response = response;
        text.text = response.Text;
    }
	
    /// <summary>
    /// Called when pressed enter with this response selected
    /// </summary>
    /// <returns>The dialogue to be displayed next</returns>
	public Dialogue HandleClick()
    {
        return response.Enter();
    }
}
