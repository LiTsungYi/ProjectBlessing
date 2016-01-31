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
	[HideInInspector] public UITitleController titleCtrl;

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

	private EnumSfx HeroAttackMinIndex = EnumSfx.HeroAttack1;
	private EnumSfx HeroAttackMaxIndex = EnumSfx.HeroAttack3;
	private EnumSfx HeroHurtMinIndex = EnumSfx.HeroHurt2;
	private EnumSfx HeroHurtMaxIndex = EnumSfx.HeroHurt3;
	private EnumSfx HeroDeathMinIndex = EnumSfx.HeroDeath1;
	private EnumSfx HeroDeathMaxIndex = EnumSfx.HeroDeath3;

	private EnumSfx BatapongAttackMinIndex = EnumSfx.BatapongAttack1;
	private EnumSfx BatapongAttackMaxIndex = EnumSfx.BatapongAttack3;
	private EnumSfx BatapongHurtMinIndex = EnumSfx.BatapongHurt1;
	private EnumSfx BatapongHurtMaxIndex = EnumSfx.BatapongHurt3;
	private EnumSfx BatapongDeathMinIndex = EnumSfx.BatapongDeath1;
	private EnumSfx BatapongDeathMaxIndex = EnumSfx.BatapongDeath3;
	
	private EnumSfx PiguluAttackMinIndex = EnumSfx.PiguluAttack1;
	private EnumSfx PiguluAttackMaxIndex = EnumSfx.PiguluAttack3;
	private EnumSfx PiguluHurtMinIndex = EnumSfx.PiguluHurt1;
	private EnumSfx PiguluHurtMaxIndex = EnumSfx.PiguluHurt3;
	private EnumSfx PiguluDeathMinIndex = EnumSfx.PiguluDeath1;
	private EnumSfx PiguluDeathMaxIndex = EnumSfx.PiguluDeath3;

	private EnumSfx SirLovelotAttackMinIndex = EnumSfx.SirLovelotAttack1;
	private EnumSfx SirLovelotAttackMaxIndex = EnumSfx.SirLovelotAttack5;
	private EnumSfx SirLovelotHurtMinIndex = EnumSfx.SirLovelotHurt1;
	private EnumSfx SirLovelotHurtMaxIndex = EnumSfx.SirLovelotHurt4;
	private EnumSfx SirLovelotDeathMinIndex = EnumSfx.SirLovelotDeath1;
	private EnumSfx SirLovelotDeathMaxIndex = EnumSfx.SirLovelotDeath4;
	
	private EnumSfx MonsterAttackMinIndex = EnumSfx.BatapongAttack1;
	private EnumSfx MonsterAttackMaxIndex = EnumSfx.BatapongAttack3;
	private EnumSfx MonsterHurtMinIndex = EnumSfx.BatapongHurt1;
	private EnumSfx MonsterHurtMaxIndex = EnumSfx.BatapongHurt3;
	private EnumSfx MonsterDeathMinIndex = EnumSfx.BatapongDeath1;
	private EnumSfx MonsterDeathMaxIndex = EnumSfx.BatapongDeath3;

	void Awake()
	{
		titleCtrl = TSUtil.InstantiateForUGUI(uiTitlePrefab, canvasTrans).GetComponent<UITitleController>();
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
		
		bgloader.Init(App.Instance.monsterInfo.stage);
//		bgloader.Init(EnumStage.GAY);
		bgloader.SetX(-bgMoveX);
		
		SetState( StageState.Moving );
		titleCtrl.SetText(App.Instance.GetTheHonorName());
		titleCtrl.SetIntroDef();

		ResetMusic();
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

	private void ResetMusic()
	{
		var monsterName = App.Instance.monsterInfo.id;
		switch ( monsterName )
		{
		case "BATAPONG":
			App.Instance.audioCtrl.PlayBGM( EnumAudio.ICE_FOREST, 0.0f, 0.2f );
			MonsterAttackMinIndex = BatapongAttackMinIndex;
			MonsterAttackMaxIndex = BatapongAttackMaxIndex;
			MonsterHurtMinIndex = BatapongHurtMinIndex;
			MonsterHurtMaxIndex = BatapongHurtMaxIndex;
			MonsterDeathMinIndex = BatapongDeathMinIndex;
			MonsterDeathMaxIndex = BatapongDeathMaxIndex;
			break;

		case "PIGULU":
			App.Instance.audioCtrl.PlayBGM( EnumAudio.SWAMP, 0.0f, 0.2f );
			MonsterAttackMinIndex = PiguluAttackMinIndex;
			MonsterAttackMaxIndex = PiguluAttackMaxIndex;
			MonsterHurtMinIndex = PiguluHurtMinIndex;
			MonsterHurtMaxIndex = PiguluHurtMaxIndex;
			MonsterDeathMinIndex = PiguluDeathMinIndex;
			MonsterDeathMaxIndex = PiguluDeathMaxIndex;
			break;

		case "SIR LOVELOT":
			App.Instance.audioCtrl.PlayBGM( EnumAudio.GAY, 0.0f, 0.2f );
			MonsterAttackMinIndex = SirLovelotAttackMinIndex;
			MonsterAttackMaxIndex = SirLovelotAttackMaxIndex;
			MonsterHurtMinIndex = SirLovelotHurtMinIndex;
			MonsterHurtMaxIndex = SirLovelotHurtMaxIndex;
			MonsterDeathMinIndex = SirLovelotDeathMinIndex;
			MonsterDeathMaxIndex = SirLovelotDeathMaxIndex;
			break;

		default:
			App.Instance.audioCtrl.PlayBGM( EnumAudio.ICE_FOREST, 0.0f, 0.2f );
			break;
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
			titleCtrl.ShowIntro(introDuration + 5f);
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
			App.Instance.audioCtrl.PlayBGM( EnumAudio.INGAME_INTRO );
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
				var deathIndex = AudioController.GetRandom( MonsterDeathMinIndex, MonsterDeathMaxIndex );
				App.Instance.audioCtrl.PlaySfx( deathIndex );
				monsterRole.gameObject.SetActive( false );
			}
			else
			{
				var deathIndex = AudioController.GetRandom( HeroDeathMinIndex, HeroDeathMaxIndex );
				App.Instance.audioCtrl.PlaySfx( deathIndex );
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
			var atkIndex = AudioController.GetRandom( HeroAttackMinIndex, HeroAttackMaxIndex );
			var hurtIndex = AudioController.GetRandom( MonsterAttackMinIndex, MonsterAttackMaxIndex );
			App.Instance.audioCtrl.PlaySfx( atkIndex );
			heroRole.transform.DOShakePosition( 0.1f ).OnComplete(
				() => { if ( action.hit ) { App.Instance.audioCtrl.PlaySfx( hurtIndex ); } } );
			monsterRole.hpText.text = string.Format( "{0}", action.hp );
		}
		else
		{
			// Monster attack
			var atkIndex = AudioController.GetRandom( MonsterAttackMinIndex, MonsterAttackMaxIndex );
			var hurtIndex = AudioController.GetRandom( HeroAttackMinIndex, HeroAttackMaxIndex );
			App.Instance.audioCtrl.PlaySfx( atkIndex );
			monsterRole.transform.DOShakePosition( 0.1f ).OnComplete(
				() => { if ( action.hit ) { App.Instance.audioCtrl.PlaySfx( hurtIndex ); } } );
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
