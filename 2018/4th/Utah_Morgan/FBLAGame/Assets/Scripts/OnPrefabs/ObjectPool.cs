using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// A very simple object pooling class
public class ObjectPool : MonoBehaviour
{
    // the prefab that this object pool returns instances of
    [SerializeField]
    private GameObject prefab;
    // collection of currently inactive instances of the prefab
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Returns an instance of the prefab
    public GameObject GetObject()
    {
        GameObject spawnedGameObject;

        // if there is an inactive instance of the prefab ready to return, return that
        if (inactiveInstances.Count > 0)
        {
            // remove the instance from the collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        // otherwise, create a new instance
        else
        {
            spawnedGameObject = (GameObject)GameObject.Instantiate(prefab);

            // add the PooledObject component to the prefab so we know it came from this pool
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }        

        // put the instance in the root of the scene and enable it
        spawnedGameObject.transform.SetParent(this.transform);
        spawnedGameObject.transform.SetParent(null);
        spawnedGameObject.SetActive(true);        

        // return a reference to the instance
        return spawnedGameObject;
    }

    // Return an instance of the prefab to the pool
    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();        

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);

            // add the instance to the collection of inactive instances
            inactiveInstances.Push(toReturn);
        }
        // otherwise, just destroy it
        else
        {
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
            Destroy(toReturn);
        }
    }

    /// <summary>
    /// Used to spawn a new classmate for when it is at its destination in the class state
    /// </summary>
    /// <param name="old">The old classmate that is at its destination, wanting to be destroyed</param>
    public void SpawnClassmate(Classmate old)
    {
        old.Destroyed = true;
        StartCoroutine(WaitToSpawn(old.transform.position));        
        ReturnObject(old.gameObject);        
    }

    /// <summary>
    /// Waits for a couple seconds before spawing a new classmate
    /// </summary>
    /// <param name="pos">The position of where the classmate needs to spawn</param>
    /// <returns></returns>
    IEnumerator WaitToSpawn(Vector3 pos)
    {
        yield return new WaitForSeconds(2f);
        GameObject classmateObject = GetObject();

        classmateObject.transform.position = pos;

        classmateObject.GetComponent<Classmate>().Destroyed = false;
               
    }
}

// a component that simply identifies the pool that a GameObject came from
public class PooledObject : MonoBehaviour
{
    public ObjectPool pool;
}