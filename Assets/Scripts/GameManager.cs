using UnityEngine;
using System.Collections;

/* Initialises the game and handles its execution behaviour */
public class GameManager : MonoBehaviour
{
	// Parameters for snake placement on the Cell grid.
	public int snakeTailPosX, snakeTailPosY, snakeLength;
	public Direction snakeDirection;

	public float speed = 1.0f;	// How quickly the snake moves.

	// Keeps track of the snake head position on the grid;
	private int snakeHeadPosX, snakeHeadPosY;

	// Whether the snake segment has collided with something.
	private bool snakeCrashed = false;

	// A 2D grid of Cell objects that will be filled with existing Cell objects
	// in the scene.
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

		// Places the snake and a food item on the grid.
		PlaceSnake(snakeTailPosX, snakeTailPosY, snakeLength, snakeDirection);
		PlaceFood();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Extensions.TimestepComplete (1 / speed)) {
			if (! snakeCrashed) {
				moveSnake();
			}
		}
	}

	// Places the snake at a set length, direction and positions on the grid.
	private void PlaceSnake(int tailPosX, int tailPosY, int length,
							Direction outDirection)
	{
		int segmentPosX = tailPosX;	// Horizontal pos of current snake segment.
		int segmentPosY = tailPosY; // Vertical pos of current snake segment.
		int segmentsLeft = length;	// Keeps number of segments left to place.

		// Finds the opposite snake direction.
		Direction inDirection = DirectionExtensions.Opposite(outDirection);

		// Iterates over each segment of the snake.
		while (segmentsLeft > 0) {

			// Decides which cell type to use given the number of segments left.
			Cell.State snakeCellType;
			if (segmentsLeft == length) {
				snakeCellType = Cell.State.SNAKE_TAIL;
			} else if (segmentsLeft == 1) {
				snakeCellType = Cell.State.SNAKE_HEAD;
				snakeHeadPosX = segmentPosX;
				snakeHeadPosY = segmentPosY;
			} else {
				snakeCellType = Cell.State.SNAKE_BODY;
			}

			// Sets cell at x/y pos to be of the decided snake cell type.
			grid[segmentPosX][segmentPosY].SetCell(snakeCellType,
				inDirection, outDirection);

			// Calculates the x/y positions for the next snake segment.
			segmentPosX += DirectionExtensions.DeltaX(outDirection);
			segmentPosY += DirectionExtensions.DeltaY(outDirection,
				isColUpper(segmentPosX));
			segmentsLeft--;	// Decrements segmentsLeft count.
		}
	}

	// Places a food item in a random position on the field.
	private void PlaceFood()
	{
		// Assumes there is at least one clear cell in the grid. If not clear,
		// another random cell is chosen until a clear cell is found. That Cell
		// has its state set to Food.
		int randomCol, randomRow;
		do {
			randomCol = (int)(Random.value * grid.Length);
			randomRow = (int)(Random.value * grid [randomCol].Length);
		}
		while (grid[randomCol][randomRow].cellType != Cell.State.CLEAR);
		grid[randomCol][randomRow].SetCell(Cell.State.FOOD);
	}

	// Attempts to move the snake by one unit to a new position.
	private void moveSnake()
	{
	}

	// Determines whether a column of cells in the grid appears 'higher' than
	// its neighbouring columns.
	private bool isColUpper(int col)
	{
		return (col % 2 == 0);
	}
}
