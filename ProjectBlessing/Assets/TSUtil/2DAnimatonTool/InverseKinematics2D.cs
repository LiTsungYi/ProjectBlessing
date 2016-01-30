using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class InverseKinematics2D : MonoBehaviour
{
	public bool overrideAnimation;
	public Transform endTransform;
	public Transform target;
	public Vector2 localTarget;

	public int iterations = 5;
		
	public List<Node> angleLimits = new List<Node>();

	Dictionary<Transform, Node> nodeCache; 
	[System.Serializable]
	public class Node
	{
		public Transform Transform;
		public float min;
		public float max;
		public float damping = 1;
	}

	void OnAwake()
	{
		localTarget = transform.position;
	}

	void Reset()
	{
		if (endTransform == null)
		{
			longestChainCount = -1;
			FindLongestChain (transform);
			endTransform = longestChainTransform;
			if (endTransform)
				localTarget = endTransform.position;
		}
	}
	
	int longestChainCount;
	Transform longestChainTransform;

	void FindLongestChain(Transform transform, int length = 0)
	{
		length++;
		foreach (Transform child in transform)
		{
			FindLongestChain (child, length);
		}

		if (length > longestChainCount)
		{
			longestChainCount = length;
			longestChainTransform = transform;
		}
	}

	void Start()
	{
		// Cache optimization
		nodeCache = new Dictionary<Transform, Node>(angleLimits.Count);
		foreach (var node in angleLimits)
			if (!nodeCache.ContainsKey(node.Transform))
				nodeCache.Add(node.Transform, node);
	}

	void Update()
	{
		if (!overrideAnimation)
			UpdateIK();
	}

	void LateUpdate()
	{
		if (overrideAnimation)
			UpdateIK();
	}

	public void UpdateIK()
	{
		if (!Application.isPlaying)
			Start ();

		if (endTransform == null)
			return;

		int i = 0;

		while (i < iterations)
		{
			CalculateIK ();
			i++;
		}

		endTransform.rotation = target ? target.rotation : Quaternion.identity;
	}

	void CalculateIK()
	{		
		Transform node = endTransform.parent;

		while (true)
		{
			RotateTowardsTarget (node);

			if (node == transform)
				break;

			node = node.parent;
		}
	}

	void RotateTowardsTarget(Transform transform)
	{		
		Vector2 toTarget = (target ? target.position : (Vector3)localTarget) - transform.position;
		Vector2 toEnd = endTransform.position - transform.position;

		// Calculate how much we should rotate to get to the target
		float angle = SignedAngle(toEnd, toTarget);

		// Flip sign if character is turned around
		angle *= Mathf.Sign(transform.root.localScale.x);

		// "Slows" down the IK solving
		if (nodeCache.ContainsKey (transform))
			angle *= nodeCache[transform].damping; 

		// Wanted angle for rotation
		angle = -(angle - transform.eulerAngles.z);

		// Take care of angle limits 
		if (nodeCache.ContainsKey(transform))
		{
			// Clamp angle in local space
			var node = nodeCache[transform];
			float parentRotation = transform.parent ? transform.parent.eulerAngles.z : 0;
			angle -= parentRotation;
			angle = ClampAngle(angle, node.min, node.max);
			angle += parentRotation;
		}

		var newRotation = Quaternion.Euler(0, 0, angle);

		transform.rotation = newRotation;
	}

	public static float SignedAngle (Vector3 a, Vector3 b)
	{
		float angle = Vector3.Angle (a, b);
		float sign = Mathf.Sign (Vector3.Dot (Vector3.back, Vector3.Cross (a, b)));

		return angle * sign;
	}

	float ClampAngle (float angle, float min, float max)
	{
		angle = Mathf.Abs((angle % 360) + 360) % 360;
		return Mathf.Clamp(angle, min, max);
	}
}
