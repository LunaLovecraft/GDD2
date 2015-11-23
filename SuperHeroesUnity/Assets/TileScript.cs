using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    public int x;
    public int y;
    UIManager uiMan;
    public Color baseColor;
    public GameObject hoverTile;

	// Use this for initialization
	void Start () {
        uiMan = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}

    void OnMouseDown()
    {
        uiMan.TileClicked(this.x, this.y);
    }

    void OnMouseEnter()
    {
        
        uiMan.TileHovered(this.x, this.y);
    }

    void OnMouseExit()
    {
        
        if(hoverTile != null)
        {
            Destroy(hoverTile);
            hoverTile = null;
        }
        uiMan.TileExited(this.x, this.y);
    }

    public void DestroyHover()
    {
        Destroy(hoverTile);
        hoverTile = null;
    }
}
