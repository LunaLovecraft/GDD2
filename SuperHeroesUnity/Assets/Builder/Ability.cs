using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Ability(Character myChar, GridHandler map);
public delegate void AbilityAddition(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes);

public enum AbilityInfo{
    Range,
    Offensive,
    Splash,
	Description
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

			case AbilityInfo.Description:
				infoType = "Description";
				break;

            default:
                infoType = "Range";
                break;
        }

        return (int)typeof(Abilities).GetField(abilityName + infoType).GetValue(null);
    }
}