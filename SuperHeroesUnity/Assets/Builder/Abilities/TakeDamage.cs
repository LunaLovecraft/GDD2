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
    public static int TakeDamageRange = 15;

	public static int TakeDamageSplash = 0;

	public static int TakeDamageDamage = 3;

    /// <summary>
    /// Offensive info
    /// </summary>
	public static bool TakeDamageOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string TakeDamageDescription = "System calls Enemy.TakeDamage()";

   

	/// <summary>
	/// TakeDamage
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void TakeDamage(Character myChar, GridHandler map)
	{

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
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, TakeDamageEffect));
	}
	
	public static void TakeDamageEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
			if (affectedNodes[i].myCharacter != null && affectedFaction != myChar.Faction)
			{
				affectedNodes[i].myCharacter.DamageCharacter(TakeDamageDamage,DamageType.Electric);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}