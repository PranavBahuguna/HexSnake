using UnityEngine;
using System.Collections;
using UnityEditor;

/* Attached to the pause menu's 'quit' button - for now, 
 * pauses the editor. */
public class QuitGame : MonoBehaviour 
{
	private void OnMouseDown()
	{
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}
}
