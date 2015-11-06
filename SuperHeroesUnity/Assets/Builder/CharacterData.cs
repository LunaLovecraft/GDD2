using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MovementType {
    Ground = 0,
    Flying = 1
}

/// <summary>
/// Used to store character data for transfer
/// </summary>
public struct CharacterData
{
    public int health;
    public int speed;
    public MovementType movement;
    public List<Ability> abilities;
    public List<Trait> traits;

    public CharacterData(int health, int speed, MovementType movement, List<Ability> abilities, List<Trait> traits){
        this.health = health;
        this.speed = speed;
        this.movement = movement;
        this.abilities = abilities;
        this.traits = traits;
    }
}


public static partial class CharacterList{
    

    // This dictionary is used to gather all of the characters by name so we can spawn them easily.
    public static Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

    // All character data added to the game has to be compounded here.
    // All characters have a health, speed, movement, list of abilities, and list of traits.
    public static void Initialize()
    {
        characters.Add("Example", new CharacterData(ExampleHealth, ExampleSpeed, ExampleMovement, ExampleAbilities, ExampleTraits));
    }

    /// <summary>
    /// Example character creation.  All characters have to be implemented inside of the partial class CharacterList
    /// </summary>
   
    // Start by setting up the health, speed, and movementtype of the character
    public static int ExampleHealth = 100;
    public static int ExampleSpeed = 4;
    public static MovementType ExampleMovement = MovementType.Ground;

    // Store all abilities the character has in this format.
    public static List<Ability> ExampleAbilities = new List<Ability>()
    {
        delegate(Character myChar, GridHandler map){Abilities.Example(myChar, map);} //You can use a comma to include multiple abilities.

    };

    // Stores all of the traits on the character.
    public static List<Trait> ExampleTraits = new List<Trait>(){
        delegate(Character myChar){Traits.Init_Example(myChar);} //Like above, you can use a comma to include multiple traits.
    };

}
