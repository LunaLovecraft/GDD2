using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    
    GridHandler map;

	// Use this for initialization
	void Start () {
        
        map = gameObject.GetComponent<GameManager>().map;

        Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        tex.SetPixel(0, 0, Color.white);
        tex.Resize(100, 100);
        tex.Apply();

        for (int y = 0; y < map.Height; y++)
        {
            for(int x = 0; x < map.Width; x++)
            {
                Debug.Log("run # " + x);
                GameObject tempTile = Instantiate(Resources.Load("tile")) as GameObject;
                //tempTile.bounds.SetMinMax(new Vector3(x * 32, y * 32, -10), new Vector3(x * 32 + 32, y * 32 + 32, -10));
                tempTile.transform.position = new Vector3(x - 5 , y - 4.5f , 2);
                tempTile.GetComponent<TileScript>().x = x;
                tempTile.GetComponent<TileScript>().y = y;
            }
        }
	}

    public void TileClicked(int x, int y)
    {
        Debug.Log("The Tile at " + x + ", " + y + " is " + map.map[y, x].height);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
