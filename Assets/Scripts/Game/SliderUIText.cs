using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* This class contains a method that allows a slider to set the
 * text to a float value produced by the slider. */
public class SliderUIText : MonoBehaviour 
{
	public void SetTextToValue(float value)
	{
		GetComponent<Text>().text = value.ToString();
	}
}
