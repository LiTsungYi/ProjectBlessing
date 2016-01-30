using UnityEngine;
using System.Collections;

public class RitualController : MonoBehaviour 
{
	public int ritualCount;
	
	public RoleViewController heroViewCtrl;
	public RoleViewController monsterViewCtrl;
	public HeroController heroSpriteCtrl;

	void Awake()
	{
		heroSpriteCtrl.Init(OnBodyClick);
	}
	
	void Start()
	{
		Init();
	}
	
	void Update()
	{
		if(ritualCount <= 0)
		{
			Application.LoadLevel("game");
		}
	}
	
	void Init()
	{
		App.Instance.heroInfo.lv++;	// hero levelup
		ritualCount = App.Instance.heroInfo.lv;
		App.Instance.monsterInfo = App.Instance.CreateNewRoleInfo(EnumRoleType.MOSTER);	//	create new monster
		
		for(int i = 0; i < App.Instance.heroInfo.lv; i++)
		{
			UpdateMonsterLevel();
		}
		
		ShowNowMosterInfo();
		ShowNowHeroInfo();
		
	}
	
	[ContextMenu("UpdateMonsterLevel")]
	void UpdateMonsterLevel()
	{
		Debug.Log("UpdateMonsterLevel");
		int ran = Random.Range(0, (int)EnumRoleValueType.max);
		
		try
		{
			EnumRoleValueType addType = (EnumRoleValueType)System.Enum.Parse(typeof(EnumRoleValueType), ran.ToString());
			App.Instance.AddRoleValue(App.Instance.monsterInfo, App.Instance.gameSettingManager.mosterLevelUpSetting, addType);	
		}
		catch
		{
			Debug.LogError("UpdateMonsterLevel Error");
		}
	}
	
	void ShowNowMosterInfo()
	{
		monsterViewCtrl.Show(App.Instance.monsterInfo);
	}
	
	void ShowNowHeroInfo()
	{
		heroViewCtrl.Show(App.Instance.heroInfo);
	}
	
	void OnBodyClick(HeroBodyBase bodyBase)
	{
		Debug.Log ("OnBodyClick: " + bodyBase.ToString());
		App.Instance.AddRoleValue(App.Instance.heroInfo, App.Instance.gameSettingManager.ritualSetting, bodyBase.bodyType);
		ritualCount--;
	}
}
