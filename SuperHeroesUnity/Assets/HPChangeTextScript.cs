using UnityEngine;
using System.Collections;

public class HPChangeTextScript : MonoBehaviour {

    float magnitude;
	// Use this for initialization
	void Start () {
        magnitude = Random.Range(-0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += new Vector3(0, 0.75f + magnitude , 0);
	}
}
