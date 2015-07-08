using UnityEngine;
using System.Collections;

public static class Extensions
{
	/* This method enables a deactivated gameobject to be found by
	 * searching its parent object for child objects that match the
	 * supplied name. */
	public static GameObject FindObject(GameObject parent, string name)
	{
		Transform[] transforms = parent.GetComponentsInChildren<Transform> (true);
		foreach(Transform t in transforms){
			if(t.name == name){
				return t.gameObject;
			}
		}
		return null;
	}

	/* This method allows code within Update() to be run by a fixed
	 * amount of time every cycle. Any code placed after this method
	 * will only execute after this returns true. */
	private static float currentTime = 0;
	public static bool TimestepComplete(float timestep)
	{
		if (currentTime < timestep) {
			currentTime += Time.deltaTime;
			return false;
		}
		currentTime = 0;	// Reset
		return true;
	}
}
