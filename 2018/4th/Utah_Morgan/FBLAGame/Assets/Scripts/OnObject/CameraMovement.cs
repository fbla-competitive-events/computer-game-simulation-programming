using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{    
    /// <summary>
    /// The camera's target in our case it is the player
    /// </summary>
    private Transform target;

    /// <summary>
    /// The minimum and maximum value of the camera
    /// </summary>
    private float xMax, xMin, yMin, yMax;

    /// <summary>
    /// A reference to the ground tilemap
    /// </summary>
    [SerializeField]
    private Tilemap tilemap;

    /// <summary>
    /// A reference to the player
    /// </summary>
    private Player player;

    // Use this for initialization
    void Start()
    {
        //Creates a reference to the player's script
        player = FindObjectOfType<Player>();
        //Creates a reference to the target
        target = player.transform;


        ////Calculates the min and max postion        
        Vector3 minTile = new Vector3(-23, -5, 0);
        Vector3 maxTile = new Vector3(6, 8, 0);

        //Sets the limits of the camera
        SetLimits(minTile, maxTile);

        //Sets the limits of the player
        player.SetLimits(minTile, new Vector3(maxTile.x, maxTile.y - 2.5f));
    }

    private void LateUpdate()
    {
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(1) == SceneManager.GetSceneByBuildIndex(2))
        {
            //Makes sure the camera doesn't go further than our world
            transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), -10);
        }
        else
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10);
        }
    }

    /// <summary>
    /// Sets the cameras limits, this is used to make sure it can't go over the edge of the main tilemap
    /// </summary>
    /// <param name="minTile">The position of the minimum tile</param>
    /// <param name="maxTile">The position of the maximum tile</param>
    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }

}
