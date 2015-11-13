using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public int x;
    public int y;
    UIManager uiMan;

	// Use this for initialization
	void Start () {
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}

    void OnMouseDown()
    {
        Debug.Log(this.x + ", " + this.y);
        uiMan.TileClicked(this.x, this.y);
    }
}
