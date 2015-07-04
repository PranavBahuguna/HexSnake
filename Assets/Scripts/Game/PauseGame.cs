using UnityEngine;
using System.Collections;

/* Provides a method to allow the game to be paused on/off */
public class PauseGame : MonoBehaviour
{
	public KeyCode pause;	// Pause key
	private GameManager gm; // Accessor to GameManager script.
	
	private void Start() 
	{
		gm = GetComponent<GameManager>();
	}
	
	private void Update() 
	{
		// Stops or allows the the gm script to be updated when
		// pause key is pressed.
		if (Input.GetKeyUp (pause)) {
			gm.enabled = !gm.enabled;
		}
	}
}