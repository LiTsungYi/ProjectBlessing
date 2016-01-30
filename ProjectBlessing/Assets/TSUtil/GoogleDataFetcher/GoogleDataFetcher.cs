using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoogleDataFetcher
{
	static public void FetchGoogleSpreadSheet(string appUrl, string spreadsheetUrl, string sheetName, bool isColKey, Action<string> callback, MonoBehaviour mono)
	{
		string connectUrl = appUrl 
			+ "?sheet=" + sheetName
			+ "&isColKey=" + isColKey.ToString().ToLower()
			+ "&url=" + spreadsheetUrl;	// url last

		Debug.Log("connect to " + connectUrl );

		WWW www = new WWW(connectUrl);

		Action resultAct = ()=>
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError("fetch error: " + www.error);
			}
			else
			{
				Debug.Log("fetch done: " + www.text);
				if(null != callback)
				{
					callback(www.text); // json
				}
			}
		};

#if UNITY_EDITOR
		ContinuationManager.Add(()=> www.isDone, resultAct);
#else
		mono.StartCoroutine(WaitFetchGoogleData(www, resultAct));
#endif
	}

	static IEnumerator WaitFetchGoogleData(WWW www, Action callback)
	{
		yield return www;

		if(null != callback) 
		{
			callback();
		}
	}
	
	static public void SaveToPrefab(GameObject obj)
	{
#if UNITY_EDITOR
		if( null != PrefabUtility.GetPrefabParent(obj) )
		{// have prefab parent
			PrefabUtility.ReplacePrefab(obj, PrefabUtility.GetPrefabParent(obj));
			Debug.Log("Replace prefab done:"+ obj.name);
		}

		EditorUtility.SetDirty(obj);
#endif
	}
}



