using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GridHandler map;

    private Character myChar;
    private Character myChar2;

    List<Node> possibleSpots = new List<Node>();
    List<Node> path = new List<Node>();

	// Use this for initialization
	void Start () {
        CharacterList.Initialize();

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

        if (myChar.Create(0, 0) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }
        if (myChar2.Create(1, 8) == CHAR_INIT.CHAR_AT_LOC_ERROR)
        {

        }

        possibleSpots = myChar.MyLocation.FindPossibleMoves(0, 4, null, null);
        path = myChar.MyLocation.FindPathTo(map.map[5, 6], map, MovementType.Ground);

        

        if (path != null)
        {
            Debug.Log(path.Count);
            for (int i = 0; i < path.Count; ++i)
            {
                Debug.Log(path[i].ToString());
            }
        }
        else
        {
            Debug.Log("No Path Possible");
        }
        Debug.Log(possibleSpots.Count);
       
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
