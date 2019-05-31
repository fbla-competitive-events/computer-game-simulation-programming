using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour, IInteract 
{
    ///<summary>
    ///What side of the door the player needs to spawn
    ///</summary>
    enum Direction { Up, Right, Down, Left}


    /// <summary>
    /// The scene to spawn when entering the door
    /// </summary>
    [SerializeField]
    private string scene;


    /// <summary>
    /// The name of the object to spawn on in the new scene. Usually another door
    /// </summary>
    [SerializeField]
    private string spawnName;

    ///<summary>
    /// What side of the door the player needs to spawn
    ///</summary>
    [SerializeField]
    private Direction direction;            

    /// <summary>
    /// What is displayed when the player is close enough
    /// </summary>
    public string Description
    {
        get
        {
            return "go through door";
        }
    }

    ///<summary>
    /// Goes through the door and updates the scene
    ///</summary>    
    public void Interact()
    {
        //Unloads the current scene
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        //Loads the new scene
        AsyncOperation async =  SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);        
        async.completed += doneloading;        
    }

    /// <summary>
    /// Called when the new scene is done loading
    /// </summary>
    /// <param name="operation"></param>
    void doneloading(AsyncOperation operation)
    {
        //Gets what side of the door the player needs to spawn
        float y = 0, x = 0;
        switch (direction)
        {
            case Direction.Up: y = 1; break;
            case Direction.Down: y = -1; break;
            case Direction.Right: x = 1; break;
            case Direction.Left: x = -1; break;
        }

        //Gets an array of all of the root objects and loops through them to find the right door to spawn on
        GameObject[] goArray = SceneManager.GetSceneByName(scene).GetRootGameObjects();        
        foreach (GameObject go in goArray)
        {            
            if (go.tag == "Grid")
            {
                GameObject t = getChildGameObject(go, spawnName);
                Player.Instance.transform.position = new Vector3(t.transform.position.x + x, t.transform.position.y + y);
            }
            Player.Instance.Colliders.Clear();
            Player.Instance.Triggers.Clear();
        }
    } 
    
    ///<summary>
    ///Gets the child of a given object with a given name
    ///</summary>   
    GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }


}
