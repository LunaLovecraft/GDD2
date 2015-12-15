using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int StompRange = 0;
	
	public static int StompSplash = 2;

	public static int StompDamage = 7;
	
	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool StompOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string StompDescription = "An earth shaking stomp";

	/// <summary>
	/// Stomp Ability, hurts enemies within a 2 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void Stomp(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		choices[0] = myChar.MyLocation.FindPossibleMoves(-1, StompSplash);
        choices[0].Remove(myChar.MyLocation);

		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, StompEffect));
	}
	
	public static void StompEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
            if (affectedNodes[i].myCharacter != null && affectedFaction == affectedNodes[i].myCharacter.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, StompDamage, DamageType.Physical);
			}
		}
		UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}