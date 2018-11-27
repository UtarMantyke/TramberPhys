using BindingsExample;
using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    PlayerActions playerActions;

    public Rigidbody2D shipBody;
    public GameObject fireLeft;
    public GameObject fireRight;
    public GameObject forceThroughAnchor;

    public float engineStrenth = 15.0f;

    // Use this for initialization
    void Start()
    {

        playerActions = CreateWithDefaultBindings();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngine();

    }


    void UpdateEngine()
    { 
        var strenth = engineStrenth;
        if (playerActions.Left.IsPressed)
        {
            var dir = forceThroughAnchor.transform.position - fireLeft.transform.position;
            dir.Normalize();            
            shipBody.AddForceAtPosition(dir * strenth, fireLeft.transform.position);
            fireLeft.SetActive(true);
        }
        else
        {
            fireLeft.SetActive(false);
        }


        if (playerActions.Right.IsPressed)
        {
            var dir = forceThroughAnchor.transform.position - fireRight.transform.position;
            dir.Normalize();
            shipBody.AddForceAtPosition(dir * strenth, fireRight.transform.position);
            fireRight.SetActive(true);
        }
        else
        {
            fireRight.SetActive(false);
        }
    }


    public static PlayerActions CreateWithDefaultBindings()
    {
        var playerActions = new PlayerActions();

        // How to set up mutually exclusive keyboard bindings with a modifier key.
        // playerActions.Back.AddDefaultBinding( Key.Shift, Key.Tab );
        // playerActions.Next.AddDefaultBinding( KeyCombo.With( Key.Tab ).AndNot( Key.Shift ) );

        playerActions.Fire.AddDefaultBinding(Key.A);
        playerActions.Fire.AddDefaultBinding(InputControlType.Action1);
        playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);

        playerActions.Jump.AddDefaultBinding(Key.Space);
        playerActions.Jump.AddDefaultBinding(InputControlType.Action3);
        playerActions.Jump.AddDefaultBinding(InputControlType.Back);

        playerActions.Up.AddDefaultBinding(Key.UpArrow);
        playerActions.Down.AddDefaultBinding(Key.DownArrow);
        playerActions.Left.AddDefaultBinding(Key.LeftArrow);
        playerActions.Right.AddDefaultBinding(Key.RightArrow);

        playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);

        return playerActions;
    }
}
