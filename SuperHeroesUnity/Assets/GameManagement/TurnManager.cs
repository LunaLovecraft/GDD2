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
    static UIManager uiMan;

    public static void Initialize(List<Faction> inputFactions)
    {
        factions = inputFactions;
        uiMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
    }

    public static void Update()
    {
        Debug.Log(currentState);
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
        Debug.Log("Selecting a character");
        uiMan.SelectCharacter(factions[currentFactionTurn].Units[0]);

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
        currentFactionTurn++;
        if (currentFactionTurn >= factions.Count)
        {
            currentFactionTurn = 0;
        }
        currentState = TurnState.TurnStart;
    }
}
