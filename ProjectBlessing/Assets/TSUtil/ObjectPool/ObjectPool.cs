using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour 
{
	public GameObject poolPrefab;
	public int poolAmount;
	public bool willGrow = true;
	public bool inPool = false;
	public bool worldPosStays = false;
	
	
	public List<GameObject> objPool = new List<GameObject>();

	void Awake()
	{
		if( poolPrefab == null )	Debug.LogError( "ObjectPoolScript ObjectPrefab null " );

		for( int i = 0; i < poolAmount; i++ )
		{
			CreateObject().SetActive(false);
		}
	}

	void OnDestroy()
	{
		for( int i = 0; i < objPool.Count; i++ )
		{
			if( objPool[i] != null )
			{
				if( !objPool[i].activeSelf ) 
				{
					Destroy(objPool[i]);
				}
				else
				{
					objPool[i].AddComponent<EndToDestroy>();
				}
			}
		}
	}
	
	public GameObject CreateObject(bool isAddPool = true)
	{
		GameObject Obj = Instantiate(poolPrefab) as GameObject;
		
		if(isAddPool)
		{
			objPool.Add(Obj);
		}
			
		if(inPool)
		{
			Obj.transform.SetParent(transform, worldPosStays);
		}
		return Obj;
	}

	public GameObject GetPoolObject() 
	{
		for( int i = 0; i < objPool.Count; i++ )
		{
			if( objPool[i] == null )
			{
				objPool[i] = CreateObject(false);
				return objPool[i];
			}
			
			if( !objPool[i].activeSelf )
			{
				return objPool[i];
			}
		}

		if( willGrow )
		{
			return CreateObject();
		}

		return null;
	}
	
	public T GetPoolObject<T>() where T : class
	{
		for( int i = 0; i < objPool.Count; i++ )
		{
			if( objPool[i] == null )
			{
				objPool[i] = CreateObject(false);
				return objPool[i].GetComponent<T>();
			}
			
			if( !objPool[i].activeSelf )
			{
				return objPool[i].GetComponent<T>();
			}
		}
		
		if( willGrow )
		{
			return CreateObject().GetComponent<T>();
		}
		
		return null;
	}
}
