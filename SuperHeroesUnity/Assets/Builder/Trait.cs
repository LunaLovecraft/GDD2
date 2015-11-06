using UnityEngine;
using System;
using System.Collections;

public delegate void Trait(Character myChar);

/// <summary>
/// Contains any helper methods that Traits use.
/// </summary>
public static partial class Traits
{
    /// <summary>
    /// Gets an ability based on a string.  Can be used for loading purposes.
    /// </summary>
    /// <param name="abilityName">The ability being looked for</param>
    /// <returns>An Ability delegate</returns>
    public static Trait GetTrait(string traitName)
    {
        return (Trait)Trait.CreateDelegate(typeof(Trait), typeof(Traits).GetMethod("Init_" + traitName));
    }
}