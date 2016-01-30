using UnityEngine;
using System.Collections;
using UnityEditor;
	
[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		Transform t = (Transform)target;

		Vector3 position = t.localPosition;
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("P", GUILayout.Width(20)))
		{
			position = Vector3.zero;
		}
		else
		{
			position = EditorGUILayout.Vector3Field("Position", t.localPosition);
		}
		EditorGUILayout.EndHorizontal();


		Vector3 eulerAngles = t.localEulerAngles;
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("R", GUILayout.Width(20)))
		{
			eulerAngles = Vector3.zero;
		}
		else
		{
			eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
		}
		EditorGUILayout.EndHorizontal();

		Vector3 scale = t.localScale;
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("S", GUILayout.Width(20)))
		{
			scale = Vector3.one;
		}
		else
		{
			scale = EditorGUILayout.Vector3Field("Scale", t.localScale);
		}
		EditorGUILayout.EndHorizontal();

		
		if (GUI.changed)
		{
			Undo.RecordObject(t, "Transform Change");
			
			t.localPosition = FixIfNaN(position);
			t.localEulerAngles = FixIfNaN(eulerAngles);
			t.localScale = FixIfNaN(scale);
		}
	}
	
	private Vector3 FixIfNaN(Vector3 v)
	{
		if (float.IsNaN(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z))
		{
			v.z = 0;
		}
		return v;
	}
}