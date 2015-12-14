using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_ToxicTouch(Character myChar)
    {
        myChar.OnDamaged += new EventHandler(Event_ToxicTouch);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_ToxicTouch(object sender, EventArgs e)
    {
		DamageEventArgs data = (DamageEventArgs)e;
		data.myChar.DamageCharacter (2);
    }
}