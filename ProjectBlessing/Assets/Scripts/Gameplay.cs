using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
	public GameRules gameRule;

	public Role heroRole;
	public Role monsterRole;
	public Text message;
	public GameObject fightButton;
	public bool enableBack = true;

	private bool playAction = false;
	private int playIndex = 0;
	private IList<AttackResult> playResult = new List<AttackResult>();
	private float playDuration = 0.0f;

	void Start ()
	{
		//ResetPlay();
		EventTriggerListener.Get(fightButton).onClick = OnFightClickX;
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

				if (enableBack )
				{
					Application.LoadLevel("ritual");
				}
			}
		}
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
		if ( heroRole.enable )
		{
			hero = heroRole.gameInfo.DeepClone();
		}
		
		var monster = new GameInfo( App.Instance.monsterInfo );
		if ( monsterRole.enable )
		{
			monster = monsterRole.gameInfo.DeepClone();
		}
		
		gameRule = new GameRules( hero, monster );
		playResult = gameRule.Attack();
		
		playAction = true;
		playDuration = 0.0f;
	}

	public void OnFightClickX( GameObject obj )
	{
		if ( playAction )
		{
			return;
		}

		Attack();
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

		if ( action.attacker == heroRole.gameInfo.Id )
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
