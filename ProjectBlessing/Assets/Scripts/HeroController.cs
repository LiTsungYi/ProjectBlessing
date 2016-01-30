using UnityEngine;
using System.Collections;
using System;

public class HeroController : MonoBehaviour 
{
	public GameObject[] ritualObjs;
	public Action<HeroBodyBase> onBodyClick;
	
	void Awake()
	{
		for(int i = 0; i < ritualObjs.Length; i++)
		{
			EventTriggerListener.Get(ritualObjs[i]).onClick = OnBodyClick;
		}
	}
	
	public void Init(Action<HeroBodyBase> callback)
	{
		onBodyClick = callback;
	}
	
	void OnBodyClick(GameObject obj)
	{
		var bodyBase = obj.GetComponent<HeroBodyBase>();
		if(null != onBodyClick)
		{
			onBodyClick(bodyBase);
		}
	}
}
