using UnityEngine;
using System.Collections;

public delegate void Ability(Character myChar, GridHandler map);

/// <summary>
/// Inside are defined any helper functions Abilities uses
/// </summary>
public static partial class Abilities{

    /// <summary>
    /// Gets an ability based on a string.  Can be used for loading purposes.
    /// </summary>
    /// <param name="abilityName">The ability being looked for</param>
    /// <returns>An Ability delegate</returns>
    public static Ability GetAbility(string abilityName)
    {
        return (Ability)Ability.CreateDelegate(typeof(Ability), typeof(Abilities).GetMethod(abilityName));
    }
}