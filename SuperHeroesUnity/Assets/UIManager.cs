using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum UIState
{
    Default,
    Movement,
    Attacking
}

public class UIManager : MonoBehaviour {
    
    GameManager gm;
    UIState currentState;
    GameObject CurrentCanvas;
    Stack History = new Stack();
    GameObject[,] Tiles;
    List<GameObject> Characters;
    public List<Node> Moves;
	// Use this for initialization
	void Start () {
        currentState = UIState.Default;
        UIUpdate();
        Characters = new List<GameObject>();
        gm = gameObject.GetComponent<GameManager>();
        
	}

    public void InitMap()
    {
        GridHandler map = gm.map;
        Tiles = new GameObject[map.Height,map.Width];


        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                GameObject tempTile = Instantiate(Resources.Load("tile")) as GameObject;
                tempTile.transform.position = new Vector3(x - 5, y - 4.5f, 0);
                tempTile.GetComponent<TileScript>().x = x;
                tempTile.GetComponent<TileScript>().y = y;
                TerrainHeight tileHeight = map.map[y, x].height;
                switch (tileHeight)
                {
                    case TerrainHeight.Empty:
                        tempTile.GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                    case TerrainHeight.Ground:
                        tempTile.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                    case TerrainHeight.Wall:
                        tempTile.GetComponent<SpriteRenderer>().color = Color.gray;
                        break;
                }

                Tiles[y, x] = tempTile;

                if (map.map[y, x].myCharacter != null)
                {
                    GameObject character = Instantiate(Resources.Load("Character")) as GameObject;
                    character.transform.position = new Vector3(x - 5, y - 4.5f, -0.5f);
                    if (map.map[y, x].myCharacter.Faction == 0)
                    {
                        character.GetComponent<SpriteRenderer>().color = Color.green;
                    } else character.GetComponent<SpriteRenderer>().color = Color.red;

         
                }
            }
        }
    }

    public void TileClicked(int x, int y)
    {
        GridHandler map = gm.map;
        Debug.Log("The Tile at " + x + ", " + y + " is " + map.map[y, x].height);
    }

   public void ButtonClicked(string buttonState)
    {
        History.Push(currentState);
        clearUI();        
        switch (buttonState)
        {
            case "Movement":
                currentState = UIState.Movement;
                break;
            case "Attack":
                currentState = UIState.Attacking;
                break;
            default:
                currentState = UIState.Default;
                break;
        }
        UIUpdate();
    }

    void UIUpdate()
    {
        switch (currentState)
        {
            case UIState.Movement:
                drawMovementUI();
                break;
            case UIState.Attacking:
                drawAttackingUI();
                break;
            default:
                drawDefaultUI();
                break;
        }
    }
	
    void clearUI()
    {
        Debug.Log("Clearing UI");

        Destroy(CurrentCanvas.gameObject);
    }

    void drawDefaultUI()
    {
        Debug.Log("Default? " + currentState);
        CurrentCanvas = Instantiate(Resources.Load("UI/DefaultCanvas")) as GameObject;
    }

    void drawMovementUI()
    {
        Debug.Log("Moving? " + currentState);
        CurrentCanvas = Instantiate(Resources.Load("UI/MovementCanvas")) as GameObject;


        gm = gameObject.GetComponent<GameManager>();
        
        Moves = gm.map.map[0, 0].FindPossibleMoves(0, 4, null, null);

        foreach (Node node in Moves)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Color.green;
        }
        
    }

    void drawAttackingUI()
    {
        Debug.Log("Attacking? " + currentState);
        CurrentCanvas = Instantiate(Resources.Load("UI/AttackingCanvas")) as GameObject;
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.Space) == true)
        {
            clearUI();
        }
	}
}
