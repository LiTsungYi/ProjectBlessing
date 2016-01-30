using UnityEngine;
using System.Collections;

public class RitualController : MonoBehaviour 
{
	public RoleViewController heroViewCtrl;
	public RoleViewController monsterViewCtrl;

	void Awake()
	{
		
	}
	
	void Start()
	{
		ShowNowMosterInfo();
		ShowNowHeroInfo();
	}
	
	void ShowNowMosterInfo()
	{
		monsterViewCtrl.Show(App.Instance.monsterInfo);
	}
	
	void ShowNowHeroInfo()
	{
		heroViewCtrl.Show(App.Instance.heroInfo);
	}
}
