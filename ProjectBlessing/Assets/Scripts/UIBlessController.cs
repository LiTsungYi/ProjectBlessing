using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBlessController : MonoBehaviour
{
	public Text monsterName;
	public RoleViewController heroViewCtrl;
	public RoleViewController monsterViewCtrl;
	
	public Text heroName;
	
	public Transform ritualTrans;
	public GameObject ritualIconPrefab;
	public GameObject[] ritualIcons;
	
	void Start()
	{
		ritualIcons = new GameObject[10];
		for(int i = 0; i < App.Instance.heroInfo.lv; i++)
		{
			ritualIcons[i] = TSUtil.InstantiateForUGUI(ritualIconPrefab, ritualTrans);
		}
	}
	
	public void ShowOffRitualIcon(int idx)
	{
		if(idx < 0 || idx >= ritualIcons.Length)
		{
			return;
		}
		
		ritualIcons[idx].SetActive(false);
	}
	
	public void ShowHeroName(string setname)
	{
		heroName.text = setname;
	}
	
	public void ShowMonsterName(string setname)
	{
		monsterName.text = setname;
	}
}
