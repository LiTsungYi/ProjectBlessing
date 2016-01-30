using UnityEngine;
using System.Collections;

public class RitualController : MonoBehaviour 
{
	public int ritualCount;
	
	public Transform canvasTrans;
	public GameObject uiBlessPrefab;
	[HideInInspector] public UIBlessController uiBlessCtrl;
	public HeroController heroSpriteCtrl;

	void Awake()
	{
		uiBlessCtrl = TSUtil.InstantiateForUGUI(uiBlessPrefab, canvasTrans).GetComponent<UIBlessController>();
		heroSpriteCtrl.Init(OnBodyClick);
	}
	
	void Start()
	{
		Init();
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
			App.Instance.AddRoleValue(App.Instance.monsterInfo, addType);	
		}
		catch
		{
			Debug.LogError("UpdateMonsterLevel Error");
		}
	}
	
	void ShowNowMosterInfo()
	{
		uiBlessCtrl.monsterViewCtrl.Show(App.Instance.monsterInfo);
	}
	
	void ShowNowHeroInfo()
	{
		uiBlessCtrl.heroViewCtrl.Show(App.Instance.heroInfo);
	}
	
	void OnBodyClick(HeroBodyBase bodyBase)
	{
		Debug.Log ("OnBodyClick: " + bodyBase.ToString());
		App.Instance.AddRoleValue(App.Instance.heroInfo, bodyBase.bodyType);
		ritualCount--;
		ShowNowHeroInfo();
		
		if(ritualCount <= 0)
		{
			Application.LoadLevel("ritual");
		}
	}
}
