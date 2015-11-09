using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum PlayerState
{
    TurnStart,
    Actions,
    TurnEnd
}

public class GameManager : MonoBehaviour {

    public GridHandler map;

    private Character myChar;
    private Character myChar2;

    private int playerIndex = 0;
    private List<Faction> factions = new List<Faction>();
    public List<Character> Characters = new List<Character>();
    private int currentFactionTurn = 0;
    private PlayerState currentState = PlayerState.TurnStart;

    List<Node> possibleSpots = new List<Node>();
    List<Node> path = new List<Node>();

	// Use this for initialization
	void Start () {
        CharacterInfoList.Initialize();

	    map = new GridHandler(10, 10);
        //map.PrintTest();
        //map.PrintNode(4, 4);
        for (int i = 0; i < 8; i++)
        {
            map.map[2, i].height = TerrainHeight.Wall;
        }
        map.map[3, 4].height = TerrainHeight.Empty;
        map.map[3, 6].height = TerrainHeight.Empty;
        map.map[4, 4].height = TerrainHeight.Empty;
        map.map[4, 5].height = TerrainHeight.Empty;
        map.map[4, 6].height = TerrainHeight.Empty;
        myChar = new Character(map, "Example");
        myChar2 = new Character(map, "Example");

        if (myChar.Create(0, 0, 0, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }
        if (myChar2.Create(1, 8, 1, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }
        List<Character> heroes = new List<Character>();
        List<Character> villains = new List<Character>();

        heroes.Add(myChar);
        villains.Add(myChar2);

        factions.Add(new Faction(heroes));
        factions.Add(new Faction(villains));
        possibleSpots = myChar.MyLocation.FindPossibleMoves(0, 4, null, null);
        path = myChar.MyLocation.FindPathTo(map.map[5, 6], map, MovementType.Ground);

        Debug.Log(myChar.Name);
        Debug.Log(myChar.Health);

        gameObject.GetComponent<UIManager>().InitMap();
	}
	
	// Update is called once per frame
	void Update () {
        // Debug map stuff leave alone for now.
	    for(int i = 0; i < 10; ++i){
                for(int j = 0; j < 10; ++j){
                    map.map[j, i].Draw();
            }
        }
        
        for (int i = 0; i < possibleSpots.Count; ++i)
        {
            Debug.DrawLine(new Vector3(possibleSpots[i].X * 5, possibleSpots[i].Y * -5), new Vector3((possibleSpots[i].X + 1) * 5, (possibleSpots[i].Y + 1) * -5), Color.red);
            Debug.DrawLine(new Vector3((possibleSpots[i].X + 1) * 5, possibleSpots[i].Y * -5), new Vector3(possibleSpots[i].X * 5, (possibleSpots[i].Y + 1) * -5), Color.red);
        }

        if (path != null)
        {
            for (int i = 0; i < path.Count; ++i)
            {
                Debug.DrawLine(new Vector3(path[i].X * 5, path[i].Y * -5), new Vector3((path[i].X + 1) * 5, (path[i].Y + 1) * -5), Color.yellow);
                Debug.DrawLine(new Vector3((path[i].X + 1) * 5, path[i].Y * -5), new Vector3(path[i].X * 5, (path[i].Y + 1) * -5), Color.yellow);
            }
        }

        switch (currentState)
        {
            case PlayerState.Actions:
                TurnAction();
                break;
            case PlayerState.TurnStart:
                TurnStart();
                break;
            case PlayerState.TurnEnd:
                TurnEnd();
                break;
        }

	}

    private void TurnStart()
    {
        for (int i = 0; i < factions[currentFactionTurn].Units.Count; ++i)
        {
            factions[currentFactionTurn].Units[i].BeginTurn();
        }
        currentState = PlayerState.Actions;
        TurnAction(); // We actually call turn action here because there's no reason to waste an update.
    }

    private void TurnAction()
    {
        if (playerIndex == currentFactionTurn) // If it's the player's turn
        {

        }
        else // AI stuff
        {


        }

    }

    private void TurnEnd()
    {
        for (int i = 0; i < factions[currentFactionTurn].Units.Count; ++i)
        {
            factions[currentFactionTurn].Units[i].BeginTurn();
        }
        currentState = PlayerState.TurnStart;
    }
}
