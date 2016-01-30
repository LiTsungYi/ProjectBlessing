using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GoogleDataBaseDicString : MonoBehaviour, IGoogleDataBase
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
	
	public List<string> keys = new List<string>();
	[SerializeField]
	public List<GoogleDataBaseDicValue> valueGroups = new List<GoogleDataBaseDicValue>();

	private List<Dictionary<string,string>> _Datas = null;
	public List<Dictionary<string,string>> datas
	{
		get
		{
			if(_Datas == null)
			{
				_Datas = new List<Dictionary<string, string>>();
				for(int i = 0; i < valueGroups.Count; i++)
				{
					Dictionary<string,string> dic = new Dictionary<string,string>();
					for(int j = 0; j < keys.Count; j++)
					{
						dic.Add(keys[j], valueGroups[i].values[j]);
					}
					datas.Add(dic);
				}
			}
			return _Datas;
		}
	}
	
	public void FetchData(GoogleDataController ctrl, Action callback)
	{
		ctrl.FetchGoogleData<Dictionary<string, string>>(sheetName, isColKey, (data)=>{
			keys.Clear();
			valueGroups.Clear();
			for(int i = 0; i < data.Count; i++)
			{
				if(i == 0)
				{
					keys.AddRange(data[i].Keys);
				}
				
				GoogleDataBaseDicValue va = new GoogleDataBaseDicValue();
				foreach( var str in data[i].Values)
				{
					va.values.Add(str);
				}
				valueGroups.Add(va);
			}
			if(null != callback)
			{
				callback();
			}
		});
	}
	
	public void LoadData(GoogleDataController ctrl, Action callback)
	{
#if UNITY_EDITOR
		ctrl.LoadBackupGooglData<Dictionary<string, string>>(sheetName, (data)=>{
			keys.Clear();
			valueGroups.Clear();
			for(int i = 0; i < data.Count; i++)
			{
				if(i == 0)
				{
					keys.AddRange(data[i].Keys);
				}
				
				GoogleDataBaseDicValue va = new GoogleDataBaseDicValue();
				foreach( var str in data[i].Values)
				{
					va.values.Add(str);
				}
				valueGroups.Add(va);
			}
			if(null != callback)
			{
				callback();
			}
		});
#endif
	}
}

[Serializable]
public class GoogleDataBaseDicValue
{
	public List<string> values = new List<string>();
}

