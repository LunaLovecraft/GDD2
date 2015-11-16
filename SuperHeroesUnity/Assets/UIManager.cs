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
    GridHandler map;
    List<GameObject> CharacterUI;
    List<Node> Moves;
    List<GameObject> BackUI;
    List<GameObject> AttackUI;
    List<GameObject> HomeUI;
    List<GameObject> MoveUI;
    Character selectedCharacter;
	// Use this for initialization
	void Start () {
	}

    public void InitMap()
    {
        BackUI = new List<GameObject>();
        AttackUI = new List<GameObject>();
        HomeUI = new List<GameObject>();
        MoveUI = new List<GameObject>();

        Moves = new List<Node>();

        BackUI.AddRange(GameObject.FindGameObjectsWithTag("BackUI"));
        AttackUI.AddRange(GameObject.FindGameObjectsWithTag("AttackUI"));
        HomeUI.AddRange(GameObject.FindGameObjectsWithTag("HomeUI"));
        MoveUI.AddRange(GameObject.FindGameObjectsWithTag("MoveUI"));

        
        CharacterUI = new List<GameObject>();
        gm = gameObject.GetComponent<GameManager>();
        
        map = gm.map;
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
                    character.GetComponent<CharacterSpriteScript>().moveTo(x, y);
                    if (map.map[y, x].myCharacter.Faction == 0)
                    {
                        selectedCharacter = map.map[y, x].myCharacter;
                        character.GetComponent<CharacterSpriteScript>().X = x;
                        character.GetComponent<CharacterSpriteScript>().Y = y;
                        character.GetComponent<SpriteRenderer>().color = Color.green;
                    } else character.GetComponent<SpriteRenderer>().color = Color.red;
                    CharacterUI.Add(character);
         
                }
            }
        }

        currentState = UIState.Default;
        UIUpdate();
    }

    public void TileClicked(int x, int y)
    {
        Debug.Log("The Tile at " + x + ", " + y + " is " + map.map[y, x].height);
        foreach (Node node in Moves)
        {
            if (node.X == x && node.Y == y)
            {
                Debug.Log("Match found at " + x + ", " + y );
                foreach(GameObject tile in CharacterUI)
                {
                    if(tile.GetComponent<CharacterSpriteScript>().X == selectedCharacter.X && tile.GetComponent<CharacterSpriteScript>().Y == selectedCharacter.Y)
                    {
                        Debug.Log("Matching Character found");
                        tile.GetComponent<CharacterSpriteScript>().moveTo(x, y);
                        break;
                    }
                }
                selectedCharacter.Move(y, x);
                Moves = map.map[selectedCharacter.Y, selectedCharacter.X].FindPossibleMoves(0, 4, null, null);
                break;
            }
        }
        
    }

    public void HandleClick(string buttonState)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>().ButtonClicked(buttonState);

    }

   public void ButtonClicked(string buttonState)
    {
        
        //clearUI();        
        switch (buttonState)
        {
            case "Movement":
                History.Push(currentState);
                currentState = UIState.Movement;
                break;
            case "Attack":
                History.Push(currentState);
                currentState = UIState.Attacking;
                break;
            case "Back":
                currentState = (UIState) History.Pop();
                Debug.Log(currentState);
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

        //Destroy(CurrentCanvas.gameObject);
    }

    void drawDefaultUI()
    {
        Debug.Log("Default? " + currentState);
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(AttackUI);
        unwanted.AddRange(BackUI);
        unwanted.AddRange(MoveUI);
        foreach(GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in HomeUI)
        {
            obj.SetActive(true);
        }
        foreach (Node node in Moves)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Tiles[node.Y, node.X].GetComponent<TileScript>().baseColor;
        }
    }

    void drawMovementUI()
    {
        Debug.Log("Moving? " + currentState);
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(AttackUI);
        unwanted.AddRange(HomeUI);
        foreach (GameObject obj in unwanted)
        {
            Debug.Log("deactivating");
            obj.SetActive(false);
        }
        List<GameObject> moveUI = new List<GameObject>();
        moveUI.AddRange(MoveUI);
        moveUI.AddRange(BackUI);
        foreach (GameObject obj in moveUI)
        {
            Debug.Log("activating");
            obj.SetActive(true);
        }
        Moves = map.map[selectedCharacter.Y, selectedCharacter.X].FindPossibleMoves(0, 4, null, null);

        foreach (Node node in Moves)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Color.green;
        }
        
    }

    void drawAttackingUI()
    {
        Debug.Log("Attacking? " + currentState);
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(MoveUI);
        unwanted.AddRange(HomeUI);
        foreach (GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        List<GameObject> attackUI = new List<GameObject>();
        attackUI.AddRange(AttackUI);
        attackUI.AddRange(BackUI);
        foreach (GameObject obj in attackUI)
        {
            obj.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.Space) == true)
        {
            clearUI();
        }
	}
}
