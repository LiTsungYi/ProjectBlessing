using UnityEngine;
using System.Collections;
using UnityEditor;
using Newtonsoft.Json;
using System.Collections.Generic;

[CustomEditor(typeof(GoogleDataController))]
public class GoogleDataControllerInspector : Editor 
{
	protected bool[] getTypes;
	//private IGoogleDataBase[] datas;

	void OnEnable()
	{
		ResetDatas();
	}

	private void ResetDatas()
	{
		GoogleDataController ctrl = (GoogleDataController)target;
		ctrl.datas = ctrl.gameObject.GetComponentsForInterface<IGoogleDataBase>();
		getTypes = new bool[ctrl.datas.Length];
//		Debug.Log("ResetDatas");
	}

	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();

		GoogleDataController ctrl = (GoogleDataController)target;
		if(getTypes.Length != ctrl.datas.Length)
		{
			getTypes = new bool[ctrl.datas.Length];
		}

		for(int i = 0; i < ctrl.datas.Length; i++)
		{
			if(null == ctrl.datas[i] )
			{ 
				continue;
			}
			getTypes[i] = GUILayout.Toggle(getTypes[i], ctrl.datas[i].sheetName);
		}

		if(GUILayout.Button("Clean All Select"))
		{
			for(int i = 0; i < getTypes.Length; i++)
			{
				getTypes[i] = false;
			}
		}

		if(GUILayout.Button("Select All"))
		{
			for(int i = 0; i < getTypes.Length; i++)
			{
				getTypes[i] = true;
			}
		}

		if(GUILayout.Button("Fetch Spreadsheet"))
		{
			ctrl.GetDatas(getTypes);
		}
		
		if(GUILayout.Button("Load Backup Datas"))
		{
			ctrl.LoadBackupDatas(getTypes);
		}
	}
}
