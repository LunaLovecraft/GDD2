using UnityEngine;
using System.Collections;

public delegate void Ability(Character myChar, GridHandler map);

/// <summary>
/// Example Ability implementation.
/// </summary>
public static partial class Abilities{

    // This function will be called when the power is used.
    public static void Example(Character myChar, GridHandler map)
    {
        Debug.Log("This is an example Ability.");
    }
}