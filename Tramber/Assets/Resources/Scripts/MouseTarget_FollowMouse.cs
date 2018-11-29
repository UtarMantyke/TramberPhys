using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;


public class MouseTarget_FollowMouse : MonoBehaviour
{

    public Transform connectedTransform;

    // Update is called once per frame
    void Update()
    {
        //get the mouse position in pixel coordinates
        Vector2 mousePositionInScreen = Input.mousePosition;

        //clamp the mouse position so it's limited to the window bounds
        mousePositionInScreen.x = Mathf.Clamp(mousePositionInScreen.x, 0f, Camera.main.pixelWidth);
        mousePositionInScreen.y = Mathf.Clamp(mousePositionInScreen.y, 0f, Camera.main.pixelHeight);


        var targetPosi = mousePositionInScreen;
        if(LevelManager.Instance.useEyeControl)
        {
            if(TobiiAPI.IsConnected)
            {
                GazePoint gazePoint = TobiiAPI.GetGazePoint();
                targetPosi = gazePoint.Screen;
            }
        }
            

        //now translate this position to world coordinates
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(targetPosi);

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
