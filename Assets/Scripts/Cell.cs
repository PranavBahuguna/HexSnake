﻿using UnityEngine;
using System.Collections;

/* Defines a hexagonal cell, which can possess several different states
 * and are arranged together to form the snake grid. */
public class Cell : MonoBehaviour
{
	// These are the possible states that a cell can be in.
	public enum State {CLEAR, BORDER, FOOD, SNAKE_HEAD, SNAKE_BODY, SNAKE_TAIL,
					   OBSTACLE};

	// An array of sprite colors that are associated with each of the
	// possible states of a cell.
	private Color[] cellColors = new Color[7];

	// The current state of the cell.
	public State cellType;

	// The in and out directions of the cell (if it is a snake type).
	private Direction inDirection, outDirection;

	// Initializes the cellColors array and the cell.
	private void Start()
	{
		cellColors[0] = Color.green;
		cellColors[1] = Color.gray;
		cellColors[2] = Color.yellow;
		cellColors[3] = Color.red;
		cellColors[4] = Color.blue;
		cellColors[5] = Color.cyan;
		cellColors[6] = Color.black;
		SetCell(cellType);
	}

	// Sets the type of the cell and applies its associated color to the
	// cell sprite.
	public void SetCell(State cellType)
	{
		this.cellType = cellType;
		GetComponent<SpriteRenderer>().color = cellColors[(int)cellType];
	}

	// This variant of SetCell() allows the snake in/out direction to be set as
	// well.
	public void SetCell(State cellType, Direction inDirection,
						Direction outDirection)
	{
		this.cellType = cellType;
		this.inDirection = inDirection;
		this.outDirection = outDirection;
		GetComponent<SpriteRenderer>().color = cellColors[(int)cellType];
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
