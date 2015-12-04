using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void PostToast();

public static class Toast{

	private static GameObject messagePrefab;
	private static Queue<GameObject> outgoingToasts;

	public static void Initialize(GameObject textPrefab){
		messagePrefab = textPrefab;
		outgoingToasts = new Queue<GameObject> ();
	}

	public static void SendToast(string message){
		GameObject newToast = GameObject.Instantiate (messagePrefab, new Vector3 (1.66f, 8.0f, -2.2f), Quaternion.identity) as GameObject;
		newToast.GetComponent<ToastText>().Initialize(message);
		PostToast myToast = NextToast;
		newToast.GetComponent<ToastText> ().endOfLife += myToast;
		outgoingToasts.Enqueue(newToast);
		if (outgoingToasts.Count == 1) {
			outgoingToasts.Peek().GetComponent<ToastText>().alive = true;
		}
	}

	public static void SendToast(string message, PostToast endFunction){
		GameObject newToast = GameObject.Instantiate (messagePrefab, new Vector3 (1.66f, 8.0f, -2.2f), Quaternion.identity) as GameObject;
		newToast.GetComponent<ToastText>().Initialize(message);
		PostToast myToast = NextToast;
		newToast.GetComponent<ToastText> ().endOfLife += endFunction;
		newToast.GetComponent<ToastText> ().endOfLife += myToast;
		outgoingToasts.Enqueue(newToast);
		if (outgoingToasts.Count == 1) {
			outgoingToasts.Peek().GetComponent<ToastText>().alive = true;
		}
	}

	public static void NextToast(){
		GameObject.Destroy (outgoingToasts.Dequeue());
		if (outgoingToasts.Count != 0) {
			outgoingToasts.Peek().GetComponent<ToastText>().alive = true;
		}
	}
}
