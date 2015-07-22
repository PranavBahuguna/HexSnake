using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelName : MonoBehaviour {

	public SlideShow levelSlideShow;
	public GameManager GM;
	private string levelName;

	public void SetLevelName()
	{
		levelName = levelSlideShow.origSlides[levelSlideShow.GetSlideIndex()].name;
	}

	public void SetTextToLevelName(Text displayText)
	{
		displayText.text = levelName;
	}

	public void ConfirmLevel()
	{
		GM.SetLevelName(levelName);
	}
}
