using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelName : MonoBehaviour {

	public SlideShow levelSlideShow;
	public GameManager GM;

	public void SetLevelName()
	{
		GetComponent<Text>().text = 
			levelSlideShow.origSlides[levelSlideShow.GetSlideIndex()].name;
	}

	public void ConfirmLevel()
	{
		GM.SetLevelName(GetComponent<Text>().text);
	}
}
