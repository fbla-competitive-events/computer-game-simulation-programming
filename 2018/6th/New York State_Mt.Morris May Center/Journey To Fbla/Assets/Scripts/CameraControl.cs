using UnityEngine;
using System.Collections;

// This is basically how the Super Metroid camera worked. Whichever direction you moved, the camera would 
// move in the same direction a multiple of the player's speed. Once the center of the camera moved a 
// certain distance from the player, the camera would lock on the player and move the same speed. Change
// movement direction, and the camera would once again move more quickly to catch up and place itself 
// ahead of the player's movement.

// Super Metroid also had area limits and locked certain axes based on where you were. For instance, if 
// you were in a vertical-only location, it would not move left or right at all, but would move normally 
// up and down until it hit a boundary. To do this, I have provided the LimitCameraMovement bool and 
// limit variables to set boundaries on camera movement. Set both limits on the same axis to the same 
// number if you want to lock the camera on that axis. I've included a few handy functions at the bottom 
// that turn this into a legit camera system.

// Get in touch with me, @Jellybit on twitter if you like this, have questions, or find it lacking or 
// weird somehow. Use this however you wish. I'd appreciate some sort of thanks, but nothing's required.

// From now on, I'll put the newest version on github here: https://gist.github.com/Jellybit/9f6182c25bceac06db31

public class CameraControl : MonoBehaviour
{

    [Header("Basic Setup")]
    [Tooltip("Drag your player, or whatever object you want the camera to track here. If you need to get the player by name, there's a line in the code you can uncomment.")]
    public GameObject player;

    [Tooltip("Should be at least 1.1, as for this to work, the camera has to move faster than the player. Otherwise, it behaves as if the camera is locked to the player.")]
    [Range(1, 10)]
    public float scrollMultiplier = 1.8f;
    [Space(10)]
    [Tooltip("The player will be kept within this area on the screen. If you have trouble visualizing it, turn on the Debug Visuals below and press play to see it.")]
    public Vector2 movementWindowSize = new Vector2(8, 6);

    [Tooltip("If the root of your character is at the feet, you can set this offset to half the player's height to compensate. You can also just use it to keep the box low to the ground or whatever you like.")]
    public Vector2 windowOffset;

    // Activate your position limitations for the Y axis by turning this on.

    [Header("Camera Movement Boundaries")]
    [Tooltip("Turn this on to have the camera use the positional limits set below. Set both limits on an axis to the same number to lock the camera so that it only moves on the other axis.")]
    public bool limitCameraMovement = false;
    [Space(10)]
    [Tooltip("Set the leftmost position you want the camera to be able to go.")]
    public float limitLeft;
    [Tooltip("Set the rightmost position you want the camera to be able to go.")]
    public float limitRight;
    [Space(10)]
    [Tooltip("Set the lowest position you want the camera to be able to go.")]
    public float limitBottom;
    [Tooltip("Set the highest position you want the camera to be able to go.")]
    public float limitTop;


    [Header("Debug Visuals")]
    [Tooltip("Draws a debug box on the screen while the game is running so you can see the boundaries against the player. Red boundaries mean that they are being ignored due to the following options.")]
    public bool showDebugBoxes = false;

    [HideInInspector]
    public bool activeTracking = true;

    private Vector3 cameraPosition;
    private Vector3 playerPosition;
    private Vector3 previousPlayerPosition;
    private Rect windowRect;

    void Start()
    {

        cameraPosition = transform.position;

        //Uncomment the following if you need to get the player by name.
        //player = GameObject.Find ( "Player Name" );

        if (player == null)
            Debug.LogError("You have to let the CameraControl script know which object is your player.");

        previousPlayerPosition = player.transform.position;

        ValidateLeftAndRightLimits();
        ValidateTopAndBottomLimits();

        //These are the root x/y coordinates that we will use to create our boundary rectangle.
        //Starts at the lower left, and takes the offset into account.
        float windowAnchorX = cameraPosition.x - movementWindowSize.x / 2 + windowOffset.x;
        float windowAnchorY = cameraPosition.y - movementWindowSize.y / 2 + windowOffset.y;

        //From our anchor point, we set the size of the window based on the public variable above.
        windowRect = new Rect(windowAnchorX, windowAnchorY, movementWindowSize.x, movementWindowSize.y);

    }


    void LateUpdate()
    {
        //Updates the camera position based on player location
        CameraUpdate();

        // This draws the camera boundary rectangle
        if (showDebugBoxes) DrawDebugBox();
    }

