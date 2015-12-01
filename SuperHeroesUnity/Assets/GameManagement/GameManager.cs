using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GridHandler map;

    private Character myChar;
    private Character myChar2;
	private Character myChar3;
	private Character myChar4;

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

        myChar = new Character(map, "PaulBunyan");
		//myChar2 = new Character (map, "Wink");
		//myChar3 = new Character (map, "Nox");
        myChar4 = new Character(map, "GreyDeath");

        if (myChar.Create(3, 0, 0, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }
        //if (myChar2.Create(4, 0, 0, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        //{

       // }
		//myChar3.Create (3, 8, 1, Characters);
		myChar4.Create (4, 8, 1, Characters);
        List<Character> heroes = new List<Character>();
        List<Character> villains = new List<Character>();

        heroes.Add(myChar);
		//heroes.Add (myChar2);
        //villains.Add(myChar3);
		villains.Add(myChar4);

        factions.Add(new Faction(heroes));
        factions.Add(new Faction(villains));
        possibleSpots = myChar.MyLocation.FindPossibleMoves(0, 4, null, null);
        path = myChar.MyLocation.FindPathTo(map.map[5, 6], map, MovementType.Ground);

        Debug.Log(myChar.Name);
        Debug.Log(myChar.Health);
        
        
        gameObject.GetComponent<UIManager>().InitMap();

        TurnManager.Initialize(factions);

        foreach(Character character in Characters)
        {
            character.OnDamaged += new EventHandler(gameObject.GetComponent<UIManager>().ShowDamage);
            character.OnHealed += new EventHandler(gameObject.GetComponent<UIManager>().ShowHeal);
        }

        Debug.Log("Map in start: " + map);
	}
	
	// Update is called once per frame
	void Update () {
        TurnManager.Update();
	}
}
