using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PosingTool : MonoBehaviour
{
	public List<Transform> joints = new List<Transform>();
}
