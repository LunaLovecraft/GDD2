using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct MovementAction
{
    public MovementAction(Node node, int score)
    {
        this.node = node;

        this.score = score;
    }
    public Node node;

    public int score;
}

public struct AbilityAction
{
    public AbilityAction(Ability ability, int score)
    {
        this.ability = ability;

        this.score = score;
    }

    public Ability ability;


    public int score;
}

public class AIBehavior : MonoBehaviour
{
    GameManager gameManager;
    Character character;

    List<Node> moveLocations;
    List<Ability> abilities;

    List<Character> allies = new List<Character>();
    List<Character> enemies = new List<Character>();

    List<MovementAction> movementActions = new List<MovementAction>();
    MovementAction chosenMovement;

    List<AbilityAction> abilityActions = new List<AbilityAction>();
    AbilityAction chosenAbility;

    float healthiness;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        character = GetComponent<Character>();
    }

    public void ChooseActions()
    {
        // Clears previous lists.
        abilityActions.Clear();
        movementActions.Clear();
        allies.Clear();
        enemies.Clear();

        // Finds allies and enemies.
        for (int i = 0; i < gameManager.Factions.Count; i++)
        {
            if (i == character.Faction)
            {
                allies = gameManager.Factions[i].Units;
            }
            else
            {
                enemies.AddRange(gameManager.Factions[i].Units);
            }
        }

        // Percentage of current health to max health used to determine how healthy a character is.
        healthiness = character.Health / character.MaxHealth;

        // Find locations to move to.
        moveLocations = character.MyLocation.FindPossibleMoves((int)character.Movement, character.Speed);
        // Find possible abilities to use.
        abilities = character.Abilities;

        // Creates a list of movement actions with scores.
        ScoreMovementActions();

        // Chooses a movement action from that list
        chosenMovement = ChooseMovementAction();

        // Creates a list of ability actions based off the chosen movement action
        ScoreAbilityActions();

        // Chooses an ability action from that list.
        chosenAbility = ChooseAbilityAction();
    }

    #region Choose Action Helper Methods
    void ScoreMovementActions()
    {
        // Goes through each node and gives a score based on specific factors about the location
        foreach (Node node in moveLocations)
        {
            int score = 0;
            int rangeToEnemey = FindRangeToClosestEnemy();
            int rangeToFriend = FindRangeToClosestAlly();
           // int enemiesInRange = FindEnemiesInRangeOfAttacks();

            if (healthiness > 0.5f) // When healthy
            {
                score -= rangeToEnemey;
            }
            else // When unhealthy
            {
                score += rangeToEnemey;
                score += rangeToFriend;
            }

            movementActions.Add(new MovementAction(node, score));
        }
    }

    MovementAction ChooseMovementAction()
    {
        // Sort actions from highest scored to lowest scored.
        for (int i = 0; i < movementActions.Count; i++)
        {
            int maxIndex = i;
            for(int j = i + 1; j < movementActions.Count; j++)
            {
                if(movementActions[maxIndex].score < movementActions[j].score)
                {
                    maxIndex = j;
                }
            }

            MovementAction temp = movementActions[i];
            movementActions[i] = movementActions[maxIndex];
            movementActions[maxIndex] = temp;
        }

        // Picks move positions from highest
        return movementActions[0];
    }

    void ScoreAbilityActions()
    {
        foreach (Ability ability in abilities)
        {
            int score = 0;

            /*
             * If the ability is an Attack
             *   + Add damage inflicted
             *   + Negative status effects given
             *   - Friendly Fire
             * 
             * If the ability is a buff
             *    + Add health given
             *    + Positive status effects given
             */

            abilityActions.Add(new AbilityAction(ability, score));
        }
    }

    AbilityAction ChooseAbilityAction()
    {
        // Sort actions from highest scored to lowest scored.
        for (int i = 0; i < abilityActions.Count; i++)
        {
            int maxIndex = i;
            for (int j = i + 1; j < abilityActions.Count; j++)
            {
                if (abilityActions[maxIndex].score < abilityActions[j].score)
                {
                    maxIndex = j;
                }
            }

            AbilityAction temp = abilityActions[i];
            abilityActions[i] = abilityActions[maxIndex];
            abilityActions[maxIndex] = temp;
        }

        // Picks move positions from highest
        return abilityActions[0];
    }

    int FindRangeToClosestEnemy()
    {
        int maxRange = int.MaxValue;

        foreach (Character enemy in enemies)
        {
            int range = enemy.MyLocation.FindPathTo(character.MyLocation, gameManager.map, enemy.Movement).Count;

            if (range < maxRange)
            {
                maxRange = range;
            }
        }

        return maxRange;

    }

    int FindRangeToClosestAlly()
    {
        int maxRange = int.MaxValue;

        foreach (Character ally in allies)
        {
            int range = ally.MyLocation.FindPathTo(character.MyLocation, gameManager.map, ally.Movement).Count;

            if (range < maxRange)
            {
                maxRange = range;
            }
        }

        return maxRange;
    }

    /* int FindEnemiesInRangeOfAttacks()
     {
         int score = 0;

         foreach(Ability ability in abilities)
         {
             if ((bool)Abilities.GetAbilityInfo(ability.Method.Name, AbilityInfo.Offensive))
             {
                 int abilityRange = (int)Abilities.GetAbilityInfo(ability.Method.Name, AbilityInfo.Range);

                 foreach(Character enemy in enemies)
                 {
                     if(enemy.MyLocation.FindPathTo(character.MyLocation, gameManager.map).Count  < abilityRange)
                     {
                         score += 1;
                     }
                 }
             }
         }

         return score;
     }*/
    #endregion

    MovementAction GetChosenMovementAction()
    {
        return chosenMovement;
    }

    AbilityAction GetChosenAbilityAction()
    {
        return chosenAbility;
    }
}
