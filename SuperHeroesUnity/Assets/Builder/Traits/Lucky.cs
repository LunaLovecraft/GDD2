using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_Lucky(Character myChar)
    {
        myChar.OnDealDamage += new EventHandler(Event_Lucky);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_Lucky(object sender, EventArgs e)
    {
		DamageEventArgs data = (DamageEventArgs)e;

		if (data.damage > 0) {
			data.damage-=1;
		}
    }
}