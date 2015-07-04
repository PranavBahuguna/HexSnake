using UnityEngine;
using System.Collections;

/* Attached to the pause menu's 'quit' button - for now, 
 * pauses the editor. */
public class QuitGame : MonoBehaviour 
{
	private void OnMouseDown()
	{
		Debug.Break();
	}
}
