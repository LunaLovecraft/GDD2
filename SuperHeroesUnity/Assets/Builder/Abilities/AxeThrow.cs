using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int AxeThrowRange = 6;
	
	public static int AxeThrowSplash = 3;
	
	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool AxeThrowOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string AxeThrowDescription = "Throws a terrifying axe";

	/// <summary>
	/// Axe Throw
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void AxeThrow(Character myChar, GridHandler map)
	{
		
		//EXAMPLE: AOE with range of 2
		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		List<Node> allSpace = myChar.MyLocation.FindPossibleMoves((int)MovementType.Ground, 2);
		foreach(Node tile in allSpace)
		{
			if(tile.myCharacter != null && tile.myCharacter.Faction != myChar.Faction)
			{
				List<Node> target = new List<Node>();
				target.Add(tile);
				choices.Add(target);
				
			}
		}
		//IMPORTANT
		//push this so that the UI can access your stuff <3 -Sean
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, AxeThrowEffect));
	}
	
	public static void AxeThrowEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, 5, DamageType.Physical);
			}
		}
		//UIInformationHandler.InformationStack.Clear ();
	}
}