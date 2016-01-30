using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
	public GameDataView gameDataView;

	private static GameRule gameRule;

	public Role heroRole;
	public Role monsterRole;
	public Text message;
	public GameObject fightButton;

	private object actionLock = new object();
	private bool playAction
	{
		get
		{
			lock ( actionLock )
			{
				return playAction;
			}
		}

		set
		{
			lock ( actionLock )
			{
				playAction = value;
			}
		}
	}
	private int playIndex = 0;
	private IList<AttackResult> playResult = null;
	private float playDuration = 0.0f;

	void Start ()
	{
		Reset();
		EventTriggerListener.Get(fightButton).onClick = OnFightClick;
	}

	void FixedUpdate()
	{
		if ( playAction )
		{
			playDuration += Time.fixedDeltaTime;
			var finish = PerformAction();

			if ( finish )
			{
				Reset();
			}
		}

	}

	private void Reset()
	{
		playIndex = 0;
		playResult = new List<AttackResult>();
		playAction = false;
		playDuration = 0.0f;
	}

	public void OnFightClick( GameObject obj )
	{
		if ( playAction )
		{
			return;
		}

		//Hero = App.Instance.heroInfo;
		//Monster = App.Instance.monsterInfo;
		gameRule = new GameRule( heroRole.gameInfo, monsterRole.gameInfo );
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

		if ( action.time < playDuration )
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
