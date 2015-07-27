using UnityEngine;
using System.Collections;

/* Provides a method to allow the game to be paused on/off */
public class PauseGame : MonoBehaviour
{
	public KeyCode pause;			// Pause key.
	public GameObject pauseMenu;	// Accessor to the pause menu.
	private GameManager gm; 		// Accessor to GameManager script.

	private void Start()
	{
		gm = GetComponent<GameManager>();
		pauseMenu.SetActive(false);		// Off by default.
	}

	// Calls Pause() if pause key is pressed during gameplay.
	private void Update()
	{
		if (Input.GetKeyUp (pause) && gm.IsGameRunning()) {
			Pause();
		}
	}

	// Pauses/unpauses game, based on whether it was previously paused or not.
	public void Pause()
	{
		gm.enabled = !gm.enabled;
		pauseMenu.SetActive(!pauseMenu.activeSelf);
	}
}
