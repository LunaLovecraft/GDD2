using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GridHandler map;

    private Character myChar;
    private Character myChar2;

    private List<Faction> factions = new List<Faction>();
    public List<Faction> Factions { get { return factions; } }

    public List<Character> Characters = new List<Character>();

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

        TurnManager.Initialize(factions);
        Debug.Log("Map in start: " + map);
	}
	
	// Update is called once per frame
	void Update () {
        TurnManager.Update();
	}
}
