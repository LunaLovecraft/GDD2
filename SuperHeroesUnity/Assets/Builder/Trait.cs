using UnityEngine;
using System;
using System.Collections;

public delegate void Trait(Character myChar);


public static partial class Traits
{
    /// <summary>
    /// Gets an ability based on a string.  Can be used for loading purposes.
    /// </summary>
    /// <param name="abilityName">The ability being looked for</param>
    /// <returns>An Ability delegate</returns>
    public static Trait GetTrait(string traitName)
    {
        return (Trait)Trait.CreateDelegate(typeof(Trait), typeof(Traits).GetMethod(traitName));
    }

    /// <summary>
    /// Example Trait Implementation.  All traits must be part of the static partial class Traits.
    /// </summary>

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