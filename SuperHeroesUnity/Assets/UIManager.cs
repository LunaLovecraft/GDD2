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
    List<GameObject> Characters;
    List<Node> Moves;
	// Use this for initialization
	void Start () {

	}

    public void InitMap()
    {

        currentState = UIState.Default;
        UIUpdate();
        Characters = new List<GameObject>();
        gm = gameObject.GetComponent<GameManager>();
        Debug.Log(gm);
        
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
                    character.transform.position = new Vector3(x - 5, y - 4.5f, -0.5f);
                    if (map.map[y, x].myCharacter.Faction == 0)
                    {
                        character.GetComponent<SpriteRenderer>().color = Color.green;
                    } else character.GetComponent<SpriteRenderer>().color = Color.red;

         
                }
            }
        }

        Debug.Log("Init" + map);
        Debug.Log(gm.map);

    }

    public void TileClicked(int x, int y)
    {
        Debug.Log("The Tile at " + x + ", " + y + " is " + map.map[y, x].height);
    }

    public void HandleClick(string buttonState)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>().ButtonClicked(buttonState);

    }

   public void ButtonClicked(string buttonState)
    {
        History.Push(currentState);
        //clearUI();        
        switch (buttonState)
        {
            case "Movement":
                currentState = UIState.Movement;
                break;
            case "Attack":
                currentState = UIState.Attacking;
                break;
            case "Back":
                currentState = (UIState) History.Pop();
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
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("AttackUI"));
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("BackUI"));
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("MoveUI"));
        foreach(GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        GameObject[] defaultUI = GameObject.FindGameObjectsWithTag("HomeUI");
        foreach(GameObject obj in defaultUI)
        {
            obj.SetActive(true);
        }
    }

    void drawMovementUI()
    {
        Debug.Log("Moving? " + currentState);
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("AttackUI"));
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("HomeUI"));
        foreach (GameObject obj in unwanted)
        {
            Debug.Log("deactivating");
            obj.SetActive(false);
        }
        List<GameObject> moveUI = new List<GameObject>();
        moveUI.AddRange(GameObject.FindGameObjectsWithTag("MoveUI"));
        moveUI.AddRange(GameObject.FindGameObjectsWithTag("BackUI"));
        Debug.Log(GameObject.FindGameObjectsWithTag("BackUI").Length);
        Debug.Log(moveUI.Count);
        foreach (GameObject obj in moveUI)
        {
            Debug.Log("activating");
            obj.SetActive(true);
        }
        Moves = map.map[0, 0].FindPossibleMoves(0, 4, null, null);

        foreach (Node node in Moves)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Color.green;
        }
        
    }

    void drawAttackingUI()
    {
        Debug.Log("Attacking? " + currentState);
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("MoveUI"));
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("HomeUI"));
        foreach (GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        List<GameObject> attackUI = new List<GameObject>();
        attackUI.AddRange(GameObject.FindGameObjectsWithTag("AttackUI"));
        attackUI.AddRange(GameObject.FindGameObjectsWithTag("BackUI"));
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
