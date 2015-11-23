using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TurnState
{
    TurnStart,
    Actions,
    TurnEnd
}

public static class TurnManager
{
    public static TurnState currentState = TurnState.TurnStart;
    public static List<Faction> factions;
    public static int currentFactionTurn = 0;
    private static int playerIndex = 0;

    public static void Initialize(List<Faction> inputFactions)
    {
        factions = inputFactions;
    }

    public static void Update()
    {

        switch (currentState)
        {
            case TurnState.Actions:
                TurnAction();
                break;
            case TurnState.TurnStart:
                TurnStart();
                break;
            case TurnState.TurnEnd:
                TurnEnd();
                break;
        }

    }

    private static void TurnStart()
    {
        for (int i = 0; i < factions[currentFactionTurn].Units.Count; ++i)
        {
            factions[currentFactionTurn].Units[i].BeginTurn();
        }
        currentState = TurnState.Actions;
        TurnAction(); // We actually call turn action here because there's no reason to waste an update.
    }

    private static void TurnAction()
    {
        if (playerIndex == currentFactionTurn) // If it's the player's turn
        {

        }
        else // AI stuff
        {


        }

    }

    private static void TurnEnd()
    {
        for (int i = 0; i < factions[currentFactionTurn].Units.Count; ++i)
        {
            factions[currentFactionTurn].Units[i].EndTurn();
        }
        currentState = TurnState.TurnStart;
    }
}
