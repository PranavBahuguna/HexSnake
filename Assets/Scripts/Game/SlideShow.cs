using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* This class allows a 'slideshow' of an array of transforms to be created. Like
 * a real slidehow, it can switch back and forth through slides, alter the
 * separation between them and the physical dimensions of the slides. The
 * original transforms however are not used; a copy of them is instantiated
 * instead. */
public class SlideShow : MonoBehaviour
{
	public Transform[] origSlides;	// Slides to be used.
	private Transform[] slides;		// A copy of origSlides.

	// The buttons used to change between the slides.
	public Button leftButton;
	public Button rightButton;

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

		// To reach the slide that it was previously on, ChangeSlide() is called
		// by slideIndex times.
		for (int i = 0; i < slideIndex; i++) {
			slideIndex -= 1;	// This is to prevent slideIndex increment.
			ChangeSlide(true);
		}

		SetSlideButtonsStates();
	}

	public void ChangeSlide(bool right)
	{
		// slideIndex is decremented if moving left, incremented if right.
		slideIndex += right ? 1 : -1;
		SetSlideButtonsStates();
		
		// If left, the slides will be moved in the opposite direction. This
		// is done by moving the slides by a negative amount.
		float currentMoveAmount = right ? -moveAmount : moveAmount;
		
		// Moves each slide horizontally by moveAmount.
		foreach (Transform slide in slides) {
			slide.Translate (new Vector3 (currentMoveAmount, 0));
		}
	}

	// Enables/disables the left/right buttons based on whether the
	// slideshow has reached its bounds.
	public void SetSlideButtonsStates()
	{
		if (slideIndex == slides.Length - 1) {
			rightButton.interactable = false;
		} else {
			rightButton.interactable = true;
		}
		if (slideIndex == 0) {
			leftButton.interactable = false;
		} else {
			leftButton.interactable = true;
		}
	}

	// Destroys the slides and resets slideIndex.
	public void Close()
	{
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
