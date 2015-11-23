using UnityEngine;
using System.Collections;

public class GlowManager : MonoBehaviour {
    SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b,  (Mathf.Cos(Time.time * 7.5f) + 1) / 2);
	}

    public void moveTo(int x, int y)
    {
        transform.position = new Vector3(x - 5, y - 4.5f, -0.5f);
    }
}
