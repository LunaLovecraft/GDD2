using UnityEngine;
using System.Collections;

/// <summary>
/// Logs all messages sent to it
/// </summary>
public static class MessageLog {

	// The maximum number of messages on the screen.
	public const int MAX_MESSAGES = 8; 

	// The array of messages
	private static string[] messages;
	public static string[] Messages { get { return messages;}}

	/// <summary>
	/// Initializes the container
	/// </summary>
	public static void Initialize(){
		messages = new string[MAX_MESSAGES];
		for (int i = 0; i < MAX_MESSAGES; ++i) {
			messages[i] = "";
		}
	}

	/// <summary>
	/// shifts all messages up by one, and appends it to the end of the list
	/// </summary>
	/// <param name="message">The message you want logged</param>
	public static void Log(string message){
		for (int i = 0; i < MAX_MESSAGES - 1; ++i) {
			messages[i] = messages[i + 1];
		}
		messages [MAX_MESSAGES - 1] = message;
	}

}
