using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

[CustomEditor (typeof (InverseKinematics2D))]
public class InverseKinematics2DEditor : Editor 
{
	const float kPositionHandleSize = 0.25f;

	InverseKinematics2D component {
		get { return target as InverseKinematics2D; }
	}

	static bool jointsFolded;
	AnimBool showJoints;

	void OnEnable()
	{
		jointsFolded = EditorPrefs.GetBool("InverseKinematics2DEditor.JointsFolded", true);
		showJoints = new AnimBool (jointsFolded);
		showJoints.valueChanged = new UnityEvent ();
		showJoints.valueChanged.AddListener (Repaint);
	}

	void OnDisable()
	{
		EditorPrefs.SetBool ("InverseKinematics2DEditor.JointsFolded", jointsFolded);
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var endTransform = serializedObject.FindProperty ("endTransform");
		var targetTransform = serializedObject.FindProperty ("target");
		var localTarget = serializedObject.FindProperty ("localTarget");
		var overrideAnimation = serializedObject.FindProperty ("overrideAnimation");
		
		if (endTransform.objectReferenceValue != null)
			Validate (endTransform.objectReferenceValue as Transform);

		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (endTransform, new GUIContent ("End Transform", "Child transform that is the end of inverse kinematics chain."));
		if (EditorGUI.EndChangeCheck())
		{
			component.angleLimits.Clear ();
			if (endTransform.objectReferenceValue != null)
				localTarget.vector2Value = (endTransform.objectReferenceValue as Transform).position;
			
			serializedObject.SetIsDifferentCacheDirty();
		}

		if (targetTransform.objectReferenceValue != null)
			localTarget.vector2Value = (targetTransform.objectReferenceValue as Transform).position;


		EditorGUI.BeginDisabledGroup (endTransform.objectReferenceValue == null);
		EditorGUILayout.PropertyField (targetTransform, new GUIContent ("Target Override", "Overrides Target Position with Transform"));
		EditorGUILayout.PropertyField (overrideAnimation, new GUIContent ("Override Animation", "Use inverse kinematics solving for joints even if they are animated."));
		EditorGUI.EndDisabledGroup ();

		EditorGUI.BeginDisabledGroup (endTransform.objectReferenceValue == null || component.angleLimits == null || component.angleLimits.Count == 0);

		jointsFolded = EditorGUILayout.Foldout (jointsFolded, "Joints");

		showJoints.target = jointsFolded;
		if (EditorGUILayout.BeginFadeGroup(showJoints.faded))
		{
			foreach (var angleLimit in component.angleLimits)
			{
				EditorGUI.BeginChangeCheck();

				EditorGUILayout.LabelField(angleLimit.Transform.name);
				var min = EditorGUILayout.FloatField("Min Angle", angleLimit.min);
				var max = EditorGUILayout.FloatField("Max Angle", angleLimit.max);
				var damping = EditorGUILayout.Slider ("Damping", angleLimit.damping, 0, 1);

				if (EditorGUI.EndChangeCheck())
				{
					angleLimit.min = min;
					angleLimit.max = max;
					angleLimit.damping = damping;
				}
			}
		}
		EditorGUILayout.EndFadeGroup ();

		EditorGUI.EndDisabledGroup();

		serializedObject.ApplyModifiedProperties();
	}

	void Validate (Transform endTransform)
	{
		if (endTransform)
			while (true)
			{
				endTransform = endTransform.parent;

				bool isMissing = true;
				foreach (var angleLimit in component.angleLimits.Where (angleLimit => angleLimit.Transform == endTransform))
					isMissing = false;

				if (isMissing)
					component.angleLimits.Add (new InverseKinematics2D.Node { Transform = endTransform, min = 0, max = 360 });

				if (endTransform == component.transform)
					break;
			}

		// min & max has to be between 0 ... 360
		foreach (var node in component.angleLimits)
		{
			node.min = Mathf.Clamp (node.min, 0, 360);
			node.max = Mathf.Clamp (node.max, 0, 360);
		}
	}

	void OnSceneGUI()
	{
		DoAngleGizmo(component);
		DoIKHandle(component);
	}

	public static void DoIKHandle(InverseKinematics2D component)
	{
		if (!component.endTransform)
			return;

		EditorGUI.BeginChangeCheck();
//		var localTarget = Handles.DoPositionHandle (component.target ? component.target.position : component.localTarget, Quaternion.identity;
		var localTarget = Handles.FreeMoveHandle(
			component.target ? component.target.position : (Vector3) component.localTarget,
			Quaternion.identity,
			HandleUtility.GetHandleSize (component.localTarget) * kPositionHandleSize,
			Vector3.zero,
			Handles.CircleCap);

		if (EditorGUI.EndChangeCheck())
		{
			foreach (var joint in component.angleLimits)
				Undo.RecordObject (joint.Transform, "move posing target");
			Undo.RecordObject (component.endTransform, "move posing target");

			if (component.target)
				component.target.position = localTarget;
			else
				component.localTarget = localTarget;

			component.UpdateIK();
			SceneView.RepaintAll();
		}
	}

	static void DoAngleGizmo(InverseKinematics2D component)
	{
		foreach (var node in component.angleLimits)
		{
			if (node.Transform == null)
				continue;

			Transform transform = node.Transform;
			Vector3 position = transform.position;

			float handleSize = HandleUtility.GetHandleSize (position);
			float discSize = handleSize * kPositionHandleSize;


			float parentRotation = transform.parent ? transform.parent.eulerAngles.z : 0;
			Vector3 min = Quaternion.Euler (0, 0, node.min + parentRotation) * Vector3.down;
			Vector3 max = Quaternion.Euler (0, 0, node.max + parentRotation) * Vector3.down;

			var color = Handles.color;
			Handles.color = new Color (0, 1, 0, 0.1f);
			Handles.DrawWireDisc (position, Vector3.back, discSize);
			Handles.DrawSolidArc (position, Vector3.forward, min, node.max - node.min, discSize);

			Handles.color = Color.green;
			Handles.DrawLine (position, position + min * discSize);
			Handles.DrawLine (position, position + max * discSize);

			Vector3 toChild = FindChildNode (transform, component.endTransform).position - position;
			Handles.DrawLine (position, position + toChild);
			Handles.color = color;
		}
	}

	static Transform FindChildNode(Transform parent, Transform endTransform)
	{
		if (!endTransform || !endTransform.parent)
			return endTransform;

		if (endTransform.parent != parent)
			return FindChildNode (parent, endTransform.parent); ;

		return endTransform;
	}
}
