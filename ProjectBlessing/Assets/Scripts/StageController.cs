using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StageController : MonoBehaviour
{
	public Camera theCamera;

	public GameObject hero;
	public GameObject monster;

	public Gameplay gamePlay;

	private StageState state = StageState.None;
	private bool entering = false;
	
	void Start ()
	{
		// NOTE: set hero and monster here!
		var heroPosition = hero.transform.position;
		hero.transform.position = new Vector3( -10.0f, heroPosition.y, heroPosition.z );
		hero.active = true;
		var monsterPosition = monster.transform.position;
		monster.transform.position = new Vector3( 510.0f, monsterPosition.y, monsterPosition.z );
		monster.active = true;
		state  = StageState.Moving;
		entering = true;
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
			break;

		default:
			break;
		}
	}

	private void Moving()
	{
		if ( entering )
		{
			theCamera.transform.DOMoveX( 500.0f, 5.0f ).OnComplete( MoveEnd );
			hero.transform.DOMoveX( 490.0f, 5.0f );
			entering = false;
		}
	}

	private void MoveEnd()
	{
		state = StageState.Meet;
		entering = true;
	}

	private void Meeting()
	{
		if ( entering )
		{
			entering = false;
			state = StageState.Fight;
			entering = true;
		}
	}

	private void Fighting()
	{
		if ( entering )
		{
			entering = false;
			gamePlay.Attack();
		}
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
