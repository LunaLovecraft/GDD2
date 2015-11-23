using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Example Ability implementation.
/// </summary>
public static partial class Abilities
{

    /// <summary>
    /// Range info
    /// </summary>
    public static int ExampleRange = 0;

    public static int ExampleSplash = 0;

    /// <summary>
    /// Offensive info
    /// </summary>
    public static bool ExampleOffensive = true;

    public static string ExampleDescription = "This is a sample Description";

   

    /// <summary>
    /// Example Ability Implementation.  All ablities must be part of the static partial class Abilities.
    /// </summary>
    public static void Example(Character myChar, GridHandler map)
    {
        List<List<Node>> choices = new List<List<Node>>();
        choices.Add(new List<Node>());
        choices[0].Add(myChar.MyLocation);
        choices[0].Add(map.map[myChar.MyLocation.Y, myChar.MyLocation.X -1]);
        choices.Add(new List<Node>());
        if(myChar.MyLocation.X + 1 < map.Width)
        choices[1].Add(map.map[myChar.MyLocation.Y, myChar.MyLocation.X + 1]);
        if (myChar.MyLocation.X + 2 < map.Width)
        choices[1].Add(map.map[myChar.MyLocation.Y, myChar.MyLocation.X + 2]);


        UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, ExamplePartTwo));
    }

    public static void ExamplePartTwo(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
    {
        for (int i = 0; i < affectedNodes.Count; ++i)
        {
            Debug.Log("Attacking with Example at " + affectedNodes[i].X + ", " + affectedNodes[i].Y);
            if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
            {
                affectedNodes[i].myCharacter.DamageCharacter(10, DamageType.Fire);
            }
        }
    }

}