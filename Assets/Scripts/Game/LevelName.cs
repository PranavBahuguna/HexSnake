using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* Singleton class that obtains the name of the currently listed level. It also
 * contains methods to set UI text and the GM's level to levelName. */
public class LevelName : MonoBehaviour {

	public SlideShow levelSlideShow;
	public GameManager GM;
	private string levelName;

	// Obtains the name of the current level from the slideshow.
	public void SetLevelName()
	{
		levelName = levelSlideShow.origSlides[levelSlideShow.GetSlideIndex()].name;
	}

	// Sets UI text to levelName.
	public void SetTextToLevelName(Text displayText)
	{
		displayText.text = levelName;
	}

	// Sets the actual level to be played to the one whose name is levelName.
	public void ConfirmLevel()
	{
		GM.SetLevelName(levelName);
	}
}
