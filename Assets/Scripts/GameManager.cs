using UnityEngine;
using System.Collections;

/* Initialises the game and handles its execution behaviour */
public class GameManager : MonoBehaviour
{	
	public int snakeTailPosX, snakeTailPosY, snakeLength;
	public Direction snakeDirection;

	// A 2D grid of Cell objects that will be filled by searching for existing Cells.
	private Cell[][] grid;

	private void Start ()
	{
		// Initialises and fills the grid array.
		Transform field = GameObject.Find("Field").transform;
		grid = new Cell[field.childCount][];
		for (int i = 0; i < field.childCount; i++) {
			Transform col = field.FindChild("Cells_Col_" + i.ToString());
			grid[i] = new Cell[col.childCount];
			for (int j = 0; j < field.GetChild(i).childCount; j++) {
				Transform row = col.FindChild("Cell_" + j.ToString());
				grid[i][j] = (Cell)row.GetComponent(typeof(Cell));
			}
		}

		for (int i = 0; i < 10; i++) {
			print (grid[1][i].gameObject);
		}

		// Places the snake and a food item on the grid.
		PlaceSnake(snakeTailPosX, snakeTailPosY, snakeLength, snakeDirection);
		PlaceFood();
	}

	// Update is called once per frame
	private void Update () { }

	// Places the snake at a set length, direction and positions on the grid.
	private void PlaceSnake(int tailPosX, int tailPosY, int length, Direction snakeOutDirection)
	{
		int segmentPosX = tailPosX;	// The horizontal position of the current snake segment.
		int segmentPosY = tailPosY; // The vertical position of the current snake segment.
		int segmentsLeft = length;	// Keeps track of the number of segments left to place.

		// Finds the opposite snake direction.
		Direction snakeInDirection = DirectionExtensions.Opposite(snakeOutDirection);

		// Iterates over each segment of the snake.
		while (segmentsLeft > 0) {

			// Decides which cell type to use given the number of segments left.
			int snakeCellType;
			if (segmentsLeft == length) {
				snakeCellType = (int)Cell.States.SNAKE_TAIL;
			} else if (segmentsLeft == 1) {
				snakeCellType = (int)Cell.States.SNAKE_HEAD;
			} else {
				snakeCellType = (int)Cell.States.SNAKE_BODY;
			}

			// Sets the cell at the current x/y position to be of the decided snake cell type.
			grid[segmentPosX][segmentPosY].SetCell(snakeCellType, snakeInDirection, snakeOutDirection);

			// Calculates the x/y positions for the next snake segment.
			segmentPosX += DirectionExtensions.DeltaX(snakeOutDirection);
			segmentPosY += DirectionExtensions.DeltaY(snakeOutDirection, isColUpper(segmentPosX));
			segmentsLeft--;	// Decrements segmentsLeft count.
		}
	}

	// Places a food item in a random position on the field.
	private void PlaceFood()
	{
		int randomCol, randomRow;
		do {
			randomCol = (int)(Random.value * grid.Length);
			randomRow = (int)(Random.value * grid [randomCol].Length);
		} while (grid[randomCol][randomRow].cellType != (int)Cell.States.CLEAR);
		grid[randomCol][randomRow].SetCell((int)Cell.States.FOOD);
	}

	// Determines whether a column of cells in the grid appears 'higher' than its neighbouring
	// columns.
	private bool isColUpper(int col)
	{
		return (col % 2 == 0);
	}
}
//EditorApplication.isPaused = true



















