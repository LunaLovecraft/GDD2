using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int GrenadeRange = 5;
	
	public static int GrenadeSplash = 4;

	public static int GrenadeDamage = 6;

	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool GrenadeOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string GrenadeDescription = "Throws a lit grenade";

	/// <summary>
	/// Grenade
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void Grenade(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		List<Node> allSpace = myChar.MyLocation.FindPossibleMoves(-1, GrenadeRange);
		foreach(Node tile in allSpace)
		{
			if(tile.myCharacter != null && tile.myCharacter.Faction != myChar.Faction)
			{
                List<Node> target = tile.FindPossibleMoves(-1, GrenadeSplash);
				choices.Add(target);
				
			}
		}

		//push this so that the UI can access your stuff
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, GrenadeEffect));
	}
	
	public static void GrenadeEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
            if (affectedNodes[i].myCharacter != null && affectedFaction == affectedNodes[i].myCharacter.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, GrenadeDamage, DamageType.Fire);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}