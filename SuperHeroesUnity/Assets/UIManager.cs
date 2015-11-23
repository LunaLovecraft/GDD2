using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

enum UIState
{
    Default,
    Movement,
    Attacking,
    Planning
}

public class UIManager : MonoBehaviour {
    
    GameManager gm;
    UIState currentState;
    public GameObject CurrentCanvas;
    Stack History = new Stack();
    GameObject[,] Tiles;
    GridHandler map;
    List<GameObject> CharacterUI;
    List<Node> Moves;
    int SelectedAbilityOptions = -1;
    List<GameObject> BackUI;
    List<GameObject> AttackUI;
    List<GameObject> HomeUI;
    List<GameObject> MoveUI;
    List<GameObject> PlanningUI;
    public Button abilityButton;
    Character selectedCharacter;
    public GameObject HoverTile;
    public Text CharName;
    public Text CharHealth;
    public Text CharMove;
    public Text AbilityName;
    public Text AbilityDescription;
    Ability selectedAbility;


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
        PlanningUI = new List<GameObject>();

        BackUI.AddRange(GameObject.FindGameObjectsWithTag("BackUI"));
        AttackUI.AddRange(GameObject.FindGameObjectsWithTag("AttackUI"));
        HomeUI.AddRange(GameObject.FindGameObjectsWithTag("HomeUI"));
        MoveUI.AddRange(GameObject.FindGameObjectsWithTag("MoveUI"));
        PlanningUI.AddRange(GameObject.FindGameObjectsWithTag("PlanningUI"));
        
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
                        tempTile.GetComponent<TileScript>().baseColor = Color.black;
                        break;
                    case TerrainHeight.Ground:
                        tempTile.GetComponent<SpriteRenderer>().color = Color.white;
                        tempTile.GetComponent<TileScript>().baseColor = Color.white;
                        break;
                    case TerrainHeight.Wall:
                        tempTile.GetComponent<SpriteRenderer>().color = Color.gray;
                        tempTile.GetComponent<TileScript>().baseColor = Color.gray;
                        break;
                }

                Tiles[y, x] = tempTile;

                if (map.map[y, x].myCharacter != null)
                {
                    GameObject character = Instantiate(Resources.Load("Character")) as GameObject;
                    character.GetComponent<CharacterSpriteScript>().moveTo(x, y);
                    if (map.map[y, x].myCharacter.Faction == 0)
                    {
                        character.GetComponent<CharacterSpriteScript>().X = x;
                        character.GetComponent<CharacterSpriteScript>().Y = y;
                        character.GetComponent<SpriteRenderer>().color = Color.green;
                    } else character.GetComponent<SpriteRenderer>().color = Color.red;
                    SelectCharacter(map.map[y, x].myCharacter);
                    CharacterUI.Add(character);
         
                }
            }
        }

        currentState = UIState.Default;
        UIUpdate();
    }

    public void SelectCharacter(Character newSelection)
    {
        selectedCharacter = newSelection;
        CharName.text = selectedCharacter.Name;
        CharHealth.text = selectedCharacter.Health + " / " + selectedCharacter.MaxHealth;
        CharMove.text = selectedCharacter.Speed + " " + selectedCharacter.Movement;
    }

    public void TileHovered(int x, int y)
    {
        if (currentState == UIState.Movement)
        {
            GameObject tile = Instantiate(HoverTile);
            tile.GetComponent<GlowManager>().moveTo(x, y);
            Tiles[y, x].GetComponent<TileScript>().hoverTile = tile;
        }
        else if (currentState == UIState.Planning)
        {
            int index = 0;
            foreach(List<Node> list in UIInformationHandler.InformationStack.Peek().options)
            {
                if(list.Contains(map.map[y, x]))
                {
                    SelectedAbilityOptions = index;
                    foreach(Node tile in list)
                    {
                        GameObject hoverTile = Instantiate(HoverTile);
                        hoverTile.GetComponent<GlowManager>().moveTo(tile.X, tile.Y);
                        Tiles[tile.Y, tile.X].GetComponent<TileScript>().hoverTile = hoverTile;
                    }
                }
                index += 1;
            }
        }
    }

    public void TileExited(int x, int y)
    {  
        if(currentState == UIState.Planning)
        {
            foreach(List<Node> list in UIInformationHandler.InformationStack.Peek().options)
                if (list.Contains(map.map[y, x]))
                {
                    SelectedAbilityOptions = -1;
                    foreach (Node tile in list)
                    {
                        if(Tiles[tile.Y, tile.X].GetComponent<TileScript>().hoverTile != null)
                        {
                            Tiles[tile.Y, tile.X].GetComponent<TileScript>().DestroyHover();
                        }
                    }
                }
        }
    }

    public void TileClicked(int x, int y)
    {
        if (currentState == UIState.Movement && selectedCharacter.canMove)
        {
            foreach (Node node in Moves)
            {
                if (node.X == x && node.Y == y)
                {
                    foreach (GameObject tile in CharacterUI)
                    {
                        if (tile.GetComponent<CharacterSpriteScript>().X == selectedCharacter.X && tile.GetComponent<CharacterSpriteScript>().Y == selectedCharacter.Y)
                        {
                            tile.GetComponent<CharacterSpriteScript>().moveTo(x, y);
                            selectedCharacter.Move(y, x);
                            break;
                        }
                    }

                    Moves = map.map[selectedCharacter.Y, selectedCharacter.X].FindPossibleMoves((int)selectedCharacter.Movement, selectedCharacter.Speed, null, null, true, selectedCharacter.Faction);
                    UIUpdate();
                    break;
                }
            }
        }
        else if(currentState == UIState.Planning && SelectedAbilityOptions != -1 && selectedCharacter.canAct)
        {
            Debug.Log("Using Ability");
            UIInformationHandler.InformationStack.Peek().SelectOption(SelectedAbilityOptions);
            if(UIInformationHandler.InformationStack.Count == 0)
            {
                CleanMap();
                currentState = UIState.Default;
                UIUpdate();
                History.Clear();

                Character nextChar = null;
                for (int i = 0; i < gm.Factions[TurnManager.currentFactionTurn].Units.Count; ++i)
                {
                    if ((gm.Factions[TurnManager.currentFactionTurn].Units[i]).canAct)
                    {
                        nextChar = gm.Factions[TurnManager.currentFactionTurn].Units[i];
                        break;
                    }
                }
                if (nextChar == null)
                {
                    TurnManager.currentState = TurnState.TurnEnd;
                }
                else
                {
                    SelectCharacter(nextChar);
                }
            }
        }
        else if(currentState == UIState.Default)
        {
            if(map.map[y, x].myCharacter != selectedCharacter)
            {
                SelectCharacter(map.map[y, x].myCharacter);
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
                CleanMap();

                currentState = (UIState) History.Pop();
                break;
            default:
                currentState = UIState.Default;
                break;
        }
        UIUpdate();
    }

    void CleanMap()
    {
        if (currentState == UIState.Attacking || currentState == UIState.Planning)
        {
            List<GameObject> toDelete = new List<GameObject>();
            toDelete.AddRange(GameObject.FindGameObjectsWithTag("Abilities"));
            toDelete.AddRange(GameObject.FindGameObjectsWithTag("Hovers"));
            foreach (GameObject btn in toDelete)
            {
                Destroy(btn);
            }
            if (currentState == UIState.Planning)
            {
                foreach (Node tile in map.map)
                {
                    Tiles[tile.Y, tile.X].GetComponent<SpriteRenderer>().color = Tiles[tile.Y, tile.X].GetComponent<TileScript>().baseColor;
                }
            }
        }
        else if (currentState == UIState.Movement)
        {
            foreach (Node node in Moves)
            {
                Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Tiles[node.Y, node.X].GetComponent<TileScript>().baseColor;
            }
        }
    }

    void UIUpdate()
    {
        switch (currentState)
        {
            case UIState.Movement:
                drawMovementUI();
                selectedAbility = null;
                break;
            case UIState.Attacking:
                drawAttackingUI();
                selectedAbility = null;
                break;
            case UIState.Planning:
                drawSelectingUI();
                break;
            default:
                drawDefaultUI();
                selectedAbility = null;
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
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(AttackUI);
        unwanted.AddRange(BackUI);
        unwanted.AddRange(MoveUI);
        unwanted.AddRange(PlanningUI);
        foreach(GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in HomeUI)
        {
            obj.SetActive(true);
        }
        
    }

    void drawSelectingUI()
    {
        List<GameObject> planUI = new List<GameObject>();
        planUI.AddRange(PlanningUI);
        foreach (GameObject obj in planUI)
        {
            obj.SetActive(true);
        }

        if(UIInformationHandler.InformationStack.Count > 0)
        {
            UIInformation tempInfo = UIInformationHandler.InformationStack.Peek();
            AbilityName.text = selectedAbility.Method.Name;
            AbilityDescription.text = "Range: " +  Abilities.GetAbilityInfo(selectedAbility.Method.Name, AbilityInfo.Range) as string + "\nDescription: " +
            Abilities.GetAbilityInfo(selectedAbility.Method.Name, AbilityInfo.Description) as string;
            foreach(List<Node> list in tempInfo.options)
            {
                foreach(Node tile in list)
                {
                    Tiles[tile.Y, tile.X].GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
    }

    void drawMovementUI()
    {
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(AttackUI);
        unwanted.AddRange(HomeUI);
        foreach (GameObject obj in unwanted)
        {
            obj.SetActive(false);
        }
        List<GameObject> moveUI = new List<GameObject>();
        moveUI.AddRange(MoveUI);
        moveUI.AddRange(BackUI);
        foreach (GameObject obj in moveUI)
        {
            obj.SetActive(true);
        }
        Moves = map.map[selectedCharacter.Y, selectedCharacter.X].FindPossibleMoves((int)selectedCharacter.Movement, selectedCharacter.Speed ,null, null, true, selectedCharacter.Faction);

        foreach (Node node in map.map)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Tiles[node.Y, node.X].GetComponent<TileScript>().baseColor;
        }

        foreach (Node node in Moves)
        {
            Tiles[node.Y, node.X].GetComponent<SpriteRenderer>().color = Color.green;
        }
        
    }

    void drawAttackingUI()
    {
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(MoveUI);
        unwanted.AddRange(HomeUI);
        unwanted.AddRange(PlanningUI);
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
        
        List<GameObject> AbilityButtons = new List<GameObject>();
        float yOffset = 0.0f;
        foreach(Ability ability in selectedCharacter.MyAbilities)
        {
            Button button = Instantiate(abilityButton);
            button.transform.SetParent(CurrentCanvas.transform, false);
            button.GetComponentInChildren<Text>().text = ability.Method.Name;
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 0.5f);
            button.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.5f);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(125, yOffset);
            button.GetComponent<AbilityButtonScript>().gm = gameObject;
            button.GetComponent<AbilityButtonScript>().map = gm.map;
            button.GetComponent<AbilityButtonScript>().myChar = selectedCharacter;
            button.GetComponent<AbilityButtonScript>().script = ability;
            yOffset -= 60;
            button.GetComponent<Button>().onClick.AddListener(() => { abilityClicked(button.GetComponent<AbilityButtonScript>().script, selectedCharacter, map); });

        }
    }

    void abilityClicked(Ability script, Character myChar, GridHandler map)
    {
        Debug.Log(script.Method.Name);
        script(myChar, map);
        History.Push(currentState);
        currentState = UIState.Planning;
        selectedAbility = script;
        UIUpdate();
    }



	// Update is called once per frame
	void Update () {
        
	}
}