    void CameraUpdate()
    {
        playerPosition = player.transform.position;

        //Only worry about updating the camera based on player position if the player has actually moved.
        //If the tracking isn't active at all, we don't bother with any of this crap.
        if (activeTracking && playerPosition != previousPlayerPosition)
        {

            cameraPosition = transform.position;

            //Get the distance of the player from the camera.
            Vector3 playerPositionDifference = playerPosition - previousPlayerPosition;

            //Move the camera this direction, but faster than the player moved.
            Vector3 multipliedDifference = playerPositionDifference * scrollMultiplier;

            cameraPosition += multipliedDifference;

            //updating our movement window root location based on the current camera position
            windowRect.x = cameraPosition.x - movementWindowSize.x / 2 + windowOffset.x;
            windowRect.y = cameraPosition.y - movementWindowSize.y / 2 + windowOffset.y;

            // We may have overshot the boundaries, or the player just may have been moving too 
            // fast/popped into another place. This corrects for those cases, and snaps the 
            // boundary to the player.
            if (!windowRect.Contains(playerPosition))
            {
                Vector3 positionDifference = playerPosition - cameraPosition;
                positionDifference.x -= windowOffset.x;
                positionDifference.y -= windowOffset.y;

                //I made a function to figure out how much to move in order to snap the boundary to the player.
                cameraPosition.x += DifferenceOutOfBounds(positionDifference.x, movementWindowSize.x);


                cameraPosition.y += DifferenceOutOfBounds(positionDifference.y, movementWindowSize.y);

            }

            // Here we clamp the desired position into the area declared in the limit variables.
            if (limitCameraMovement)
            {
                cameraPosition.y = Mathf.Clamp(cameraPosition.y, limitBottom, limitTop);
                cameraPosition.x = Mathf.Clamp(cameraPosition.x, limitLeft, limitRight);
            }

            // and now we're updating the camera position using what came of all the calculations above.
            transform.position = cameraPosition;

        }

        previousPlayerPosition = playerPosition;
    }


    //This takes the player distance from the camera, and subtracks the boundary distance to find how far the
    //player has overshot things.
    static float DifferenceOutOfBounds(float differenceAxis, float windowAxis)
    {
        float difference;

        // We're seeing here if the player has overshot it at all on this axis. If not, we just set the 
        // difference to zero. This is because if we subtract the boundary distance when the player isn't far 
        // from the camera, we'll needlessly compensate, and screw up the camera.
        if (Mathf.Abs(differenceAxis) <= windowAxis / 2)
            difference = 0f;
        // And if the player legit overshot the boundary, we subtract the boundary from the distance.
        else
            difference = differenceAxis - (windowAxis / 2) * Mathf.Sign(differenceAxis);


        //Returns something if the overshot was legit, and zero if it wasn't.
        return difference;

    }

    // These try to correct for accidents/confusion.
    void ValidateTopAndBottomLimits()
    {
        if (limitTop < limitBottom)
        {
            Debug.LogError("You have set the limitBottom (" + limitBottom + ") to a higher number than limitTop (" + limitTop + "). This makes no sense as the top has to be higher than the bottom.");
            Debug.LogError("I'm correcting this for you, but please make sure the bottom is under the top next time. If you meant to lock the camera, please set top and bottom limits to the same number.");

            float tempFloat = limitTop;

            limitTop = limitBottom;
            limitBottom = tempFloat;
        }
    }

    void ValidateLeftAndRightLimits()
    {
        if (limitRight < limitLeft)
        {
            Debug.LogError("You have set the limitLeft (" + limitLeft + ") to a higher number than limitRight (" + limitRight + "). This makes no sense as it puts the left limit to the right of the right limit.");
            Debug.LogError("I'm correcting this for you, but please make sure the left limit is to the left of the right limit. If you meant to lock the camera, please set left and right limits to the same number.");

            float tempFloat = limitRight;

            limitRight = limitLeft;
            limitLeft = tempFloat;
        }
    }

