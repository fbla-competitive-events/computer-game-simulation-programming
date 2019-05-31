using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    /// <summary>
    /// The object pool of trash objects
    /// </summary>
    [SerializeField]
    private ObjectPool trashPool;

    /// <summary>
    /// The minimum tile that the trash can spawn on
    /// </summary>
    [SerializeField]
    Vector3 minTile;

    /// <summary>
    /// The maximum tile that the trash can spawn on 
    /// </summary>
    [SerializeField]
    Vector3 maxTile;

	/// <summary>
    /// Initialization
    /// </summary>
	void Start ()
    {
        //If there is a trash task, spawn trash
        if (TaskManager.Instance.ContainsTask(TTrash.TaskName))
        {
            SpawnTrash();
        }
	}	

    /// <summary>
    /// Spawns trash around within the min and max tiles
    /// </summary>
    private void SpawnTrash()
    {
        int TrashAmount = TTrash.TotalAmount;
        
        //Just so if there is a trash that is really hard to find, the player can still pick up enough trash
        TrashAmount += 3;

        //Start spawning trash
        for (int i = 0; i < TrashAmount; i++)
        {
            GameObject TrashObject = trashPool.GetObject();
            TrashObject.transform.localScale = Vector3.one;
            Vector3 position = new Vector3(Random.Range(minTile.x, maxTile.x), Random.Range(minTile.y, maxTile.y));
            TrashObject.transform.position = position;
        }
    }
}
