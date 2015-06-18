using UnityEngine;
using System.Collections;

/* Initialises the game and handles its execution behaviour */
public class GameManager : MonoBehaviour
{	
	private Cell[][] grid;

	private void Start ()
	{
		// Initialises and fills the grid array.
		Transform field = GameObject.Find("Field").transform;
		grid = new Cell[field.childCount][];
		for (int i = 0; i < field.childCount; i++) {
			grid[i] = new Cell[field.GetChild(i).childCount];
			for (int j = 0; j < field.GetChild(i).childCount; j++) {
				grid[i][j] = (Cell)field.GetChild(i).GetChild(j).GetComponent(typeof(Cell));
			}
		}

		// Places the snake and a food item on the grid.
		//PlaceSnake();
		PlaceFood();
	}

	// Update is called once per frame
	private void Update () { }

	// Places the snake at a set length, direction and positions on the grid.
	private void PlaceSnake(int headPosX, int headPosY, int length, int direction)
	{

	}

	// Places a food item in a random position on the field.
	private void PlaceFood()
	{
		int randomCol, randomRow;
		do {
			randomCol = (int)(Random.value * grid.Length);
			randomRow = (int)(Random.value * grid [randomCol].Length);
		} while (grid[randomCol][randomRow].cellType != (int)Cell.States.CLEAR);
		grid[randomCol][randomRow].setCell((int)Cell.States.FOOD);
	}
}
//EditorApplication.isPaused = true