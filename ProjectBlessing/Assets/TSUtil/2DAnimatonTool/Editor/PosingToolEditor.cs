using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PosingTool))]
public class PosingToolEditor : Editor
{
	const float kRotationHandleSize = 0.1f;
	const float kPositionHandleSize = 0.25f;

	PosingTool component
	{
		get { return target as PosingTool; }
	}

	void OnSceneGUI()
	{
		//Handles.color = Color.red;
		foreach (var joint in component.joints)
			DoHandle (joint);

		foreach (var inverseKinematics2D in component.GetComponentsInChildren<InverseKinematics2D>())
			InverseKinematics2DEditor.DoIKHandle (inverseKinematics2D);
	}

	void DoHandle(Transform transform)
	{
		if (transform == null)
			return;

		EditorGUI.BeginChangeCheck();

		var handleSize = HandleUtility.GetHandleSize(transform.position);

		var direction = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z, Vector3.forward) * Vector3.up;
		var rotationHandlePosition = transform.position + (handleSize * (kPositionHandleSize + kRotationHandleSize)) * direction.normalized;
		Handles.DrawAAPolyLine (new[] { transform.position, rotationHandlePosition });
		var newRotation = Handles.FreeMoveHandle (
							rotationHandlePosition,
							Quaternion.identity,
							handleSize * kRotationHandleSize,
							Vector3.zero,
							Handles.CircleCap);

		var newPosition = Handles.FreeMoveHandle (
						transform.position,
						Quaternion.identity,
						handleSize * kPositionHandleSize,
						Vector3.zero,
						Handles.CircleCap);

		if (EditorGUI.EndChangeCheck ())
		{
			Undo.RecordObject(transform, "move posing target");

			transform.localRotation = Quaternion.LookRotation (Vector3.forward, newRotation - transform.position);
			transform.position = newPosition;
			SceneView.RepaintAll ();

			foreach (var inverseKinematics2D in component.GetComponentsInChildren<InverseKinematics2D>())
			{
				foreach (var joint in inverseKinematics2D.angleLimits)
					Undo.RecordObject (joint.Transform, "move posing target");
				Undo.RecordObject (inverseKinematics2D.endTransform, "move posing target");

				inverseKinematics2D.UpdateIK();
			}
		}
	}
}
