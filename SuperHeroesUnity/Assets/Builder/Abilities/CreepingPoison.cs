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
    public static int CreepingPoisonRange = 0;

	public static int CreepingPoisonSplash = 10;

	public static int CreepingPoisonDamage = 1;

    /// <summary>
    /// Offensive info
    /// </summary>
	public static bool CreepingPoisonOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string CreepingPoisonDescription = "A slow gas overwhelms the enemy team";

   

	/// <summary>
	/// CreepingPoison
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void CreepingPoison(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		
		//add all of the nodes within 2 Moves of the player
		choices[0] = myChar.MyLocation.FindPossibleMoves(1, CreepingPoisonSplash);
		choices[0].Remove(myChar.MyLocation);
		
		//Push so that the UI can access

		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 0, CreepingPoisonEffect));
	}
	
	public static void CreepingPoisonEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				affectedNodes[i].myCharacter.DamageCharacter(CreepingPoisonDamage,DamageType.Poison);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}