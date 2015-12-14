using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class Abilities{

	/// <summary>
	/// Range info
	/// </summary>
	public static int OverrunRange = 0;
	
	public static int OverrunSplash = 6;

	public static int OverrunDamage = 3;
	

	/// <summary>
	/// Offensive info
	/// </summary>
	public static bool OverrunOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string OverrunDescription = "Enemies are overrun by a swarm of rodents";

	/// <summary>
	/// Chill Of Death Ability, hurts enemies within a 3 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void Overrun(Character myChar, GridHandler map)
	{

		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();

		//Add a list of nodes to the previous list, representing a set of new affected nodes;
		choices.Add(new List<Node>());

		//add all of the nodes within 2 Moves of the player
		choices[0] = myChar.MyLocation.FindPossibleMoves(-1, OverrunSplash);
        choices[0].Remove(myChar.MyLocation);

		//Push so that the UI can access
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 0, OverrunEffect));
	}
	
	public static void OverrunEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				myChar.DamageOtherCharacter(affectedNodes[i].myCharacter, OverrunDamage, DamageType.Physical);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}