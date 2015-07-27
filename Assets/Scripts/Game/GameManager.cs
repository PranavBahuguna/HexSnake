using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

/* Initialises the game and handles its execution behaviour */
public class GameManager : MonoBehaviour
{
	// The keys used in game for controls.
	public KeyCode moveUp;
	public KeyCode moveRight;
	public KeyCode moveDown;
	public KeyCode moveLeft;

	// Copies of the original placement parameters to record current position.
	private int tailPosX, tailPosY, length;
	private Direction tailDirection;

	// Keeps track of the snake head position on the grid;
	private int headPosX, headPosY;
	private Direction headDirection;

	private float speed = 5; 			// The current speed of the snake.

	public GameObject gameOverScreen;	// Access to game over screen.
	public Canvas canvas;				// Access to the game canvas.
	public Text score;					// Score displayed on UI.
	public Text bestScore;				// Best score displayed on UI.
	public Text finalScore;				// Score displayed on Game Over UI.
	private Level origLevel;			// Access to the original level.
	private Level level;				// A copy of the original level.

	private string levelName = "Open";	// Name of the selected level.
	private bool powerupsEnabled  =	false;
	private bool obstaclesEnabled = false;

	// A 2D grid of Cell objects that will be filled with existing Cell objects
	// in the scene.
	private Cell[][] grid;

	// Starts a new game of snake given the values of the current parameters.
	public void Initialise()
	{
		this.enabled = true;	// Allows updating

		// Obtains the level from the level name and creates a duplicate to use.
		origLevel = Extensions.FindObject(GameObject.Find("Levels"),
		                              levelName).GetComponent<Level>();
		level = Instantiate(origLevel);
		level.gameObject.SetActive(true);

		// Initialises and fills the grid array.
		grid = new Cell[level.transform.childCount][];
		for (int i = 0; i < level.transform.childCount; i++) {
			Transform col = level.transform.FindChild("Cells_Col_" +
				i.ToString());
			grid[i] = new Cell[col.childCount];
			for (int j = 0; j < level.transform.GetChild(i).childCount; j++) {
				Transform row = col.FindChild("Cell_" + j.ToString());
				grid[i][j] = (Cell)row.GetComponent(typeof(Cell));
			}
		}

		canvas.gameObject.SetActive(true);	// Activates the game canvas.
		score.text = "0"; 					// Score is reset

		// Sets all 'current' parameters to the value of the original values.
		tailPosX = level.tailPosX;
		tailPosY = level.tailPosY;
		length = level.snakeLength;
		tailDirection = level.tailDirection;
		headDirection = tailDirection;

		// Places the snake and a food item on the grid.
		PlaceSnake(tailPosX, tailPosY, length, tailDirection);
		PlaceFood();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Extensions.TimestepComplete(1 / speed))
		{
			// Obtains the new snake direction.
			headDirection = DirectionExtensions.GetDirection(headDirection,
                            	Input.GetKey(moveUp),
                             	Input.GetKey(moveRight),
                             	Input.GetKey(moveDown),
                 				Input.GetKey(moveLeft));

			// Calculates the new snake head position and checks if there will
			// be a crash there.
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

			// Snake crash case.
			if (newPosCellType != Cell.State.CLEAR &&
			    newPosCellType != Cell.State.FOOD) {
				EndGame();
				return;
			}

			// Eat food case - increases length and replaces food.
			if (newPosCellType == Cell.State.FOOD) {
				length++;
				PlaceFood();
			}

			// Moves snake head to new position and sets the old head cell to be
			// a body part if snake has a length greater than 2.
			grid[newHeadPosX][newHeadPosY].SetCell(Cell.State.SNAKE_HEAD,
            	DirectionExtensions.Opposite(headDirection), headDirection);
			if (length > 2) {
				grid[headPosX][headPosY].SetCell(Cell.State.SNAKE_BODY,
                	DirectionExtensions.Opposite(headDirection), headDirection);
			} else {
				grid[headPosX][headPosY].SetCell(Cell.State.SNAKE_HEAD,
                	DirectionExtensions.Opposite(headDirection), headDirection);
			}
			headPosX = newHeadPosX;
			headPosY = newHeadPosY;

 			// If the snake ate food, the tail is prevented from moving thus
			// incrementing length.
			if (newPosCellType == Cell.State.FOOD) {
				// Points added is equal to speed of snake
				UpdateScore((int)speed);
			}
			// Otherwise allows tail to move by calculating the new position.
			else {
				// Calculates the new tail position and clears old tail cell.
				grid[tailPosX][tailPosY].SetCell(Cell.State.CLEAR);
				tailPosX += DirectionExtensions.DeltaX(tailDirection);
				tailPosY += DirectionExtensions.DeltaY(tailDirection,
								isColUpper(tailPosX));

				// Corrects tail positions.
				correctedPosition = CorrectPosition(tailPosX, tailPosY);
				tailPosX = (int)correctedPosition.x;
				tailPosY = (int)correctedPosition.y;

				// Sets direction and type of new cell to be tail.
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

	// Takes x and y coordinates as arguments and checks whether that position
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

	// Places a food item in a random valid position on the level.
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

	// Allows the score to be updated given several parameters.
	private void UpdateScore(int rawPoints)
	{
		score.text = int.Parse(score.text) + rawPoints + "";
	}

	// Deals with event in which the snake crashes.
	public void EndGame()
	{
		finalScore.text = score.text;
		DestroyLevel();
		canvas.gameObject.SetActive(false);
		gameOverScreen.SetActive(true);
		this.enabled = false;
	}

	// Destroys the level copy
	public void DestroyLevel()
	{
		if (level != null) {
			Destroy(level.gameObject);
		}
	}

	// Allows the level copy to be set active or not.
	public void SetLevelActive(bool isActive)
	{
		level.gameObject.SetActive(isActive);
	}

	// levelName setter.
	public void SetLevelName(string levelName)
	{
		this.levelName = levelName;
	}

	// Allows snake speed to be set
	public void SetSpeed(float speed)
	{
		this.speed = (int)speed;
	}

	// Toggles powerupsEnabled bool.
	public void togglePowerups()
	{
		powerupsEnabled = !powerupsEnabled;
	}

	// Toggles obstaclesEnabled bool.
	public void toggleObstacles()
	{
		obstaclesEnabled = !obstaclesEnabled;
	}

	// Quits the game.
	public void Quit()
	{
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}
}
