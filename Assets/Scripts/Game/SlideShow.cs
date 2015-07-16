using UnityEngine;
using System.Collections;

public class SlideShow : MonoBehaviour 
{
	public Transform[] origSlides;		// Slides to be used.
	private Transform[] slides;

	// Transform components of each slide.
	public Vector3 slidePos;
	public Quaternion slideRotation;
	public Vector3 slideScale;

	public float moveAmount;	// Spacing between each slide.
	private int slideIndex = 0;	// The current index of slideshow.

	// Sets up the slideshow.
	public void Initialise()
	{
		// Places each slide in the appropriate transform and position,
		// and adds them to the slides[] array.
		slides = new Transform[origSlides.Length];
		for (int i = 0; i < slides.Length; i++) {
			Transform slide = Instantiate(origSlides[i]);
			slide.gameObject.SetActive(true);
			slide.position = new Vector3(
				slidePos.x + moveAmount * i,
              	slidePos.y,
            	slidePos.z);
			slide.rotation = slideRotation;
			slide.localScale = slideScale;
			slides[i] = slide;
		}
	}

	public void ChangeSlide(bool right)
	{
		// Only allows movement if the new slideIndex does not exceed
		// the bounds of the slideshow.
		if (right && slideIndex < slides.Length - 1 ||
			!right && slideIndex > 0) {

			// slideIndex is decremented if moving left, incremented if right.
			slideIndex += right ? 1 : -1;

			// If left, the slides will be moved in the opposite direction.
			// This is done by moving the slides by a negative amount.
			float currentMoveAmount = right ? -moveAmount : moveAmount;

			// Moves each slide horizontally by moveAmount.
			foreach (Transform slide in slides) {
				slide.Translate (new Vector3 (currentMoveAmount, 0));
			}
		}
	}

	// Destroys the slides and resets slideIndex.
	public void Close()
	{
		slideIndex = 0;
		foreach (Transform slide in slides) {
			Destroy(slide.gameObject);
		}
	}

	// Getter method for slideIndex.
	public int GetSlideIndex()
	{
		return slideIndex;
	}
}
