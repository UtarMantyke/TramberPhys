using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerActions : PlayerActionSet
{
    public PlayerAction Fire;
    
    public PlayerAction Left;
    public PlayerAction Right;


    public MyPlayerActions()
    {
        Fire = CreatePlayerAction("Fire");
       
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
    }


    public static MyPlayerActions CreateWithDefaultBindings()
    {
        var playerActions = new MyPlayerActions();

        // How to set up mutually exclusive keyboard bindings with a modifier key.
        // playerActions.Back.AddDefaultBinding( Key.Shift, Key.Tab );
        // playerActions.Next.AddDefaultBinding( KeyCombo.With( Key.Tab ).AndNot( Key.Shift ) );


        playerActions.Fire.AddDefaultBinding(InputControlType.Action1);
        playerActions.Left.AddDefaultBinding(Key.LeftArrow);
        playerActions.Right.AddDefaultBinding(Key.RightArrow);


        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 4;
        //playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
        //playerActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
        playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        //playerActions.ListenOptions.IncludeMouseButtons = true;
        //playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
        //playerActions.ListenOptions.IncludeMouseButtons = true;
        //playerActions.ListenOptions.IncludeMouseScrollWheel = true;

        playerActions.ListenOptions.OnBindingFound = (action, binding) =>
        {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerActions.ListenOptions.OnBindingAdded += (action, binding) =>
        {
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
        };

        playerActions.ListenOptions.OnBindingRejected += (action, binding, reason) =>
        {
            Debug.Log("Binding rejected... " + reason);
        };

        return playerActions;
    }
}

