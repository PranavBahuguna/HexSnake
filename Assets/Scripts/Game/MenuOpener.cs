using UnityEngine;
using System.Collections;

/* A simple class that allows a clickable button to open a 
 * new menu and close the current one when clicked. */
public class MenuOpener : MonoBehaviour 
{
	public GameObject thisMenu;		// Menu that button is attached to.
	public GameObject targetMenu; 	// Menu that is opened on click.

	// When clicked, opens the target menu by enabling said menu and
	// disabling this one.
	private void OnMouseDown()
	{
		targetMenu.SetActive(true);
		thisMenu.SetActive(false);
	}
}
