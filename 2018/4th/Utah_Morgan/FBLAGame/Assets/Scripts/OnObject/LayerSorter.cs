using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    /// <summary>
    /// A reference to the sprite of the character
    /// </summary>
    private SpriteRenderer renderer;

    /// <summary>
    /// A list of the obstacles currently being collided with
    /// </summary>
    private List<Obstacle> obstacles = new List<Obstacle>();

	// Use this for initialization
	void Start ()
    {
        renderer = transform.parent.GetComponent<SpriteRenderer>();	
	}
	
    /// <summary>
    /// Adds the entering collision to the list of obstacles and updates our sorting order
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        Obstacle o = collision.GetComponent<Obstacle>();
        if (o != null)
        {
            if (obstacles.Count == 0 || o.Renderer.sortingOrder + 1 > renderer.sortingOrder)
            {
                renderer.sortingOrder = o.Renderer.sortingOrder + 1;
            }
            obstacles.Add(o);
        }
    }

    /// <summary>
    /// Removes the obstacle that is exiting and updates the sorting order
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        Obstacle o = collision.GetComponent<Obstacle>();
        if (o != null)
        {
            obstacles.Remove(o);
            if (obstacles.Count == 0)
            {
                renderer.sortingOrder = 0;
            }
            else
            {
                obstacles.Sort();
                renderer.sortingOrder = obstacles[0].Renderer.sortingOrder + 1;
            }
        }
    }
}
