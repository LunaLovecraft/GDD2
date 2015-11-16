using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Example Ability implementation.
/// </summary>
public static partial class Abilities
{

    /// <summary>
    /// Example Ability Implementation.  All ablities must be part of the static partial class Abilities.
    /// </summary>
    public static void Example(Character myChar, GridHandler map)
    {
        List<List<Node>> choices = new List<List<Node>>();
        choices.Add(new List<Node>());
        choices[0].Add(myChar.MyLocation);

        //UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, ExamplePartTwo));
    }

    public static void ExamplePartTwo(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
    {
        for (int i = 0; i < affectedNodes.Count; ++i)
        {
            if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
            {
                affectedNodes[i].myCharacter.DamageCharacter(10, DamageType.Fire);
            }
        }
    }

}