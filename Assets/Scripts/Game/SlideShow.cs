using UnityEngine;
using System.Collections;

public class SlideShow : MonoBehaviour 
{
	public Transform[] slides;
	public float moveAmount;

	public void OnActive()
	{
		print ("hello");
	}

	public void ChangeSlide(bool right)
	{
		// If left, the slides will be moved in the opposite direction.
		// This is done by moving the slides by a negative amount.
		moveAmount = right ? moveAmount : -moveAmount;

		// Moves each slide horizontally by moveAmount.
		foreach (Transform slide in slides) {
			slide.Translate(new Vector3(moveAmount, 0));
		}
	}
}
