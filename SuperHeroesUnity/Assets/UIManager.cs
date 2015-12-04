using UnityEngine;
using System.Collections;
using System;
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
    public GameObject HPChangeText;
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
    public GameObject AbilityPanel;
    public Text CharName;
    public Text CharHealth;
    public Text CharMove;
    public Text CharTraits;
    Vector2 GlobalAnchor = new Vector2(0.0f, 0.5f);


    public GameObject indicator;

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
                tempTile.transform.position = new Vector3(x - 3.0f, y - 4.5f, 0);
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
                    character.name = map.map[y, x].myCharacter.Name;
                    character.GetComponent<CharacterSpriteScript>().moveTo(x, y);
                    character.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("CharacterSprites/" + map.map[y, x].myCharacter.Name);
                    if (map.map[y, x].myCharacter.Faction == 0)
                    {
                        character.GetComponent<CharacterSpriteScript>().X = x;
                        character.GetComponent<CharacterSpriteScript>().Y = y;
                        SpriteRenderer charSprite = character.transform.FindChild("CharacterOutline").GetComponent<SpriteRenderer>();
                        charSprite.color = Color.blue;
                        charSprite.sprite = Resources.Load<Sprite>("CharacterOutlines/" + map.map[y, x].myCharacter.Name + "_OUTLINE");
                    }
                    else
                    {
                        character.transform.FindChild("CharacterOutline").GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    CharacterUI.Add(character);
         
                }
            }
        }

        currentState = UIState.Default;
        UIUpdate();
    }

    public void ShowDamage(object sender, EventArgs e)
    {
        DamageEventArgs data = (DamageEventArgs)e;
        GameObject damageText = Instantiate(HPChangeText);
        damageText.GetComponent<Text>().text = data.damage.ToString();
        damageText.GetComponent<Text>().color = Color.red;
        RectTransform CanvasRect = CurrentCanvas.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(Tiles[data.myChar.Y, data.myChar.X].transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2( ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        damageText.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        damageText.transform.SetParent(CurrentCanvas.transform, false);
        Destroy(damageText, 2.5f);
        
    }

    public void ShowHeal(object sender, EventArgs e)
    {
        HealEventArgs data = (HealEventArgs)e;
        GameObject damageText = Instantiate(HPChangeText);
        damageText.GetComponent<Text>().text = data.heal.ToString();
        damageText.GetComponent<Text>().color = Color.green;
        RectTransform CanvasRect = CurrentCanvas.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(Tiles[data.myChar.Y, data.myChar.X].transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        damageText.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        damageText.transform.SetParent(CurrentCanvas.transform, false);
        Destroy(damageText, 2.5f);
        
    }

    void positionAnchors()
    {
        List<GameObject> elements = new List<GameObject>();
        elements.AddRange(BackUI);
        elements.AddRange(AttackUI);
        elements.AddRange(HomeUI);
        elements.AddRange(MoveUI);
        elements.AddRange(PlanningUI);
        elements.AddRange(GameObject.FindGameObjectsWithTag("AbilityPanelUI"));

        foreach(GameObject obj in elements)
        {
            if(obj.GetComponent<RectTransform>() != null)
            {
                Vector3 savePos = obj.GetComponent<RectTransform>().anchoredPosition3D;
                if (selectedCharacter.Faction == 0)
                {
                    GlobalAnchor = new Vector2(1.0f, 0.5f);
                    obj.GetComponent<RectTransform>().anchorMax = GlobalAnchor;
                    obj.GetComponent<RectTransform>().anchorMin = GlobalAnchor;
                }
                else
                {
                    GlobalAnchor = new Vector2(0.0f, 0.5f);
                    obj.GetComponent<RectTransform>().anchorMax = GlobalAnchor;
                    obj.GetComponent<RectTransform>().anchorMin = GlobalAnchor;
                }
                obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-savePos.x, savePos.y, savePos.z);
            }
        }
    }

    public void SelectCharacter(Character newSelection)
    {

        if (newSelection.canAct)
        {
            foreach(GameObject character in CharacterUI)
            {
                if(character.GetComponent<CharacterSpriteScript>().X == newSelection.X && character.GetComponent<CharacterSpriteScript>().Y == newSelection.Y)
                {
                    indicator.transform.SetParent(character.transform, false);
                    indicator.GetComponent<SpriteRenderer>().color = character.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                }
            }

            foreach (GameObject obj in HomeUI)
            {
                if (obj.GetComponent<Button>() != null)
                {
                    obj.GetComponent<Button>().interactable = true;
                }
            }
        }
        else
        {
            foreach (GameObject obj in HomeUI)
            {
                if (obj.GetComponent<Button>() != null)
                {
                    obj.GetComponent<Button>().interactable = false;
                }
            }
        }

        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("AbilityPanelUI"));

        foreach(GameObject obj in unwanted)
        {
            Destroy(obj);
        }
        
        selectedCharacter = newSelection;
        CharName.text = selectedCharacter.Name;
        CharHealth.text = selectedCharacter.Health + " / " + selectedCharacter.MaxHealth;
        CharMove.text = selectedCharacter.Speed + " " + selectedCharacter.Movement;
        string traits = "";
        foreach(string trait in selectedCharacter.TraitNames)
        {
            traits += trait + " ";
        }
        CharTraits.text = traits;
        GameObject CharPanel = GameObject.Find("CharacterInfoPanel");
        if (newSelection.Faction == 0)
        {
            CharPanel.GetComponent<Image>().color = Color.blue;
        }
        else CharPanel.GetComponent<Image>().color = Color.red;


        float yOffset = 0.0f;
        Debug.Log(newSelection.Abilities.Count);
        foreach(Ability ability in newSelection.Abilities)
        {
            GameObject abilityPanel = Instantiate(AbilityPanel);
            abilityPanel.transform.SetParent(CurrentCanvas.transform, false);
            abilityPanel.GetComponent<RectTransform>().anchorMin = GlobalAnchor;
            abilityPanel.GetComponent<RectTransform>().anchorMax = GlobalAnchor;
            if(selectedCharacter.Faction == 0)
            { 
                abilityPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(334.0f, -yOffset);
            }
            else
            {
                abilityPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-334.0f, -yOffset);
            }
            yOffset += 150.0f;
            List<string> abilityText = new List<string>();
            string name = ability.Method.Name;
            abilityText.Add(name);
            int abilityRange = (int)Abilities.GetAbilityInfo(ability.Method.Name, AbilityInfo.Range);
            string abilityDescription = Abilities.GetAbilityInfo(ability.Method.Name, AbilityInfo.Description) as string;
            abilityText.Add("Range: " + abilityRange + "\n" + abilityDescription);
            for(int i = 0; i < abilityPanel.transform.childCount; i++)
            {
                abilityPanel.transform.GetChild(i).gameObject.GetComponent<Text>().text = abilityText[i];
            }
        }


        //AbilityName.text = selectedAbility.Method.Name;
        //AbilityDescription.text = "Range: " + Abilities.GetAbilityInfo(selectedAbility.Method.Name, AbilityInfo.Range) as string + "\nDescription: " +
        //Abilities.GetAbilityInfo(selectedAbility.Method.Name, AbilityInfo.Description) as string;
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
                    positionAnchors();
                }
                else
                {
                    SelectCharacter(nextChar);
                }
            }
        }
        else if(currentState == UIState.Default)
        {
            if(map.map[y, x].myCharacter != selectedCharacter && map.map[y, x].myCharacter != null)
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
            if(obj.GetComponent<Button>()!= null)
            obj.GetComponent<Button>().interactable = false;
        }
        foreach(GameObject obj in HomeUI)
        {
            if (obj.GetComponent<Button>() != null)
            obj.GetComponent<Button>().interactable = true;
        }
        
    }

    void drawSelectingUI()
    {
        List<GameObject> unwanted = new List<GameObject>();
        unwanted.AddRange(GameObject.FindGameObjectsWithTag("Abilities"));
        foreach (GameObject obj in unwanted)
        {
            if (obj.GetComponent<Button>() != null)
                obj.GetComponent<Button>().interactable = false;
        }

        List<GameObject> planUI = new List<GameObject>();
        planUI.AddRange(PlanningUI);
        foreach (GameObject obj in planUI)
        {
            if (obj.GetComponent<Button>() != null)
                obj.GetComponent<Button>().interactable = true;
        }

        if(UIInformationHandler.InformationStack.Count > 0)
        {
            UIInformation tempInfo = UIInformationHandler.InformationStack.Peek();
            
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
            if (obj.GetComponent<Button>() != null)
            obj.GetComponent<Button>().interactable = false;
        }
        List<GameObject> moveUI = new List<GameObject>();
        moveUI.AddRange(MoveUI);
        moveUI.AddRange(BackUI);
        foreach (GameObject obj in moveUI)
        {
            if (obj.GetComponent<Button>() != null)
            obj.GetComponent<Button>().interactable = true;
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
            if (obj.GetComponent<Button>() != null)
            obj.GetComponent<Button>().interactable = false;
        }
        List<GameObject> attackUI = new List<GameObject>();
        attackUI.AddRange(AttackUI);
        attackUI.AddRange(BackUI);
        foreach (GameObject obj in attackUI)
        {
            if (obj.GetComponent<Button>() != null)
            obj.GetComponent<Button>().interactable = true;
        }
        
        List<GameObject> AbilityButtons = new List<GameObject>();
        float yOffset = 0.0f;
        foreach(Ability ability in selectedCharacter.MyAbilities)
        {
            Button button = Instantiate(abilityButton);
            button.transform.SetParent(CurrentCanvas.transform, false);
            button.GetComponentInChildren<Text>().text = ability.Method.Name;
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.GetComponent<RectTransform>().anchorMax = GlobalAnchor;
            button.GetComponent<RectTransform>().anchorMin = GlobalAnchor;
            
            if(selectedCharacter.Faction == 0)
            {
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(125, yOffset);
            }
            else
            {
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, yOffset);
            }
            button.GetComponent<AbilityButtonScript>().gm = gameObject;
            button.GetComponent<AbilityButtonScript>().map = gm.map;
            button.GetComponent<AbilityButtonScript>().myChar = selectedCharacter;
            button.GetComponent<AbilityButtonScript>().script = ability;
            yOffset -= 150;
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
