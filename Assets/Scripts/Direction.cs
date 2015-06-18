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
}