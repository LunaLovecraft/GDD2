using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Possible errors on a character creation
/// </summary>
public enum CHAR_INIT
{
    SUCCESS = 0,
    CHAR_AT_LOC_ERROR = 1,
    CHAR_DOESNT_EXIST = 2
}

/// <summary>
/// Any damage types in the game.
/// </summary>
public enum DamageType
{
    Physical,
    Fire,
    Cold,
    Electric
}

/// <summary>
/// Any conditions in the game.
/// </summary>
public enum Condition
{
    Slow,
    Fast
}

/// <summary>
/// Any unit on the map.
/// </summary>
public partial class Character{

    private int y;
    private int x;
    private Node myLocation;
    private GridHandler map;
    private string name;

    private List<Ability> myAbilities;
    private int health;
    private int maxHealth;
    private int speed;
    private MovementType movement;
    private Dictionary<Condition, int> myConditions;

    private int faction;

    public int Y { get { return y; } }
    public int X { get { return x; } }
    public Node MyLocation { get { return myLocation; } }
    public string Name { get { return name; } }
    public int Health { get { return health; } }
    public int Speed { get { return speed; } }
    public MovementType Movement { get { return movement; } }
    public int Faction { get { return faction; } }

    /// <summary>
    /// Any unit on the map
    /// </summary>
    /// <param name="map">The map the character is on</param>
    /// <param name="name">The characters name, for use in the lookup dictionary</param>
    public Character(GridHandler map, string name)
    {
        this.y = 0;
        this.x = 0;
        this.myLocation = null;
        this.map = map;
        this.myAbilities = null;
        this.name = name;
        this.myAbilities = null;
        this.health = 0;
        this.speed = 0;
        this.movement = MovementType.Ground;
        this.myConditions = null;
        this.faction = 0;
    }

    /// <summary>
    /// Called to formally create the object in the scene.
    /// </summary>
    /// <param name="y">Y position to spawn the character</param>
    /// <param name="x">X position to spawn the character</param>
    /// <returns>Any error codes</returns>
    public CHAR_INIT Create(int y, int x, int faction)
    {
        if (map.map[y, x].myCharacter != null)
        {
            return CHAR_INIT.CHAR_AT_LOC_ERROR;
        }

        this.y = y;
        this.x = x;
        myLocation = map.map[y, x];
        map.map[y, x].myCharacter = this;

        // Load the abilities and such
        if (!CharacterList.characters.ContainsKey(name))
        {
            return CHAR_INIT.CHAR_DOESNT_EXIST;
        }
        CharacterData myData = CharacterList.characters[name];
        this.health = myData.health;
        this.maxHealth = myData.health;
        this.speed = myData.speed;
        this.movement = myData.movement;
        this.myConditions = new Dictionary<Condition, int>();
        this.myAbilities = myData.abilities;
        this.faction = faction;

        // Add all the traits to the character's event handlers
        List<Trait> myTraits = myData.traits;
        for (int i = 0; i < myTraits.Count; ++i)
        {
            myTraits[i](this);
        }

        // Call Spawn event.
        E_Spawn(new CharEventArgs(this));

        return CHAR_INIT.SUCCESS;
    }

    /// <summary>
    /// Damages a character for a specified amount
    /// </summary>
    /// <param name="damage">The amount to damage</param>
    /// <param name="type">The type of the damage</param>
    public void DamageCharacter(int damage, DamageType type = DamageType.Physical){
        // Call the damaged event.
        E_Damaged(new DamageEventArgs(this, ref type, ref damage));

        this.health -= damage;

        // If health is below zero, call the dead event.
        if (health <= 0)
        {
            E_Killed(new CharEventArgs(this));
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /// <summary>
    /// Heals a character for a specified amount.
    /// </summary>
    /// <param name="heal">The amount to heal</param>
    public void HealCharacter(int heal)
    {
        // Call the healed event.
        E_Healed(new HealEventArgs(this, ref heal));

        this.health += heal;

        // If health is below zero, call the dead event.
        if (health <= 0)
        {
            E_Killed(new CharEventArgs(this));
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /// <summary>
    /// Move a player from one location to another on the map
    /// </summary>
    /// <param name="y">The new y position</param>
    /// <param name="x">The new x position</param>
    /// <returns>Returns false if you cannot move to that spot.</returns>
    public bool Move(int y, int x)
    {
        if (map.map[y, x].myCharacter != null || map.map[y, x].height != TerrainHeight.Empty || (movement == MovementType.Ground && map.map[y, x].height == TerrainHeight.Wall))
        {
            return false;
        }

        map.map[this.y, this.x].myCharacter = null;
        map.map[y, x].myCharacter = this;
        this.y = y;
        this.x = x;

        myLocation = map.map[y, x];

        // Call Move Event
        E_Moved(new CharEventArgs(this));

        return true;
    }

    /// <summary>
    /// Adds a condition to a character
    /// </summary>
    /// <param name="newCondition">The condition to be applied</param>
    /// <param name="turns">The number of turns the effect should stay active.</param>
    public void AddCondition(Condition newCondition, int turns)
    {
        E_GainCondition(new ConditionEventArgs(this, ref newCondition, ref turns));

        if (myConditions.ContainsKey(newCondition))
        {
            if(myConditions[newCondition] < turns){
                myConditions[newCondition] = turns;
            }
            return;
        }

        myConditions.Add(newCondition, turns);

        // Any effects that occur when a condition begins (perhaps instant heal);
        switch (newCondition)
        {

        }
    }

    /// <summary>
    /// Removes a condition from the list of current active conditions.
    /// </summary>
    /// <param name="newCondition">The condition to be removed</param>
    public void RemoveCondition(Condition newCondition)
    {
        if (myConditions.ContainsKey(newCondition))
        {
            // For simplicity sake I'm passing in a meaningless turns.  If there's a reason to use it later all the better
            int turns = 0;
            E_LoseCondition(new ConditionEventArgs(this, ref newCondition, ref turns));

            myConditions.Remove(newCondition);
            
            // Any effects that occur when a condition ends (say disabling speed or something) should happen here
            switch(newCondition){

            }
        }
    }

    /// <summary>
    /// Call on all characters at the beginning of their team's turn.
    /// </summary>
    public void BeginTurn()
    {
        // Called at the beginning of every turn.
        E_BeginTurn(new CharEventArgs(this));

        // Is the character dead?
        if(health <= 0){
            // Killed event
            E_Killed(new CharEventArgs(this));
        }

        // Go through all conditions and apply any effects that occur at the beginning of a round.
        foreach (Condition myCondition in myConditions.Keys)
        {
            switch (myCondition)// condition effects
            {
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Call on all characters at the end of their side's turn.
    /// </summary>
    public void EndTurn()
    {
        // Called at the beginning of every turn.
        E_EndTurn(new CharEventArgs(this));

        // Is the character dead?
        if (health <= 0)
        {
            // Killed event
            E_Killed(new CharEventArgs(this));
        }

        // Go through all conditions and apply any effects that occur at the end of a round.
        foreach (Condition myCondition in myConditions.Keys)
        {
            switch (myCondition)// condition effects
            {
                default:
                    break;
            }

            // All conditions count down and get removed at the end of a turn.
            myConditions[myCondition] -= 1;

            if (myConditions[myCondition] <= 0)
            {
                RemoveCondition(myCondition);
            }
        }
    }

}
