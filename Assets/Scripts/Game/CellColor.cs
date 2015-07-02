using UnityEngine;
using System.Collections;

// This singleton class enables the color of a cell to be set
// according to the state of the cell.
public class CellColor : MonoBehaviour
{	
	// The respective possible colors for cells.
	public Color clear;
	public Color obstacle;
	public Color food;
	public Color snakeHead;
	public Color snakeBody;
	public Color snakeTail;

	// Returns the appropriate color given the state of the cell.
	public Color GetColor(Cell.State state) 
	{
		switch (state) {
			case (Cell.State.CLEAR):		return clear;
			case (Cell.State.OBSTACLE): 	return obstacle;
			case (Cell.State.FOOD):			return food;
			case (Cell.State.SNAKE_HEAD): 	return snakeHead;
			case (Cell.State.SNAKE_BODY): 	return snakeBody;
			case (Cell.State.SNAKE_TAIL): 	return snakeTail;
			default:						return clear;
		}
	}
}
