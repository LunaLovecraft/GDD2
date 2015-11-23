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

public static class AIBehavior
{
    static GameManager gameManager = GameObject.FindObjectOfType<GameManager>();

    public static void DoTurn(Character character)
    {
        if (character.canMove)
        {
            // Creates a list of movement actions with scores.
            List<MovementAction> movementActions = FindMovementActions(character);

            // Chooses a movement action from that list
            MovementAction chosenMovement = ChooseMovementAction(movementActions);

            // Do Move
            character.Move(chosenMovement.node.X, chosenMovement.node.Y);
        }

        if (character.canAct)
        {
            // Creates a list of ability actions based off the chosen movement action
            List<AbilityAction> abilityActions = FindAbilityActions(character);

            // Chooses an ability action from that list.
            AbilityAction chosenAbility = ChooseAbilityAction(abilityActions);

            // Do Abillity
            chosenAbility.ability(character, gameManager.map);
            UIInformation uIInformation = UIInformationHandler.InformationStack.Peek();
            int i = Random.Range(0, uIInformation.options.Count);
            uIInformation.SelectOption(i);
        }
    }

    #region Choose Action Helper Methods
    static List<MovementAction> FindMovementActions(Character character)
    {
        // Find locations to move to.
        List<Node> moveLocations = character.MyLocation.FindPossibleMoves((int)character.Movement, character.Speed);
        List<MovementAction> movementActions = new List<MovementAction>();

        // Finds allies and enemies.
        List<Node> allyLocations = new List<Node>();
        List<Node> enemylocations = new List<Node>();

        // Percentage of current health to max health used to determine how healthy a character is.
        float healthiness = character.Health / character.MaxHealth;

        for (int i = 0; i < gameManager.Factions.Count; i++)
        {
            if (i == character.Faction)
            {
                foreach (Character ally in gameManager.Factions[i].Units)
                {
                    allyLocations.Add(ally.MyLocation);
                }
            }
            else
            {
                foreach (Character enemy in gameManager.Factions[i].Units)
                {
                    enemylocations.Add(enemy.MyLocation);
                }
            }
        }


        // Goes through each node and gives a score based on specific factors about the location
        foreach (Node node in moveLocations)
        {
            int score = 0;


            int rangeToEnemey = FindRangeToClosest(character.MyLocation, enemylocations);
            int rangeToFriend = FindRangeToClosest(character.MyLocation, enemylocations);
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

        return movementActions;
    }

    static MovementAction ChooseMovementAction(List<MovementAction> movementActions)
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

    static List<AbilityAction> FindAbilityActions(Character character)
    {
        List<AbilityAction> abilityActions = new List<AbilityAction>();

        foreach (Ability ability in character.Abilities)
        {
            int score = 0;

            abilityActions.Add(new AbilityAction(ability, score));
        }

        return abilityActions;
    }

    static AbilityAction ChooseAbilityAction(List<AbilityAction> abilityActions)
    {
        int index = Random.Range(0, abilityActions.Count);

        return abilityActions[index];
    }

    static int FindRangeToClosest(Node startLocation, List<Node> locations)
    {
        int maxRange = int.MaxValue;

        foreach (Node location in locations)
        {
            int range = startLocation.FindPathTo(location, gameManager.map, MovementType.Ground).Count;

            if (range < maxRange)
            {
                maxRange = range;
            }
        }

        return maxRange;
    }
    #endregion
}
