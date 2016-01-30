using UnityEngine;
using System.Collections;

public class TSRootBehaviour<T> : TSBaseBehaviour where T : TSBaseBehaviour
{
	private static T _instance;
	
	private static object _lock = new Object();
	
	public static T Instance
	{
		get
		{
			lock(_lock)
			{
				if (!_instance) //Thread safe way of null comparison
				{
					_instance = (T) FindObjectOfType(typeof(T));
				}
				
				return _instance;
			}
		}
	}
}
