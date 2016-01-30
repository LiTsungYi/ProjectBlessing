using UnityEngine;
using System.Collections;

public class TSBaseBehaviour : MonoBehaviour 
{
	protected Transform _Transform;		// TODO: Remove at Unity 5
	protected GameObject _GameObject;	

	public Transform cacheTrans
	{
		get
		{
			if(null == _Transform) _Transform = transform;
			return _Transform;
		}
	}

	public GameObject cacheGameObj
	{
		get
		{
			if(null == _GameObject) _GameObject = gameObject;
			return _GameObject;
		}
	}
}
