using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_Giant(Character myChar)
    {
        myChar.OnDamaged += new EventHandler(Event_Giant);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_Giant(object sender, EventArgs e)
    {
		DamageEventArgs data = (DamageEventArgs)e;
		if (data.type == DamageType.Physical) {
			data.damage/=2;
		}
    }
}