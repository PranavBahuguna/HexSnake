using UnityEngine;
using System.Collections;

/* This class provides a management system for menu screens, providing a method
 * to allow switching from one menu to another. */
public class MenuManager : MonoBehaviour
{
	public GameObject[] menus;	// A list of menus controlled by the manager.

	// 'Opens' a menu taking integer argument as index for menus[] array. If
	// argument set to -1, no new menu is opened.
	public void OpenMenu(int menuNum)
	{
		// Closes all currently open menus, then opens the selected menu.
		foreach (GameObject menu in menus) {
			menu.SetActive(false);
		}

		if (menuNum != -1) {
			menus[menuNum].SetActive(true);
		}
	}
}
