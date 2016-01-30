using System.Collections;
using UnityEngine;

public class gameplay : MonoBehaviour
{
	public GameDataView gameDataView;

	private GameData Hero;
	private GameData Monster;
	private GameRule GameRule;

	void Start ()
	{
		Hero = App.Instance.Hero;
		Monster = App.Instance.Monster;
		GameRule = new GameRule( Hero, Monster );
		GameRule.Attack();
	}

	void Update ()
	{
		
	}




}
