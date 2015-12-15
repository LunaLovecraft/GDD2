using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GridHandler map;

    private Character PaulBunyan;
    private Character Wink;
	private Character Technomancer;
	private Character GreyDeath;
	private Character Swarm;
	private Character Nox;

    private List<Faction> factions = new List<Faction>();
    public List<Faction> Factions { get { return factions; } }
	public GameObject toastTextPrefab;

    public List<Character> Characters = new List<Character>();

    List<Node> possibleSpots = new List<Node>();
    List<Node> path = new List<Node>();

	// Use this for initialization
	void Start () {
		MessageLog.Initialize ();
		Toast.Initialize (toastTextPrefab);
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

        PaulBunyan = new Character(map, "PaulBunyan");
		Wink = new Character (map, "Wink");
		Technomancer = new Character (map, "Technomancer");

		Nox = new Character (map, "Nox");
		GreyDeath = new Character(map, "GreyDeath");
		Swarm = new Character(map, "Swarm");

        if (PaulBunyan.Create(3, 0, 0, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }
        //if (PaulBunyan2.Create(4, 0, 0, Characters) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        //{

       // }
		Wink.Create (4, 0, 0, Characters);
		Technomancer.Create (5, 0, 0, Characters);

		GreyDeath.Create (3, 8, 1, Characters);
		Nox.Create (4, 8, 1, Characters);
		Swarm.Create (5, 8, 1, Characters);
        List<Character> heroes = new List<Character>();
        List<Character> villains = new List<Character>();

        heroes.Add(PaulBunyan);
		heroes.Add (Wink);
		heroes.Add (Technomancer);

        villains.Add(GreyDeath);
		villains.Add(Nox);
		villains.Add (Swarm);


        factions.Add(new Faction(heroes));
        factions.Add(new Faction(villains));
        possibleSpots = PaulBunyan.MyLocation.FindPossibleMoves(0, 4, null, null);
        path = PaulBunyan.MyLocation.FindPathTo(map.map[5, 6], map, MovementType.Ground);

        Debug.Log(PaulBunyan.Name);
        Debug.Log(PaulBunyan.Health);
        
        
        gameObject.GetComponent<UIManager>().InitMap();

        TurnManager.Initialize(factions);

        foreach(Character character in Characters)
        {
            character.OnDamaged += new EventHandler(gameObject.GetComponent<UIManager>().ShowDamage);
            character.OnHealed += new EventHandler(gameObject.GetComponent<UIManager>().ShowHeal);

            character.OnKilled += new EventHandler(RemoveCharacter);
        }

        Debug.Log("Map in start: " + map);
		Toast.SendToast ("Game Start!");
	}
	
	// Update is called once per frame
	void Update () {
        TurnManager.Update();
	}
    
    /// <summary>
    /// Removes the character
    /// </summary>
    void RemoveCharacter(object sender, EventArgs e)
    {
        CharEventArgs data = (CharEventArgs)e;
        Character myChar = data.myChar;

        factions[myChar.Faction].Units.Remove(myChar);

        Characters.Remove(myChar);

        CharacterSpriteScript[] characterSprites=  FindObjectsOfType<CharacterSpriteScript>();

        foreach(CharacterSpriteScript characterSprite in characterSprites)
        {
            if (characterSprite.X == myChar.MyLocation.X && characterSprite.Y == myChar.MyLocation.Y)
            {
                Debug.Log(characterSprite);

                characterSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }
}
