using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
	public GameRules gameRule;
	public Camera theCamera;

	public Role heroRole;
	public Role monsterRole;
	public bool enableBack = true;

	private bool playAction = false;
	private int playIndex = 0;
	private IList<AttackResult> playResult = new List<AttackResult>();
	private float playDuration = 0.0f;
	
	private StageState state = StageState.None;
	private bool entering = false;

	void Start ()
	{
		// NOTE: set hero and monster here!
		var heroPosition = heroRole.gameObject.transform.position;
		heroRole.gameObject.transform.position = new Vector3( 0.0f, heroPosition.y, heroPosition.z );
		heroRole.gameObject.SetActive( true );
		var monsterPosition = monsterRole.gameObject.transform.position;
		monsterRole.gameObject.transform.position = new Vector3( 505.0f, monsterPosition.y, monsterPosition.z );
		monsterRole.gameObject.SetActive( true );
		SetState( StageState.Moving );
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
			theCamera.transform.DOMoveX( 500.0f, 5.0f ).OnComplete( MoveEnd );
			heroRole.gameObject.transform.DOMoveX( 500.0f, 5.0f );
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
			if ( enableBack )
			{
				Application.LoadLevel("ritual");
			}
			else
			{
				if ( App.Instance.isWin )
				{
					monsterRole.gameObject.SetActive( false );
				}
				else
				{
					heroRole.gameObject.SetActive( false );
				}

				SetState( StageState.None );
			}
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

	    var monster = new GameInfo( App.Instance.monsterInfo );
		Debug.Log( string.Format( "Monster: {0}/{6}, Hp={1}, Atk={2}, Def={3}, Spd={4}, Avo={5}", 
			monster.Name, monster.HitPoint, monster.Attack, monster.Defence, monster.Speed, monster.Avoid, monster.Id ) );
		monsterRole.gameInfo = monster;

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
			heroRole.transform.DOShakePosition( 0.1f );
		}
		else
		{
			// Monster attack
			monsterRole.transform.DOShakePosition( 0.1f );
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
