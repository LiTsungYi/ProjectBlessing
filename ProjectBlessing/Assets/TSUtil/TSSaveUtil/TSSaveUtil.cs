using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System;
//using Opus.Encodings;
using TeamSignal.Utilities.Encodings;

public class TSSaveUtil : Singleton<TSSaveUtil> 
{
//	private ICodec codec = null;

//	private JsonSerializerSettings jsonSerialSetting = new JsonSerializerSettings();

//	void Awake()
//	{
//		jsonSerialSetting.MissingMemberHandling = MissingMemberHandling.Ignore;		// ignore property
//	}
//	public void Initialize( ICodec encoder)
//	{
//		codec = encoder;
//	}
	
	public void SaveFile(object data, string PlayerPrefsKey)
	{
		Formatting formatting = Formatting.None;

#if UNITY_EDITOR
		formatting = Formatting.Indented;	// good to read
#endif
		try
		{
			string saveStr = JsonConvert.SerializeObject(data, formatting);
//			if ( null != codec )
//			{
//				string encodedStr = codec.Encode( saveStr );
//				PlayerPrefs.SetString(Md5.Sum(PlayerPrefsKey), encodedStr);
//				Debug.Log("SaveFile:" + Md5.Sum(PlayerPrefsKey) + " = " + encodedStr);
//			}
			PlayerPrefs.SetString(PlayerPrefsKey, saveStr);
			PlayerPrefs.Save();
			//		Debug.Log(string.Format("SaveFile Done[{0}]", PlayerPrefsKey));
			Debug.Log("SaveFile:" + saveStr);
		}
		catch
		{
			Debug.LogError(string.Format("SaveFile JsonConvert ERROR [{0}]", PlayerPrefsKey));
		}
	}

	public T LoadFile<T>(string PlayerPrefsKey) where T: class
	{
		string loadStr = PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);
//		if ( null != codec )
//		{
//			string encodedStr = PlayerPrefs.GetString(Md5.Sum(PlayerPrefsKey), string.Empty);
//			string decodedStr = codec.Decode( encodedStr );
//			if ( string.IsNullOrEmpty( decodedStr ) )
//			{
//				loadStr = decodedStr;
//			}
//		}
		Debug.Log("LoadFile:" + loadStr);

		if(loadStr.Equals(string.Empty))
		{
			Debug.Log(string.Format("LoadFile Error[{0}]", PlayerPrefsKey));
			return null;
		}

		try
		{
			//		Debug.Log(string.Format("LoadFile Done[{0}]", PlayerPrefsKey));
			//		Debug.Log("SaveFile:" + saveStr);
			T data = JsonConvert.DeserializeObject<T>(loadStr);
			return data;
		}
		catch
		{
			Debug.LogError(string.Format("LoadFile JsonConvert ERROR [{0}]", PlayerPrefsKey));
			return null;
		}
	}
	
	public byte[] ConvertStringToBytes(string str)
	{
		return System.Text.ASCIIEncoding.Default.GetBytes(str);
	}
	
	public string ConvertBytesToString(byte[] bytes)
	{
		return System.Text.ASCIIEncoding.Default.GetString(bytes);
	}
	
	public string JsonConvertSerializeObject(object data)
	{
		Formatting formatting = Formatting.None;
		
		#if UNITY_EDITOR
		formatting = Formatting.Indented;	// good to read
		#endif
	
		try
		{
			string saveStr = JsonConvert.SerializeObject(data, formatting);
			return saveStr;
		}
		catch
		{
			return string.Empty;
		}
	}
	
	public T JsonConvertDeserializeObject<T>(string strData) where T: class
	{
		try
		{
			T data = JsonConvert.DeserializeObject<T>(strData);
			return data;
		}
		catch
		{
			return null;
		}
	}
}
