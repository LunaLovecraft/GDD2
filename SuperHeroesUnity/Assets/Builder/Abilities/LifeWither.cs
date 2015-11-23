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
    public static int LifeWitherRange = 15;

	public static int LifeWitherSplash = 0;

    /// <summary>
    /// Offensive info
    /// </summary>
	public static bool LifeWitherOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string LifeWitherDescription = "Drains health from the enemy";

   

	/// <summary>
	/// Heal Ability, hurts enemies within a 2 tile radius of the player
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void LifeWither(Character myChar, GridHandler map)
	{
		
		//EXAMPLE: AOE with range of 2
		//create a list of the possible moves, and the nodes they affect
		List<List<Node>> choices = new List<List<Node>>();
		
		//add all of the nodes within 2 Moves of the player
		foreach(Node tile in map.map)
		{
			if(tile.myCharacter != null && tile.myCharacter.Faction != myChar.Faction)
			{
                //Add a list of nodes to the previous list, representing a set of new affected nodes;
                choices.Add(new List<Node>() { tile} );
			}
		}
		
		//IMPORTANT
		//push this so that the UI can access your stuff <3 -Sean
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 0, LifeWitherEffect));
	}
	
	public static void LifeWitherEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				affectedNodes[i].myCharacter.DamageCharacter(2,DamageType.Cold);
				myChar.HealCharacter(2);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}