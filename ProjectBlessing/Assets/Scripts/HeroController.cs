using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class HeroController : MonoBehaviour 
{
	public GameObject[] ritualObjs;
	public Action<HeroBodyBase> onBodyClick;
	
	public GameObject[] bodys;
	
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
	
	public void ShowLight(EnumRoleValueType roleType)
	{
		int idx = (int)roleType;
		var renders = bodys[idx].GetComponentsInChildren<SpriteRenderer>();
		for(int i = 0; i < renders.Length; i++)
		{
			DOTween.Complete(renders[i]);
			renders[i].DOColor(Color.white, 1f).From();
		}
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
