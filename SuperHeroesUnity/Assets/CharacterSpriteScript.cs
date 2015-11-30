using UnityEngine;
using System.Collections;

public class CharacterSpriteScript : MonoBehaviour {

    private int x;
    private int y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void moveTo(int x, int y)
    {
        gameObject.transform.position = new Vector3(x - 3.0f, y - 4.5f, -0.5f);
        X = x;
        Y = y;
    }

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
}
