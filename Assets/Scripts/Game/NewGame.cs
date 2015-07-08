using UnityEngine;
using System.Collections;

/* Starts a game of snake. */
public class NewGame : MonoBehaviour 
{
	public void OnMouseDown()
	{
		GameObject.Find("GM").GetComponent<GameManager>().enabled = true;
		GameObject.Find("Main").SetActive(false);
	}
}
