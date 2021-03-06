﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTools  {


    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

    public static void SendFSMEvent(this GameObject go, string eventName)
    {
        foreach(var fsm in go.GetComponents<PlayMakerFSM>())
        {
            fsm.SendEvent(eventName);
        }
    }

}
