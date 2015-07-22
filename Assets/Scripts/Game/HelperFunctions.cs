using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelperFunctions : MonoBehaviour 
{
	// Sets the text of a GUI element to the value given by a
	// GUI slider value.
	public void SetTextToValue(float value)
	{
		GetComponent<Text>().text = value.ToString();
	}
}
