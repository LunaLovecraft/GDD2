using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct MovementAction
{
    public MovementAction(Node node, Ability ability, int score)
    {
        this.node = node;
        this.ability = ability;

        this.score = score;
    }
    public Node node;

    public int score;
}

struct AbilityAction
{

}

public class AIBehavior : MonoBehaviour
{
    GameManager gameManager;
    Character character;

    List<Node> moveLocations;
    List<Ability> abilities;

    List<Character> allies;
    List<Character> enemies;

    List<MovementAction> movementActions = new List<MovementAction>();
    MovementAction chosenMovement;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        character = GetComponent<Character>();
    }

    void MakeAction()
    {
        // Find locations to move to.
        moveLocations = character.MyLocation.FindPossibleMoves((int)character.Movement, character.Speed);

        // Find possible abilities to use.
        abilities = character.Abilities;

        ScoreMovementActions();

        chosenMovement = ChooseMovementAction();

        ScoreAbilityActions();
    }

    void ScoreMovementActions()
    {
        // Percentage of current health to max health used to determine how healthy a character is.
        float healthiness = character.Health / character.MaxHealth;

        // Finds allies and enemies.
        for(int i = 0; i < gameManager.Factions.Count; i++)
        {
            if(i == character.Faction)
            {
                allies = gameManager.Factions[i].Units;
            }
            else
            {
                enemies.AddRange(gameManager.Factions[i].Units);
            }
        }


        // Goes through each node and gives a score based on specific factors about the location
        foreach (Node node in moveLocations)
        {
            int score = 0;
            int rangeToEnemey = FindRangeToClosestEnemy();
            int rangeToFriend = FindRangeToClosestAlly();

            if (healthiness > 0.5f) // When healthy
            {
                score += rangeToEnemey;
                
            }
            else // When unhealthy
            {
                score -= rangeToEnemey;
                score += rangeToFriend;
            }
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
        }
    }

    int FindRangeToClosestEnemy()
    {
        int maxRange = int.MaxValue;

        foreach(Character enemy in enemies)
        {
            int rangeX =  Mathf.Abs( enemy.MyLocation.X - character.MyLocation.X);
            int rangeY = Mathf.Abs(enemy.MyLocation.Y - character.MyLocation.Y);
            int range = Mathf.Max(rangeX, rangeY);

            if(range < maxRange)
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
            int rangeX = Mathf.Abs(ally.MyLocation.X - character.MyLocation.X);
            int rangeY = Mathf.Abs(ally.MyLocation.Y - character.MyLocation.Y);
            int range = Mathf.Max(rangeX, rangeY);

            if (range < maxRange)
            {
                maxRange = range;
            }
        }

        return maxRange;
    }
}
