using UnityEngine;
using System.Collections;

/* Starts a game of snake. */
public class NewGame : MonoBehaviour 
{
	public GameManager manager;
	public GameObject currentMenu;

	public void OnMouseDown()
	{
		manager.enabled = true;
		manager.Initialise();
		currentMenu.SetActive(false);
	}
}
