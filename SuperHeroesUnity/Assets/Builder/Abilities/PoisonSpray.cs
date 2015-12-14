using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int PoisonSprayRange = 0;
	
	public static int PoisonSpraySplash = 4;

	public static int PoisonSprayDamage = 4;
	

	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool PoisonSprayOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string PoisonSprayDescription = "A toxic gas sprays at the enemies";

	/// <summary>
	/// Chill Of Death Ability, hurts enemies within a 3 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void PoisonSpray(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();

		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());

		//add all of the nodes within 2 Moves of the player
		choices[0] = myChar.MyLocation.FindPossibleMoves(-1, PoisonSpraySplash);
        choices[0].Remove(myChar.MyLocation);

		//Push so that the UI can access
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 0, PoisonSprayEffect));
	}
	
	public static void PoisonSprayEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, PoisonSprayDamage, DamageType.Poison);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}