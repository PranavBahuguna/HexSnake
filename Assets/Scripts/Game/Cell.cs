using UnityEngine;
using System.Collections;

/* Defines a hexagonal cell, which can possess several different states
 * and are arranged together to form the snake grid. */
public class Cell : MonoBehaviour
{
	// These are the possible states that a cell can be in.
	public enum State {CLEAR, OBSTACLE, FOOD, SNAKE_HEAD, SNAKE_BODY, SNAKE_TAIL};
	
	public State cellType;		 	// The current state of the cell.
	private CellColor cellColor;	// Accessor to the CellColor singleton.

	// The in and out directions of the cell (if it is a snake type).
	private Direction inDirection, outDirection;

	// Initializes the cellColors array and the cell.
	private void Awake()
	{
		cellColor = GameObject.Find("CellColor").GetComponent<CellColor>();
		SetCell(cellType);
	}

	// Sets the type of the cell and applies its associated color to the
	// cell sprite.
	public void SetCell(State cellType)
	{
		this.cellType = cellType;
		GetComponent<SpriteRenderer>().color = cellColor.GetColor(cellType);
	}

	// This variant of SetCell() allows the snake in/out direction to be set as
	// well.
	public void SetCell(State cellType, Direction inDirection,
						Direction outDirection)
	{
		this.cellType = cellType;
		this.inDirection = inDirection;
		this.outDirection = outDirection;
		GetComponent<SpriteRenderer>().color = cellColor.GetColor(cellType);
	}

	// Getter method for outDirection()
	public Direction GetOutDirection()
	{
		return outDirection;
	}

	// Getter method for inDirection()
	public Direction GetInDirection()
	{
		return inDirection;
	}
}
