using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UIInformationHandler {
    public static Stack<UIInformation> InformationStack = new Stack<UIInformation>();
}

public class UIInformation
{
    public Character myChar;
    public GridHandler map;
    public List<List<Node>> options;
    public int affectedFaction;
    public AbilityAddition selectFunction;

    public UIInformation(Character myChar, GridHandler map, List<List<Node>> options, int affectedFaction, AbilityAddition selectFunction)
    {
        this.myChar = myChar;
        this.map = map;
        this.options = options;
        this.affectedFaction = affectedFaction;
        this.selectFunction = selectFunction;
    }

    public void SelectOption(int optionIndex){
        selectFunction(myChar, map, affectedFaction, options[optionIndex]);
    }

}