    void DrawDebugBox()
    {
        Vector3 cameraPos = transform.position;

        //This will draw the boundaries you are tracking in green, and boundaries you are ignoring in red.
        windowRect.x = cameraPos.x - movementWindowSize.x / 2 + windowOffset.x;
        windowRect.y = cameraPos.y - movementWindowSize.y / 2 + windowOffset.y;

        Color xColorA;
        Color xColorB;
        Color yColorA;
        Color yColorB;

        if (!activeTracking || limitCameraMovement && cameraPos.x <= limitLeft)
            xColorA = Color.red;
        else
            xColorA = Color.green;

        if (!activeTracking || limitCameraMovement && cameraPos.x >= limitRight)
            xColorB = Color.red;
        else
            xColorB = Color.green;

        if (!activeTracking || limitCameraMovement && cameraPos.y <= limitBottom)
            yColorA = Color.red;
        else
            yColorA = Color.green;

        if (!activeTracking || limitCameraMovement && cameraPos.y >= limitTop)
            yColorB = Color.red;
        else
            yColorB = Color.green;

        Vector3 actualWindowCorner1 = new Vector3(windowRect.xMin, windowRect.yMin, 0);
        Vector3 actualWindowCorner2 = new Vector3(windowRect.xMax, windowRect.yMin, 0);
        Vector3 actualWindowCorner3 = new Vector3(windowRect.xMax, windowRect.yMax, 0);
        Vector3 actualWindowCorner4 = new Vector3(windowRect.xMin, windowRect.yMax, 0);

        Debug.DrawLine(actualWindowCorner1, actualWindowCorner2, yColorA);
        Debug.DrawLine(actualWindowCorner2, actualWindowCorner3, xColorB);
        Debug.DrawLine(actualWindowCorner3, actualWindowCorner4, yColorB);
        Debug.DrawLine(actualWindowCorner4, actualWindowCorner1, xColorA);

        // And now we display the camera limits. If the camera is inactive, they will show in red.
        // There is an x in the middle of the screen to show what hits against the limit.
        if (limitCameraMovement)
        {
            Color limitColor;

            if (!activeTracking)
                limitColor = Color.red;
            else
                limitColor = Color.cyan;

            Vector3 limitCorner1 = new Vector3(limitLeft, limitBottom, 0);
            Vector3 limitCorner2 = new Vector3(limitRight, limitBottom, 0);
            Vector3 limitCorner3 = new Vector3(limitRight, limitTop, 0);
            Vector3 limitCorner4 = new Vector3(limitLeft, limitTop, 0);

            Debug.DrawLine(limitCorner1, limitCorner2, limitColor);
            Debug.DrawLine(limitCorner2, limitCorner3, limitColor);
            Debug.DrawLine(limitCorner3, limitCorner4, limitColor);
            Debug.DrawLine(limitCorner4, limitCorner1, limitColor);

            //And a little center point

            Vector3 centerMarkCorner1 = new Vector3(cameraPos.x - 0.1f, cameraPos.y + 0.1f, 0);
            Vector3 centerMarkCorner2 = new Vector3(cameraPos.x + 0.1f, cameraPos.y - 0.1f, 0);
            Vector3 centerMarkCorner3 = new Vector3(cameraPos.x - 0.1f, cameraPos.y - 0.1f, 0);
            Vector3 centerMarkCorner4 = new Vector3(cameraPos.x + 0.1f, cameraPos.y + 0.1f, 0);

            Debug.DrawLine(centerMarkCorner1, centerMarkCorner2, Color.cyan);
            Debug.DrawLine(centerMarkCorner3, centerMarkCorner4, Color.cyan);
        }
    }

    // Public functions start here. These are for other objects/scripts to communicate with the camera.

    // Use this to change/activate level limits.
    public void ActivateLimits(float leftLimit, float rightLimit, float bottomLimit, float topLimit)
    {
        limitLeft = leftLimit;
        limitRight = rightLimit;
        limitBottom = bottomLimit;
        limitTop = topLimit;

        ValidateLeftAndRightLimits();
        ValidateTopAndBottomLimits();

        limitCameraMovement = true;
    }

    // No longer use limits.
    public void DeactivateLimits()
    {
        limitCameraMovement = false;
    }

    // I'd use this next one for transitions between areas. Probably best used with a new set of limits 
    // immediately following. You don't have to wait until the movement is over to activate new limits. It 
    // will hold off on using the new limits until the movement is over.
    //
    // Usage example initating a transition to a new vertical area from the perspective of a door gameobject:
    //-----------------------------------------------------------------------------
    //  CameraControl cameraScript = Camera.main.GetComponent<CameraControl>();
    //  Vector3 target = new Vector3 (60, 10, 0);
    //  cameraScript.MoveCamera( target, 5f);
    //  cameraScript.ActivateLimits( 60, 60, 10, 100 );
    //-----------------------------------------------------------------------------

    public void MoveCamera(Vector3 targetPosition, float moveSpeed)
    {
        StartCoroutine(MoveToPosition(targetPosition, moveSpeed));
    }

    // This coroutine disables the player tracking to move to a given position, then turns the player 
    // tracking back on. You'll likely have to change this method if you're doing pixel perfect stuff.
    IEnumerator MoveToPosition(Vector3 targetPosition, float moveSpeed)
    {
        activeTracking = false;

        //Assumes 2D usage
        targetPosition.z = transform.position.z;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return 0;
        }

        activeTracking = true;
    }

}