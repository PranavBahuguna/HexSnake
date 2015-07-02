using UnityEngine;
using System.Collections;
using UnityEditor;

/* Custom editor for the Cell object - its primary function is to
 * automatically alter the colour of a cell in the editor window
 * depending on the state of the cell that is set in the inspector. */
[CustomEditor(typeof(Cell))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class CellEditor : Editor
{
	public SerializedProperty cellType_Prop;

	// Setup the SerializedProperties
	private void OnEnable()
	{
		cellType_Prop = serializedObject.FindProperty("cellType");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		EditorGUILayout.PropertyField(cellType_Prop);
		foreach (GameObject obj in Selection.gameObjects) {
			obj.GetComponent<Cell>().SetCell((Cell.State)cellType_Prop.intValue);
		}
		serializedObject.ApplyModifiedProperties ();
	}
}