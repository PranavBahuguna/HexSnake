using UnityEngine;
using System.Collections;

/* To be attached to the pause menu's 'resume' button. This unpauses the game. */
public class ResumeGame : MonoBehaviour
{
	private PauseGame pauseGame;	// PauseGame script accessor.

	public void Start()
	{
		pauseGame = GameObject.Find("GM").GetComponent<PauseGame>();
	}

	// Calls the pauseGame's Pause function to unpause the game.
	private void OnMouseDown()
	{
		pauseGame.Pause();
	}
}
