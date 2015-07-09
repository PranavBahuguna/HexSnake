using UnityEngine;
using System.Collections;

/* Starts a game of snake. */
public class NewGame : MonoBehaviour
{
	public GameManager manager;		// GameManager script access.
	public GameObject currentMenu;	// Menu this button is on.

	// Enables and initialises the game manager, and deactivates the menu it was
	// activated from.
	public void OnMouseDown()
	{
		manager.enabled = true;
		manager.Initialise();
		currentMenu.SetActive(false);
	}
}
