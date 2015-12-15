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


public static partial class CharacterInfoList{
    

    // This dictionary is used to gather all of the characters by name so we can spawn them easily.
    public static Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

    // All character data added to the game has to be compounded here.
    // All characters have a health, speed, movement, list of abilities, and list of traits.
    public static void Initialize()
    {
        // Characters that are hardcoded for some reason can be loaded in here, but try to avoid it.
        LoadCharacters();
    }

    /// <summary>
    /// Loads characters from text files in the characters folder.
    /// </summary>
    public static void LoadCharacters()
    {
        System.IO.DirectoryInfo myDirectory = new System.IO.DirectoryInfo("Assets/Builder/Characters");
        System.IO.FileInfo[] myFiles = myDirectory.GetFiles();
        for (int i = 0; i < myFiles.Length; ++i)
        {
            if (myFiles[i].Name.Contains("meta"))
            {
                continue;
            }

            // Setup all the data we need to gather.
            string name = "";
            int health = 0;
            int speed = 0;
            MovementType movement = MovementType.Ground;
            List<Ability> abilities = new List<Ability>();
            List<Trait> traits = new List<Trait>();

            System.IO.StreamReader myReader = new System.IO.StreamReader(myFiles[i].FullName);
            name = myReader.ReadLine();

            // Read in health
            int.TryParse(myReader.ReadLine(), out health);
            // Read in speed
            int.TryParse(myReader.ReadLine(), out speed);

            // Read in movement type
            switch(myReader.ReadLine().ToLower()){
                case "ground":
                    movement = MovementType.Ground;
                    break;

                case "flying":
                    movement = MovementType.Flying;
                    break;
            }

            // Read in the list of abilities.
            string line;
            myReader.ReadLine(); //Go past the abilities section starter
            while ((line = myReader.ReadLine()) != "Traits")
            {
                abilities.Add(Abilities.GetAbility(line));
            }
            // Read in the list of traits
            while((line = myReader.ReadLine()) != null){
                traits.Add(Traits.GetTrait(line));
            }
            
            // Add it to the character list.
            characters.Add(name, new CharacterData(health, speed, movement, abilities, traits));
        }
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
