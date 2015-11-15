using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public int x;
    public int y;
    UIManager uiMan;
    public Color baseColor;

	// Use this for initialization
	void Start () {
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
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
