using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
	public GameRules gameRule;
	public Camera theCamera;
	public Transform canvasTrans;

	public Role heroRole;
	public Role monsterRole;
	public Text titleText;

	private bool playAction = false;
	private int playIndex = 0;
	private IList<AttackResult> playResult = new List<AttackResult>();
	private float playDuration = 0.0f;
	private float leaveDuration = 0.0f;
	
	private StageState state = StageState.None;
	private bool entering = false;

	public GameObject uiTitlePrefab;
	
	[Header("MovingSetting")]
	public Vector3 camEndPos = new Vector3(9,0,-10);
	public Vector3 heroIntroPos = new Vector3(-8, -1.7f, 0);
	public Vector3 heroEndPos = new Vector3(5.4f, -1.7f, 0);
	public float introDuration = 3f;
	public Vector3 monsterIntroPos = new Vector3(22, -2.2f, 0);
	public Vector3 monsterEndPos = new Vector3(15, -2.2f, 0);
	public float bgMoveX = 3;
	private BgLoader bgloader;

	void Awake()
	{
		titleText = TSUtil.InstantiateForUGUI(uiTitlePrefab, canvasTrans).GetComponentInChildren<Text>();
		bgloader = TSUtil.Instantiate(Resources.Load<GameObject>("BgLoader")).GetComponent<BgLoader>();
	}

	void Start ()
	{
		// NOTE: set hero and monster here!
		var heroPosition = heroRole.gameObject.transform.position;
		heroRole.gameObject.transform.position = heroIntroPos;
		heroRole.CreateRole("HeroFight");
		heroRole.gameObject.SetActive( true );
		
		var monsterPosition = monsterRole.gameObject.transform.position;
		monsterRole.gameObject.transform.position = monsterIntroPos;
		monsterRole.CreateRole(App.Instance.monsterInfo.id);
		monsterRole.gameObject.SetActive( true );
		
		bgloader.SetX(-bgMoveX);
		
		SetState( StageState.Moving );
		titleText.text = App.Instance.heroInfo.name + "T";
		App.Instance.audioCtrl.PlayBGM( EnumAudio.ICE_FOREST, 0.0f, 0.2f );
	}
	
	void Update ()
	{
		switch ( state )
		{
			
		case StageState.Moving:
			Moving();
			break;
			
		case StageState.Meet:
			Meeting();
			break;
			
		case StageState.Fight:
			Fighting();
			break;
			
		case StageState.Final:
			Finally();
			break;
			
		default:
			break;
		}
	}

	void FixedUpdate()
	{
		if ( playAction )
		{
			playDuration += Time.fixedDeltaTime;
			var finish = PerformAction();

			if ( finish )
			{
				ResetPlay();
				SetState( StageState.Final );
			}
		}
	}

	#region Update

	private void Moving()
	{
		if ( entering )
		{
			theCamera.transform.DOMove( camEndPos, introDuration )
				.SetEase(Ease.InOutSine)
				.OnComplete( MoveEnd );
			heroRole.transform.DOMove( heroEndPos, introDuration )
				.SetEase(Ease.InOutSine);
			monsterRole.transform.DOMove( monsterEndPos, introDuration)
				.SetEase(Ease.InOutSine);
				
			bgloader.ShowMove(bgMoveX, introDuration);
			entering = false;
		}
	}
	
	private void MoveEnd()
	{
		SetState( StageState.Meet );
	}
	
	private void Meeting()
	{
		if ( entering )
		{
			SetState( StageState.Fight );
			App.Instance.audioCtrl.PlayBGM( EnumAudio.INGAME, 0.0f, 0.2f );
		}
	}
	
	private void Fighting()
	{
		if ( entering )
		{
			entering = false;
			Attack();
		}
	}
	
	private void Finally()
	{
		if ( entering )
		{
			entering = false;
			if ( App.Instance.isWin )
			{
				App.Instance.audioCtrl.PlaySfx( EnumSfx.MonsterDie );
				monsterRole.gameObject.SetActive( false );
			}
			else
			{
				App.Instance.audioCtrl.PlaySfx( EnumSfx.HeroDie );
				heroRole.gameObject.SetActive( false );
			}
			
			leaveDuration = Time.timeSinceLevelLoad + 2.5f;
		}

		if ( leaveDuration < Time.timeSinceLevelLoad )
		{
		    Application.LoadLevel("ritual");
		}
	}
	#endregion

	private void SetState( StageState newState )
	{
		state = newState;
		entering = true;
	}

	private void ResetPlay()
	{
		playIndex = 0;
		playResult = new List<AttackResult>();
		playAction = false;
		playDuration = 0.0f;
	}

	public void Attack()
	{
		var hero = new GameInfo( App.Instance.heroInfo );
		Debug.Log( string.Format( "Hero: {0}/{6}, Hp={1}, Atk={2}, Def={3}, Spd={4}, Avo={5}", 
			hero.Name, hero.HitPoint, hero.Attack, hero.Defence, hero.Speed, hero.Avoid, hero.Id ) );
		heroRole.gameInfo = hero;
		heroRole.hpText.text = string.Format( "{0}", hero.HitPoint );

	    var monster = new GameInfo( App.Instance.monsterInfo );
		Debug.Log( string.Format( "Monster: {0}/{6}, Hp={1}, Atk={2}, Def={3}, Spd={4}, Avo={5}", 
			monster.Name, monster.HitPoint, monster.Attack, monster.Defence, monster.Speed, monster.Avoid, monster.Id ) );
		monsterRole.gameInfo = monster;
		monsterRole.hpText.text = string.Format( "{0}", monster.HitPoint );

		gameRule = new GameRules( hero, monster );
		playResult = gameRule.Attack();
		
		playAction = true;
		playDuration = 0.0f;
	}

	private bool PerformAction()
	{
		if ( playResult == null || playResult.Count == 0 || playIndex >= playResult.Count )
		{
			return true;
		}

		var action = playResult[ playIndex ];
		if ( null == action )
		{
			return true;
		}

		if ( action.time > playDuration )
		{
			return false;
		}

		if ( action.attacker == heroRole.gameInfo )
		{
			// Hero Attack
			App.Instance.audioCtrl.PlaySfx( EnumSfx.HeroAtk );
			heroRole.transform.DOShakePosition( 0.1f ).OnComplete(
				() => { if ( action.hit ) { App.Instance.audioCtrl.PlaySfx( EnumSfx.MonsterHurt ); } } );
			monsterRole.hpText.text = string.Format( "{0}", action.hp );
		}
		else
		{
			// Monster attack
			App.Instance.audioCtrl.PlaySfx( EnumSfx.MonsterAtk );
			monsterRole.transform.DOShakePosition( 0.1f ).OnComplete(
				() => { if ( action.hit ) { App.Instance.audioCtrl.PlaySfx( EnumSfx.HeroHurt ); } } );
			heroRole.hpText.text = string.Format( "{0}", action.hp );
		}

		return ++playIndex >= playResult.Count;
	}
}

public enum StageState
{
	None,
	Moving,
	Meet,
	Fight,
	Final,
}
