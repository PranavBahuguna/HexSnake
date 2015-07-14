using UnityEngine;
using System.Collections;

/* This class provides a management system for menu screens,
 * providing a method to allow switching from one menu to another. */
public class MenuManager : MonoBehaviour
{
	public GameObject[] menus;

	public void OpenMenu(int menuNum)
	{
		// Closes all currently open menus.
		foreach (GameObject menu in menus) {
			menu.SetActive(false);
		}

		// If menuNum equals -1, no menus are opened, otherwise a menu
		// from menus (using menuNum as index) is opened.
		if (menuNum != -1) {
			menus[menuNum].SetActive(true);
		}
	}
}
