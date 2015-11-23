using UnityEngine;
using System.Collections;

enum TurnState
{
    TurnStart,
    Actions,
    TurnEnd
}

public class TurnManager : MonoBehaviour
{
    TurnOrderManager turnOrderManager;
    Character character;
    TurnState turnState;

    UIManager uIManager;

    void Start()
    {
        turnOrderManager = GetComponent<TurnOrderManager>();
        turnState = TurnState.TurnStart;
        uIManager = GetComponent<UIManager>();
    }

    public void StartNewTurn(Character character)
    {
        this.character = character;
        uIManager.SetCharacter(character);
    }

    private void TurnStart()
    {
    }

    private void TurnAction()
    {
    }

    public void TurnEnd()
    {
        turnOrderManager.EndCurrentTurn();
    }
}
