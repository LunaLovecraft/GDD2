using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_Example(Character myChar)
    {
        myChar.OnSpawn += new EventHandler(Event_Example);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_Example(object sender, EventArgs e)
    {
        Debug.Log("This is an example trait");
    }
}