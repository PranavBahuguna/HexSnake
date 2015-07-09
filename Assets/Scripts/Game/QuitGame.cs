using UnityEngine;
using System.Collections;
using UnityEditor;

/* Attached to the pause menu's 'quit' button - for now stops the editor. */
public class QuitGame : MonoBehaviour
{
	private void OnMouseDown()
	{
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}
}
