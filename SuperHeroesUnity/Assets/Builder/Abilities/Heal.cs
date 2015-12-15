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
    public static int HealRange = 7;

    public static int HealSplash = 0;

	public static int HealDamage = 4;


    /// <summary>
    /// Offensive info
    /// </summary>
    public static bool HealOffensive = false;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string HealDescription = "System calls Ally.health++";

   

	/// <summary>
	/// Heal Ability
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void Heal(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		foreach(Node tile in map.map)
		{
			if(tile.myCharacter != null && tile.myCharacter.Faction == myChar.Faction)
			{
				choices[0].Add(tile);
			}
		}

        UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, myChar.Faction, HealEffect));
	}
	
	public static void HealEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
            if (affectedNodes[i].myCharacter != null && affectedFaction == affectedNodes[i].myCharacter.Faction)
			{
				affectedNodes[i].myCharacter.HealCharacter(HealDamage);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}