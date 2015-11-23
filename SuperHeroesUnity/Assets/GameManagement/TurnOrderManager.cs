using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderManager : MonoBehaviour
{
    public int currentTurn = 0;
    public List<Character> turnOrder  = new List<Character>();
    public TurnManager turnManager;

    void Start()
    {
        turnManager = gameObject.AddComponent<TurnManager>();
    }

    public void StartTurnOrder()
    {
        turnManager.StartNewTurn(turnOrder[currentTurn]);
    }


    /// <summary>
    /// Add character to turn order.
    /// </summary>
    /// <param name="character">The character to be added</param>
    public void AddToTurnOrder(Character character)
    {
        Debug.Log(character + ": " + character.Name);
        turnOrder.Add(character);
    }

    public void RemoveFromTurnOrder(Character character)
    {
        int index = FindOnTurnOrder(character);
        turnOrder.RemoveAt(index);

        if (index < currentTurn)
            currentTurn--;
    }

    public int FindOnTurnOrder(Character character)
    {
        int index = turnOrder.BinarySearch(character);

        if (index >= 0)
            return index;
        else
            return -1;
    }

    public Character GetCurrentTurnCharacter()
    {
        return turnOrder[currentTurn];
    }

    public int GetCurrentTurnNumber()
    {
        return currentTurn;
    }

    public void EndCurrentTurn()
    {
        currentTurn = (currentTurn + 1) % turnOrder.Count;

        turnManager.StartNewTurn(turnOrder[currentTurn]);
    }
}
