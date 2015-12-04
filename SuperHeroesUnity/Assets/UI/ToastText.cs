using UnityEngine;
using System.Collections;


public class ToastText : MonoBehaviour {

	public event PostToast endOfLife;
	public float distanceMoved = 0;
	public float startTime = 0.0f;
	public bool alive = false;
	public bool reverse = false;
	public bool wait = false;


	// Use this for initialization
	public void Initialize (string text) {
		this.GetComponent<TextMesh> ().text = text;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (alive) {
			if(!reverse){
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.08f, this.transform.position.z);
				distanceMoved += 0.08f;
				if(distanceMoved >= 4.0f){
					this.transform.position = new Vector3(this.transform.position.x, 4.0f, this.transform.position.z);
					reverse = true;
					wait = true;
					startTime = Time.fixedTime;
				}
			}
			else
			{
				if(wait){
					if(Time.fixedTime - startTime > 1)//2 seconds
					{
						wait = false;
					}
				}
				else{
					this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.02f, this.transform.position.z);
					distanceMoved -= 0.02f;
					if(distanceMoved <= 0.0f){
						if(endOfLife != null){
							endOfLife();
						}
					}
				}
			}

		}
	}
}
