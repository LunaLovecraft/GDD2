using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Faction {

    private List<Character> units;
    public List<Character> Units { get { return units; } }

    public Faction(List<Character> units)
    {
        this.units = units; 
    }
}
