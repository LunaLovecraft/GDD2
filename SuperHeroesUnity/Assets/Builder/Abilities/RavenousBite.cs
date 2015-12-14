using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int RavenousBiteRange = 2;
	
	public static int RavenousBiteSplash = 1;

	public static int RavenousBiteDamage = 4;
	

	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool RavenousBiteOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string RavenousBiteDescription = "The player savagely bites the target and drinks their blood";

	/// <summary>
	/// Chill Of Death Ability, hurts enemies within a 3 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void RavenousBite(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());
		//add all of the nodes within 2 Moves of the player
		List<Node> allSpace = myChar.MyLocation.FindPossibleMoves(-1, RavenousBiteRange);
		foreach(Node tile in allSpace)
		{
			if(tile.myCharacter != null && tile.myCharacter.Faction != myChar.Faction)
			{
				List<Node> target = tile.FindPossibleMoves(-1, RavenousBiteSplash);
				choices.Add(target);
				
			}
		}

		//Push so that the UI can access
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 0, RavenousBiteEffect));
	}
	
	public static void RavenousBiteEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, RavenousBiteDamage, DamageType.Physical);
				myChar.HealCharacter(2);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}