using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Ability(Character myChar, GridHandler map);
public delegate void AbilityAddition(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes);

public enum AbilityInfo{
    Range,
    Offensive,
    Splash
}

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

    public static object GetAbilityInfo(string abilityName, AbilityInfo infoWanted)
    {
        string infoType;
        switch (infoWanted)
        {
            case AbilityInfo.Offensive:
                infoType = "Offensive";
                break;

            case AbilityInfo.Range:
                infoType = "Range";
                break;

            case AbilityInfo.Splash:
                infoType = "Splash";
                break;

            default:
                infoType = "Range";
                break;
        }

        return (int)typeof(Abilities).GetField(abilityName + infoType).GetValue(null);
    }

    public static List<Node> TilesInRange(Node position, string abilityName)
    {
        int range = (int)GetAbilityInfo(abilityName, AbilityInfo.Range);

        List<Node> tiles = position.FindPossibleMoves((int)MovementType.Flying, range);

        return tiles;
    }

    /// <summary>
    /// Find Characters in Range of an abillity
    /// </summary>
    /// <param name="position"></param>
    /// <param name="abilityName"></param>
    /// <returns></returns>
    public static List<Character> CharactersInRange(Node position, string abilityName)
    {
        int range = (int)GetAbilityInfo(abilityName, AbilityInfo.Range);

        List<Node> tiles = position.FindPossibleMoves((int)MovementType.Flying, range);
        List<Character> characters = new List<Character>();

        foreach(Node tile in tiles)
        {
            if(tile.myCharacter != null)
            {
                characters.Add(tile.myCharacter);
            }
        }

        return characters;
    }


    /// <summary>
    /// Used to find tiles effected by an AOE.
    /// </summary>
    /// <param name="position">The center of the effect.</param>
    /// <param name="abilityName">The name of the ability</param>
    /// <returns></returns>
    public static List<Node> TilesEffected(Node position, string abilityName)
    {
        int range = (int)GetAbilityInfo(abilityName, AbilityInfo.Splash);

        List<Node> tiles = position.FindPossibleMoves((int)MovementType.Flying, range);

        return tiles;
    }
}