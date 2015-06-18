using UnityEngine;
using System.Collections;

/* Defines the possible directions of the snake grid and provides
 * additional helper methods. */
public enum Direction {UP, UP_RIGHT, DOWN_RIGHT, DOWN, DOWN_LEFT, UP_LEFT}
public static class Extensions
{
	// Returns the direction opposite to the one given as argument.
	public static int Opposite(this Direction direction)
	{
		return ((int)direction + 3) % 6; 
	}

	// Determines the horizontal displacement given by moving in the given direction
	// by one unit.
	public static int DeltaX(this Direction direction)
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

	// Determines the vertical displacement given by moving in the given direction
	// by one unit. This takes an additional bool 'isColUpper' (whether a column of
	// cells 'looks' higher than its neighbouring columns).
	public static int DeltaY(this Direction direction, bool isColUpper)
	{
		switch (direction) {
			case (Direction.UP): 			return 1;
			case (Direction.UP_RIGHT):		return (isColUpper) ? 1 : 0;
			case (Direction.DOWN_RIGHT):	return (isColUpper) ? -1 : 0;
			case (Direction.DOWN):			return 1;
			case (Direction.DOWN_LEFT):		return (isColUpper) ? -1 : 0;
			case (Direction.UP_LEFT):		return (isColUpper) ? 1 : 0;
			default:						return 0;
		}
	}
}