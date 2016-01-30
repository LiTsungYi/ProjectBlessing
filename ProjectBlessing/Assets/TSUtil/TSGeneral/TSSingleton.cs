using UnityEngine;
using System.Collections;

// TODO: Use for Tool Back To Default
/// <summary>
/// Singleton.
/// </summary>
/// Dont Destroy
public class TSSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	private static object _lock = new Object();
	
	public static T Instance
	{
		get
		{
			if (applicationIsQuitting && Application.isPlaying)		// for editor
			{
				Debug.LogWarning("[Singleton] Instance '"+ typeof(T) + 
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
//				return null;
			}
			lock(_lock)
			{
				if (!_instance) //Thread safe way of null comparison
				{
					_instance = (T) FindObjectOfType(typeof(T));

					if (!_instance)
					{
						GameObject container = new GameObject();
						container.name = typeof(T).ToString() + "(Singleton!)";
						_instance = (T) container.AddComponent(typeof(T));
						DontDestroyOnLoad(container);
					}
				}
				
				return _instance;
			}
		}
	}
	
	public static bool applicationIsQuitting = false;
	
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits
	/// If any script calls Instance after it have been destroyed,
	/// it will create a buggy ghost object that will stay on the Editor scene
	/// even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object
	/// </summary>
	public void OnDestroy()
	{
		applicationIsQuitting = true;	
	}
}
