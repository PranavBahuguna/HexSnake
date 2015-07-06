using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* Initialises the game and handles its execution behaviour */
public class GameManager : MonoBehaviour
{
	// The keys used in game for controls.
	public KeyCode moveUp;
	public KeyCode moveRight;
	public KeyCode moveDown;
	public KeyCode moveLeft;

	// Parameters for snake placement on the Cell grid.
	public int tailPosX, tailPosY, snakeLength;
	public Direction tailDirection;

	public float snakeSpeed;		// How quickly the snake moves.
	private float currentSpeed; 	// The current speed of the snake.

	// Keeps track of the snake head position on the grid;
	private int headPosX, headPosY;
	private Direction headDirection;

	// Whether the snake segment has collided with something.
	private bool snakeCrashed = false;

	// A 2D grid of Cell objects that will be filled with existing Cell objects
	// in the scene.
	private Cell[][] grid;

	// The current score displayed on UI
	private Text score;

	private void Start ()
	{
		// Obtains UI Text element for score.
		score = GameObject.Find("Score_Number").GetComponent<Text>();

		// Initialises and fills the grid array.
		Transform field = GameObject.Find("Field_0").transform;
		grid = new Cell[field.childCount][];
		for (int i = 0; i < field.childCount; i++) {
			Transform col = field.FindChild("Cells_Col_" + i.ToString());
			grid[i] = new Cell[col.childCount];
			for (int j = 0; j < field.GetChild(i).childCount; j++) {
				Transform row = col.FindChild("Cell_" + j.ToString());
				grid[i][j] = (Cell)row.GetComponent(typeof(Cell));
			}
		}

		// currentSpeed is set to match the snake speed.
		currentSpeed = snakeSpeed;

		// Places the snake and a food item on the grid.
		PlaceSnake(tailPosX, tailPosY, snakeLength, tailDirection);
		PlaceFood();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Extensions.TimestepComplete(1 / currentSpeed) && !snakeCrashed)
		{
			// Obtains the new snake direction.
			headDirection = DirectionExtensions.GetDirection(headDirection,
                            	Input.GetKey(moveUp),
                             	Input.GetKey(moveRight),
                             	Input.GetKey(moveDown),
                 				Input.GetKey(moveLeft));

			// Calculates the new snake head position and checks if there
			// will be a crash there.
			int newHeadPosX = headPosX +
				DirectionExtensions.DeltaX(headDirection);
			int newHeadPosY = headPosY +
				DirectionExtensions.DeltaY(headDirection,
					isColUpper(newHeadPosX));

			// Corrects head positions.
			Vector2 correctedPosition = CorrectPosition(newHeadPosX, newHeadPosY);
			newHeadPosX = (int)correctedPosition.x;
			newHeadPosY = (int)correctedPosition.y;

			// Finds the cell type of the new position.
			Cell.State newPosCellType = grid[newHeadPosX][newHeadPosY].cellType;

			if (newPosCellType != Cell.State.CLEAR &&
			    newPosCellType != Cell.State.FOOD) {
				// For now, stops game.
				Debug.Break();
				return;
			}

			// If the new cell is of food type, score is incremented and food
			// is replaced on the grid.
			if (newPosCellType == Cell.State.FOOD) {
				snakeLength++;
				PlaceFood();
			}

			// Moves snake head to new position and sets the old head cell to be
			// a body part if snake has a length greater than 2.
			grid[newHeadPosX][newHeadPosY].SetCell(Cell.State.SNAKE_HEAD,
            	DirectionExtensions.Opposite(headDirection), headDirection);
			if (snakeLength > 2) {
				grid[headPosX][headPosY].SetCell(Cell.State.SNAKE_BODY,
                	DirectionExtensions.Opposite(headDirection), headDirection);
			} else {
				grid[headPosX][headPosY].SetCell(Cell.State.SNAKE_HEAD,
                	DirectionExtensions.Opposite(headDirection), headDirection);
			}
			headPosX = newHeadPosX;
			headPosY = newHeadPosY;

 			// If the snake ate food, the tail is prevented from moving, thus
			// incrementing the snake length.
			if (newPosCellType == Cell.State.FOOD) {
				UpdateScore(10); // For now, simply adds 10 points to score.
			} 
			// Otherwise allows tail to move by calculating the new pos/direction.
			else {
				// Calculates the new tail position and sets the old tail cell clear.
				grid[tailPosX][tailPosY].SetCell(Cell.State.CLEAR);
				tailPosX += DirectionExtensions.DeltaX(tailDirection);
				tailPosY += DirectionExtensions.DeltaY(tailDirection,
								isColUpper(tailPosX));

				// Corrects tail positions, then sets
				correctedPosition = CorrectPosition(tailPosX, tailPosY);
				tailPosX = (int)correctedPosition.x;
				tailPosY = (int)correctedPosition.y;

				// Sets direction and type of new cell to be a tail.
				tailDirection = grid[tailPosX][tailPosY].GetOutDirection();
				grid[tailPosX][tailPosY].SetCell(Cell.State.SNAKE_TAIL,
	            	DirectionExtensions.Opposite(tailDirection), tailDirection);
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
				headPosX = segmentPosX;
				headPosY = segmentPosY;
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

	// Takes an x and y coordinates as arguments and checks whether that position
	// is contained within the grid. If not, it returns a new position at the
	// opposite edge to where the old position was out of bounds at.
	private Vector2 CorrectPosition(int x, int y)
	{
		int newPosX, newPosY;

		// Corrects x position.
		if (x < 0) {
			newPosX = grid.Length - 1;
		} else if (x >= grid.Length) {
			newPosX = 0;
		} else {
			newPosX = x;
		}

		// Corrects y position.
		if (y < 0) {
			newPosY = grid[newPosX].Length - 1;
		} else if (y >= grid[newPosX].Length) {
			newPosY = 0;
		} else {
			newPosY = y;
		}

		return new Vector2(newPosX, newPosY);
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

	// Determines whether a column of cells in the grid appears 'higher' than
	// its neighbouring columns.
	private bool isColUpper(int col)
	{
		return (col % 2 == 0);
	}

	// Allows the score to be updated at a certain event given several
	// circumstances.
	private void UpdateScore(int rawPoints)
	{
		score.text = int.Parse(score.text) + rawPoints + "";
	}

	// Allows snake speed to be set
	public void SetSnakeSpeed(float snakeSpeed)
	{
		this.snakeSpeed = snakeSpeed;
	}
}
