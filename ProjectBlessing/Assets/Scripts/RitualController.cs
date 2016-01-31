using UnityEngine;
using System.Collections;
using DG.Tweening;

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
	
	public SpriteRenderer blessLight;

	void Awake()
	{
		uiBlessCtrl = TSUtil.InstantiateForUGUI(uiBlessPrefab, canvasTrans).GetComponent<UIBlessController>();
		uiSwordTrans = TSUtil.InstantiateForUGUI(uiSwordPrefab, canvasTrans).GetComponent<RectTransform>();
		uiResultCtrl = TSUtil.InstantiateForUGUI(uiResultPrefab, canvasTrans).GetComponent<UIResultsController>();
		
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
		if(!App.Instance.isFirstPlay)
		{
			App.Instance.GetTheHonorName();
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
		App.Instance.audioCtrl.PlayBGM(EnumAudio.MENU, 0.0f, 0.5f);
		var sfxIndex = AudioController.GetRandom(EnumSfx.HeroTalk1, EnumSfx.HeroTalk3);
		App.Instance.audioCtrl.PlaySfx( sfxIndex );
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
		bool isBoss = (App.Instance.heroInfo.lv == 10) ? true : false;
		App.Instance.monsterInfo = App.Instance.CreateNewRoleInfo(EnumRoleType.MOSTER, isBoss);	//	create new monster
		
		//int[] monsterPoints = { 0, 2, 4, 6, 8, 12, 16, 24, 32, 48, 64 };
		//int[] monsterPoints = { 0, 2, 4, 6, 10, 15, 25, 40, 55, 75, 100 };
		int[] monsterPoints = { 0, 2, 4, 6, 10, 15, 20, 30, 45, 60, 80 };
		int mosterUpadteCnt = monsterPoints[App.Instance.heroInfo.lv];
		/*
		for(int i = 0; i <= App.Instance.heroInfo.lv; i++)
		{
			mosterUpadteCnt += i;
		}
		if(isBoss)
		{
			mosterUpadteCnt += 10;
		}
		*/
		
		Debug.LogWarning("UpdateMonsterLevel LV:" + mosterUpadteCnt);
		
		for(int i = 0; i < mosterUpadteCnt; i++)
		{
			UpdateMonsterLevel();
		}
		
		ShowNowMosterInfo();
		ShowNowHeroInfo();
		uiBlessCtrl.ShowHeroName(App.Instance.heroInfo.name);
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
		uiBlessCtrl.SetMonsertShadowCut(App.Instance.monsterInfo.id);
		uiBlessCtrl.monsterViewCtrl.Show(App.Instance.monsterInfo);
	}
	
	void ShowNowHeroInfo()
	{
		uiBlessCtrl.heroViewCtrl.Show(App.Instance.heroInfo);
	}
	
	void OnBodyClick(HeroBodyBase bodyBase)
	{
		if(ritualCount <= 0)
		{
			return;
		}
		
//		DOVirtual.Float(255f, blessLight.color.a, 1f, (a)=>{
//			var color = blessLight.color;
//			color.a = a;
//			blessLight.color = color;
//		});

		var color = blessLight.color;
		color.a = 1f;
		blessLight.DOColor(color, 1f).From();
		
		heroSpriteCtrl.ShowLight(bodyBase.bodyType);

	//		StartCoroutine(EffectLight());
		
		App.Instance.audioCtrl.PlaySfx( EnumSfx.SwardTap );
		App.Instance.audioCtrl.PlaySfx( EnumSfx.Blessing );
		Debug.Log ("OnBodyClick: " + bodyBase.ToString());
		App.Instance.AddRoleValue(App.Instance.heroInfo, bodyBase.bodyType);
		uiBlessCtrl.ShowOffRitualIcon(ritualCount-1);
		ritualCount--;
		ShowNowHeroInfo();
		
		if(ritualCount <= 0)
		{
			StartCoroutine(TSUtil.WaitForSeconds(1.2f, ()=>{
				Application.LoadLevel("game");
				}));
		}
	}
	
	IEnumerator EffectLight()
	{	
		var color = blessLight.color;
		color.a = 1f;
		
		while(color.a >= 0.4f)
		{
			color.a -= 0.1f;
			blessLight.color = color;
			Debug.Log("EffectLight: " + color.a);
			yield return new WaitForEndOfFrame();
		}
	}
}
