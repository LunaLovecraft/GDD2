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
    public static int HealRange = 15;

    public static int HealSplash = 0;

    /// <summary>
    /// Offensive info
    /// </summary>
    public static bool HealOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string HealDescription = "Heals the player";

   

	/// <summary>
	/// Heal Ability, hurts enemies within a 2 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void Heal(Character myChar, GridHandler map)
	{
		
		//EXAMPLE: AOE with range of 2
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
		
		//IMPORTANT
		//push this so that the UI can access your stuff <3 -Sean
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, HealEffect));
	}
	
	public static void HealEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				affectedNodes[i].myCharacter.HealCharacter(5);
			}
		}
		//UIInformationHandler.InformationStack.Clear ();
	}
}