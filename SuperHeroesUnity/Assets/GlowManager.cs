using UnityEngine;
using System.Collections;

public class GlowManager : MonoBehaviour {
    SpriteRenderer myRenderer;
    public bool glowing = true;
	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, (Mathf.Cos(Time.time * 7.5f) + 1) / 2);
        }
        else myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0);
        
	}

    public void moveTo(int x, int y)
    {
        transform.position = new Vector3(x - 3.0f, y - 4.5f, -0.5f);
    }
}
