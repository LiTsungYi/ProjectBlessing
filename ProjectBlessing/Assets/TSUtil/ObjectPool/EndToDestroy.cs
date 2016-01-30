using UnityEngine;
using System.Collections;

public class EndToDestroy : MonoBehaviour 
{
	void OnDisable()
	{
		Destroy(gameObject);
	}
}
