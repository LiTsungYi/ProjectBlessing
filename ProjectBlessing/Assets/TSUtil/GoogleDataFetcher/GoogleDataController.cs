using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoogleDataController : MonoBehaviour 
{
	public string appUrl;
	public string spreadsheetUrl;
	public string backupPath;
	
	public IGoogleDataBase[] datas;

	void Awake()
	{
		datas = gameObject.GetComponentsForInterface<IGoogleDataBase>();
#if UNITY_EDITOR
		// Load All Backup
		LoadAllBackup();
#endif
	}

	public void GetDatas(bool[] types, Action complete = null)
	{
		int GetTypeCnt = 0;
		for(int i = 0; i < types.Length; i++ )
		{
			if(types[i]) GetTypeCnt++;
		}

		if( 0 == GetTypeCnt)
		{
			Debug.Log("GetDataByTypes No Select");
			return;
		}

		Debug.Log(string.Format("=========== GetDataByTypes Start GetTypeCnt[{0}] ===========", GetTypeCnt));

		for(int i = 0; i < types.Length; i++ )
		{
			if(types[i])
			{
				IGoogleDataBase inter = datas[i];

				if(null != inter)
				{
					inter.FetchData(this, ()=>{
						GetTypeCnt--;
						if(GetTypeCnt == 0)
						{
							GoogleDataFetcher.SaveToPrefab(gameObject);
							Debug.Log(string.Format("=========== GetDataByTypes End ==========="));
							if(null != complete)
							{
								complete();
							}
						}
					});
				}
			}
		}
	}

	public void GetAllDatas(Action complete = null)
	{
		bool[] flags = new bool[datas.Length];
		for( int i = 0; i < flags.Length; i++ )
		{
			flags[i] = true;
		}
		GetDatas(flags, complete);
	}

	public void FetchGoogleData<T>(string sheetName, bool isColKey, Action<List<T>> callback)
	{
		Debug.Log(string.Format("[{0}] is Begin fetch", typeof(T).ToString()));
		GoogleDataFetcher.FetchGoogleSpreadSheet(appUrl, spreadsheetUrl, sheetName, isColKey, (json)=>
		{
			Debug.Log(string.Format("[{0}] fetch done", typeof(T).ToString()));
			
			List<T> datas = null;
			if(JsonConvertGoogleData<T>(json, out datas))
			{
#if UNITY_EDITOR
				SaveBackupGoogleData(sheetName, json);
#endif
			}

			if(null != callback)
			{
				callback(datas);
			}
		}, this);
	}
	
	/// <summary>
	/// Jsons convert to google data.
	/// </summary>
	/// <returns><c>true</c>, if convert google data was jsoned, <c>false</c> otherwise.</returns>
	/// <param name="json">Json.</param>
	/// <param name="datas">Datas.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	private bool JsonConvertGoogleData<T>(string json, out List<T> datas)
	{
		datas = new List<T>();
		// JsonCovert
		try
		{
			datas = new List<T>(JsonConvert.DeserializeObject<T[]>(json));
		}
		catch(Exception ex)
		{
			Debug.LogError("FetchGoogleData JsonConvert error:"+ex);
			return false;
		}
		
		return true;
	}

#if UNITY_EDITOR
	private void SaveBackupGoogleData(string filename, string json)
	{
		string path = Application.dataPath + "/" + backupPath;
		string filePath = path + "/" + filename + ".json";
		
		try
		{
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			if(File.Exists(filePath))
			{
				File.Delete(filePath);
//				AssetDatabase.DeleteAsset(filePath);
				AssetDatabase.Refresh();
			}
			
			var file = File.CreateText(filePath);
			file.Write(json);
			file.Close();
			
			AssetDatabase.Refresh();
			Debug.Log("SaveBackupGoogleData Done: " + filename);
		}
		catch(Exception ex)
		{
			Debug.LogError("SaveBackupGoogleData ERROR: " + ex);
		}
	}

	public void LoadAllBackup(Action callback = null)
	{
		bool[] flags = new bool[datas.Length];
		for( int i = 0; i < flags.Length; i++ )
		{
			flags[i] = true;
		}
		LoadBackupDatas(flags, callback);
	}

	public void LoadBackupDatas(bool[] types, Action complete = null)
	{
		int LoadCnt = 0;
		for(int i = 0; i < types.Length; i++ )
		{
			if(types[i]) LoadCnt++;
		}
		
		if( 0 == LoadCnt)
		{
			Debug.Log("LoadBackupGooglData No Select");
			return;
		}
		
		Debug.Log(string.Format("=========== LoadBackupGooglData Start LoadCnt[{0}] ===========", LoadCnt));
		
		for(int i = 0; i < types.Length; i++ )
		{
			if(types[i])
			{
				IGoogleDataBase inter = datas[i];
				
				if(null != inter)
				{
					inter.LoadData(this, ()=>{
						LoadCnt--;
						if(LoadCnt == 0)
						{
							GoogleDataFetcher.SaveToPrefab(gameObject);
							Debug.Log(string.Format("=========== LoadBackupGooglData End ==========="));
							if(null != complete)
							{
								complete();
							}
						}
					});
				}
			}
		}
	}
	
	public void LoadBackupGooglData<T>(string sheetName, Action<List<T>> callback)
	{
		string path = Application.dataPath + "/" + backupPath;
		string filePath = path + "/" + sheetName + ".json";
		
		Debug.Log(string.Format("[{0}] is Begin LoadBackup", typeof(T).ToString()));
		try
		{
			if(!Directory.Exists(path))
			{
				Debug.LogError("LoadBackupGooglData Directory no exists: " + path);
				return;
			}
			
			if(!File.Exists(filePath))
			{
				Debug.LogError("LoadBackupGooglData File no exists: " + sheetName);
				return;
			}
			
			var json = File.ReadAllText(filePath);

			List<T> datas = null;
			if(!JsonConvertGoogleData<T>(json, out datas))
			{
				Debug.Log(string.Format("[{0}] LoadBackupGooglData fail", sheetName));
				return;
			}
			Debug.Log(string.Format("[{0}] LoadBackupGooglData success", sheetName));
			
			if(null != callback)
			{
				callback(datas);
			}
		}
		catch(Exception ex)
		{
			Debug.LogError("LoadBackupGooglData ERROR: " + ex);
			return;
		}
	}
#endif
}

