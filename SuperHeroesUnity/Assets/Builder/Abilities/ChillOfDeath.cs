using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int ChillOfDeathRange = 0;
	
	public static int ChillOfDeathSplash = 3;
	
	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool ChillOfDeathOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string ChillOfDeathDescription = "An icy storm surrounds the character";

	/// <summary>
	/// Stomp Ability, hurts enemies within a 2 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void ChillOfDeath(Character myChar, GridHandler map)
	{
		
		//EXAMPLE: AOE with range of 2
		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		choices[0] = myChar.MyLocation.FindPossibleMoves(-1, 3);
        choices[0].Remove(myChar.MyLocation);
		//IMPORTANT
		//push this so that the UI can access your stuff <3 -Sean
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, ChillOfDeathEffect));
	}
	
	public static void ChillOfDeathEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, 5, DamageType.Cold);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}