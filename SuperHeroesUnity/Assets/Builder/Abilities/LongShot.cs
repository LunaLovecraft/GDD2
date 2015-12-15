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
    public static int LongShotRange = 15;

	public static int LongShotSplash = 0;

	public static int LongShotDamage = 4;

    /// <summary>
    /// Offensive info
    /// </summary>
	public static bool LongShotOffensive = true;

	/// <summary>
	/// Ability Description
	/// </summary>
	public static string LongShotDescription = "The enemy is hit by a careful sniper shot";

   

	/// <summary>
	/// Life Wither
	/// </summary>
	/// <param name="myChar"></param>
	/// <param name="map"></param>
	public static void LongShot(Character myChar, GridHandler map)
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
		UIInformationHandler.InformationStack.Push(new UIInformation(myChar, map, choices, 1, LongShotEffect));
	}
	
	public static void LongShotEffect(Character myChar, GridHandler map, int affectedFaction, List<Node> affectedNodes)
	{
		for (int i = 0; i < affectedNodes.Count; ++i)
		{
            if (affectedNodes[i].myCharacter != null && affectedFaction == affectedNodes[i].myCharacter.Faction)
			{
				affectedNodes[i].myCharacter.DamageCharacter(LongShotDamage,DamageType.Physical);
			}
		}
        UIInformationHandler.InformationStack.Clear();
        myChar.canAct = false;
	}
}