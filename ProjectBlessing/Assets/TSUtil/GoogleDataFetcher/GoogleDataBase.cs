using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GoogleDataBase<T> : MonoBehaviour, IGoogleDataBase where T : class
{
	public string _sheetName;
	public string sheetName
	{
		get
		{
			return _sheetName;
		}
	}
	
	public bool isColKey = true;

	public List<T> datas = new List<T>();
	
	public void FetchData(GoogleDataController ctrl, Action callback)
	{
		ctrl.FetchGoogleData<T>(sheetName, isColKey, (data)=>{
			datas = data;
			if(null != callback)
			{
				callback();
			}
		});
	}
	
	public void LoadData(GoogleDataController ctrl, Action callback)
	{
#if UNITY_EDITOR
		ctrl.LoadBackupGooglData<T>(sheetName, (data)=>{
			datas = data;
			if(null != callback)
			{
				callback();
			}
		});
#endif
	}
}

public interface IGoogleDataBase
{
	string sheetName
	{
		get;
	}
	
	void FetchData(GoogleDataController ctrl, Action callback);
	void LoadData(GoogleDataController ctrl, Action callback);
}


