using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SetConnectedAnchors : MonoBehaviour {

	void Start ()
	{
		foreach (var hingeJoint in GetComponentsInChildren<HingeJoint2D> ())
		{
			if (hingeJoint.transform.parent.GetComponent<Rigidbody2D>())
			{
				hingeJoint.connectedBody = hingeJoint.transform.parent.GetComponent<Rigidbody2D>();
				hingeJoint.anchor = Vector2.zero;
				hingeJoint.connectedAnchor = hingeJoint.transform.parent.InverseTransformPoint(hingeJoint.transform.position);
			}
		}
	}
}
