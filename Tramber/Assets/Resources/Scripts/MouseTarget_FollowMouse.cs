using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using Tobii.Gaming.Internal;
using UnityEngine;
using UnityEngine.UI;

public class MouseTarget_FollowMouse : MonoBehaviour
{

    public Transform connectedTransform;

    public GameObject gazePlot;
    public Vector3 preferredPosi;

    public Text testText;

    bool tobiiIsConnected = true;

    private void Update()
    {

        // don't know why TobiiAPI.IsConnected swing between false and true when it not connected


        if(Time.time > 2.0f)
        {
            if (!TobiiAPI.IsConnected)
                tobiiIsConnected = false;
        }


        //testText.text = TobiiAPI.IsConnected + " " + tobiiIsConnected;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get the mouse position in pixel coordinates
        Vector2 mousePositionInScreen = Input.mousePosition;

        //clamp the mouse position so it's limited to the window bounds
        mousePositionInScreen.x = Mathf.Clamp(mousePositionInScreen.x, 0f, Camera.main.pixelWidth);
        mousePositionInScreen.y = Mathf.Clamp(mousePositionInScreen.y, 0f, Camera.main.pixelHeight);


        var targetPosi = mousePositionInScreen;            

        //now translate this position to world coordinates
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(targetPosi);

        if (LevelManager.Instance.useEyeControl)
        {
            if (tobiiIsConnected)
            {
                if (gazePlot)
                    mousePositionInWorld = gazePlot.transform.position;
            }
        }

        // mousePositionInWorld = gazePlot.transform.position;

        preferredPosi = mousePositionInWorld;

        //move the mouse target to the mouse position
        transform.position = mousePositionInWorld;        
    }

    //Draw a line to the bat to show where this is pulling from
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, connectedTransform.position);
    }
}
