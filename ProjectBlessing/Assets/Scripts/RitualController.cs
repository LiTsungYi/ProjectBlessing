using UnityEngine;
using System.Collections;

public class RitualController : MonoBehaviour 
{
	public int ritualCount;
	public Camera uiCamera;
	
	public Transform canvasTrans;
	public GameObject uiBlessPrefab;
	[HideInInspector] public UIBlessController uiBlessCtrl;
	public HeroController heroSpriteCtrl;
	
	public GameObject uiResultPrefab;
	[HideInInspector] public UIResultsController uiResultCtrl;
	
	public GameObject uiSwordPrefab;
	[HideInInspector] public RectTransform uiSwordTrans;

	void Awake()
	{
		uiBlessCtrl = TSUtil.InstantiateForUGUI(uiBlessPrefab, canvasTrans).GetComponent<UIBlessController>();
		uiResultCtrl = TSUtil.InstantiateForUGUI(uiResultPrefab, canvasTrans).GetComponent<UIResultsController>();
		uiSwordTrans = TSUtil.InstantiateForUGUI(uiSwordPrefab, canvasTrans).GetComponent<RectTransform>();
		
		heroSpriteCtrl.Init(OnBodyClick);
	}
	
	void Update()
	{
		Vector3 point = Input.mousePosition;
		point.z = 10;
		uiSwordTrans.position = uiCamera.ScreenToWorldPoint(point);
	}
	
	void Start()
	{
		Init();
	}
	
	void Init()
	{
		App.Instance.audioCtrl.PlayBGM(EnumAudio.MENU, 0.0f, 0.5f);
		if(!App.Instance.isFirstPlay)
		{
			uiResultCtrl.Show(App.Instance.isWin, ()=>{
				if(!App.Instance.isWin)
				{
					Application.LoadLevel("main");
				}
				else
				{	
					uiResultCtrl.gameObject.SetActive(false);
					RitualStart();
				}
			});
		}
		else
		{
			App.Instance.isFirstPlay = false;
			App.Instance.SetDefaultName();
			uiResultCtrl.gameObject.SetActive(false);
			RitualStart();
		}
	}
	
	void RitualStart()
	{
		App.Instance.heroInfo.lv++;	// hero levelup
		if(App.Instance.heroInfo.lv > 10)
		{
			Debug.Log("GAME OVER");
			uiResultCtrl.ShowGameover();
			return;
		}
		uiBlessCtrl.Init();
		App.Instance.monsterInfo.lv = App.Instance.heroInfo.lv;
		ritualCount = App.Instance.heroInfo.lv;
		App.Instance.monsterInfo = App.Instance.CreateNewRoleInfo(EnumRoleType.MOSTER);	//	create new monster
		
		int mosterUpadteCnt = 0;
		for(int i = 0; i <= App.Instance.heroInfo.lv; i++)
		{
			mosterUpadteCnt += i;
		}
		
		Debug.LogWarning("UpdateMonsterLevel LV:" + mosterUpadteCnt);
		
		for(int i = 0; i < mosterUpadteCnt; i++)
		{
			UpdateMonsterLevel();
		}
		
		ShowNowMosterInfo();
		ShowNowHeroInfo();
		uiBlessCtrl.ShowHeroName(App.Instance.GetTheHonorName());
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
		uiBlessCtrl.ShowMonsterName(App.Instance.monsterInfo.id);
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
		uiBlessCtrl.ShowOffRitualIcon(ritualCount-1);
		ritualCount--;
		ShowNowHeroInfo();
		
		
		if(ritualCount <= 0)
		{
			Application.LoadLevel("game");
		}
	}
}
