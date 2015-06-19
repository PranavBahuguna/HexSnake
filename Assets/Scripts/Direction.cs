using UnityEngine;
using System.Collections;

/* Defines the possible directions of the snake grid and provides
 * additional helper methods. */
public enum Direction {UP, UP_RIGHT, DOWN_RIGHT, DOWN, DOWN_LEFT, UP_LEFT}

public static class DirectionExtensions
{
	// A 2D array of ints that when casted to Direction, give a direction based
	// on the keys pressed and the current direction given.
	public static int[][] directionFromKeysArray = {
		new []{0, 0, 0, 3, 0, 0},
		new []{1, 1, 1, 1, 4, 1},
		new []{1, 1, 2, 2, 2, 1},
		new []{2, 2, 2, 2, 2, 1},
		new []{0, 3, 3, 3, 3, 3},
		new []{4, 1, 4, 4, 4, 4},
		new []{5, 5, 4, 4, 4, 5},
		new []{5, 5, 2, 5, 5, 5}
	};

	// Returns the direction opposite to the one given as argument.
	public static Direction Opposite(Direction direction)
	{
		return (Direction)((int)(direction + 3) % 6);
	}

	// Determines the horizontal displacement given by moving in the given
	// direction by one unit.
	public static int DeltaX(Direction direction)
	{
		switch (direction) {
			case (Direction.UP): 			return 0;
			case (Direction.UP_RIGHT):		return 1;
			case (Direction.DOWN_RIGHT):	return 1;
			case (Direction.DOWN):			return 0;
			case (Direction.DOWN_LEFT):		return -1;
			case (Direction.UP_LEFT):		return -1;
			default:						return 0;
		}
	}

	// Determines the vertical displacement given by moving in the given
	// direction by one unit. This takes an additional bool 'isColUpper'
	// (whether a column of cells 'looks' higher than its neighbouring columns).
	public static int DeltaY(Direction direction, bool isColUpper)
	{
		switch (direction) {
			case (Direction.UP): 			return 1;
			case (Direction.UP_RIGHT):		return (isColUpper) ? 0 : 1;
			case (Direction.DOWN_RIGHT):	return (isColUpper) ? -1 : 0;
			case (Direction.DOWN):			return -1;
			case (Direction.DOWN_LEFT):		return (isColUpper) ? -1 : 0;
			case (Direction.UP_LEFT):		return (isColUpper) ? 0 : 1;
			default:						return 0;
		}
	}

	// Finds a direction, given the keys pressed and the current direction
	public static Direction GetDirection(Direction direction) {

		// Sets the keysIndex from 0 to 7 based on the keys pressed - it starts
		// from the up key and increments in a clockwise manner.
		int keysIndex = 0;
		bool up 	= Input.GetKey(KeyCode.UpArrow);
		bool right	= Input.GetKey(KeyCode.RightArrow);
		bool down	= Input.GetKey(KeyCode.RightArrow);
		bool left	= Input.GetKey(KeyCode.RightArrow);

		if (up && !right && !down && !left) 			{ keysIndex = 0; }
			else if (up && right && !down && !left) 	{ keysIndex = 1; }
			else if (!up && right && !down && !left) 	{ keysIndex = 2; }
			else if (!up && right && down && !left) 	{ keysIndex = 3; }
			else if (!up && !right && down && !left) 	{ keysIndex = 4; }
			else if (!up && !right && down && left) 	{ keysIndex = 5; }
			else if (!up && !right && !down && left) 	{ keysIndex = 6; }
			else if (up && !right && !down && left) 	{ keysIndex = 7;
		}

		// Returns the new direction, or if none of the keys were pressed, the 
		// same direction as the one passed as argument.
		if (up || right || down || left) {
			return (Direction)directionFromKeysArray [keysIndex] [(int)direction];
		} else {
			return direction;
		}
	}
}
