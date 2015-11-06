using UnityEngine;
using System.Collections;

public delegate void Ability(Character myChar, GridHandler map);

/// <summary>
/// Example Ability implementation.
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

    /// <summary>
    /// Example Ability Implementation.  All ablities must be part of the static partial class Abilities.
    /// </summary>
    public static void Example(Character myChar, GridHandler map)
    {
        Debug.Log("This is an example Ability.");
    }

}