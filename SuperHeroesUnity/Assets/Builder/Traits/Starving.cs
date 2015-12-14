using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_Starving(Character myChar)
    {
        myChar.OnEndTurn += new EventHandler(Event_Starving);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_Starving(object sender, EventArgs e)
    {
		CharEventArgs data = (CharEventArgs)e;
		data.myChar.DamageCharacter (1, DamageType.Physical);
    }
}