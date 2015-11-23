using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Example Trait Implementation.  All traits must be part of the static partial class Traits.
/// </summary>
public static partial class Traits
{   

    // This function will be called to initialize the trait to the proper event handlers.
    public static void Init_WatchfulEye(Character myChar)
    {
        myChar.OnDealDamage += new EventHandler(Event_WatchfulEye);
    }

    // If a trait causes multiple events, then it can have multiple functions to it.
    public static void Event_WatchfulEye(object sender, EventArgs e)
    {
		DamageEventArgs data = (DamageEventArgs)e;

		if (UnityEngine.Random.Range(0,4) == 3) {
			data.damage*=1.25;
		}
    }
}