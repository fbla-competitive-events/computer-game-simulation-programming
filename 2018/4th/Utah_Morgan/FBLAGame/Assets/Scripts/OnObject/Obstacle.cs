using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    /// <summary>
    /// The sprite renderer of the object
    /// </summary>
    public SpriteRenderer Renderer
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// How the obstacle is sorted. Based on the sorting order
    /// </summary>
    /// <param name="other">Compare to</param>
    /// <returns></returns>
    public int CompareTo(Obstacle other)
    {
        if (Renderer.sortingOrder < other.Renderer.sortingOrder)
        {
            return 1;
        }
        else if (Renderer.sortingOrder > other.Renderer.sortingOrder)
        {
            return -1;
        }
        return 0;
    }
}
