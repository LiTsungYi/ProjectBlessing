using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameRules
{
	private GameInfo Hero = null;
	private GameInfo Monster = null;

	public GameRules( GameInfo hero, GameInfo monster )
	{
		Hero = hero;
		Hero.FixValue();
		Monster = monster;
		Monster.FixValue();
	}
	
	private bool CalaulateHit( GameInfo defender )
	{
		var random = UnityEngine.Random.value;
		var avoid = Mathf.Min( defender.Avoid / 100.0f, 1.0f );
		return ( random > avoid );
	}

	private int CalulateDamage( GameInfo attacker, GameInfo defender )
	{
		return defender.Defence >= attacker.Attack ? 1 : attacker.Attack - defender.Defence + 1;
	}

	private void UpdateDamage( GameInfo defender, int damage )
	{
		defender.HitPoint = Math.Max( defender.HitPoint - damage, 0 );
	}

	private void UpdateCooldown( GameInfo attacker )
	{
		attacker.Cooldown += attacker.Speed;
	}

	public IList<AttackResult> Attack()
	{
		var result = new List<AttackResult>();
		if ( null == Hero || null == Monster )
		{
			return result;
		}

		App.Instance.isWin = false;
		while ( Hero.Alive && Monster.Alive )
		{
			GameInfo attacker = null;
			GameInfo defender = null;
			
			if ( Hero.Cooldown < Monster.Cooldown  )
			{
				attacker = Hero;
				defender = Monster;
			}
			else
			{
				attacker = Monster;
				defender = Hero;
			}
			
			if ( !CalaulateHit( defender ) )
			{
				Debug.Log( string.Format( "{0} missed at {1}", attacker.Name, attacker.Cooldown ) );

				result.Add( new AttackResult() {
					time = attacker.Cooldown,
					attacker = attacker,
					hit = false,
					damage = 0,
					hp = defender.HitPoint,
				});
				UpdateCooldown( attacker );
				continue;
			}
			
			var damage = CalulateDamage( attacker, defender );
			UpdateDamage( defender, damage );
			result.Add( new AttackResult() {
				time = attacker.Cooldown,
				attacker = attacker,
				hit = true,
				damage = damage,
				hp = defender.HitPoint,
			});
			
			Debug.Log( string.Format( "{0} deal {1} damage to {2} at {3}", attacker.Name, damage, defender.Name, attacker.Cooldown ) );
			UpdateCooldown( attacker );
		}

		if ( Hero.Alive )
		{
			App.Instance.isWin = true;
			Debug.Log( "Hero Win" );
		}
		else
		{
			Debug.Log( "Hero Lose" );
		}

		return result;
	}
}

[System.Serializable]
public class GameInfo
{
	public GameInfo( GameData data )
	{
		Id = data.id;
		Name = data.name;
		HitPoint = data.hitPoint;
		Attack = data.attack;
		Defence = data.defence;
		Speed = data.speed;
		Avoid = data.avoid;
	}

	public bool Alive
	{
		get
		{
			return HitPoint > 0;
		}
	}

	public string Id;
	public string Name;
	public int HitPoint;
	public int Attack;
	public int Defence;
	public float Speed;
	public float Avoid;

	[HideInInspector]
	public float Cooldown;

	public void FixValue()
	{
		Speed = Mathf.Max( Mathf.Min( Speed, 10.0f ), 0.01f );
		Avoid = Mathf.Max( Mathf.Min( Avoid, 100.0f ), 0.0f );
		
		Cooldown = Speed;
	}
}

public class AttackResult
{
	public float time;
	public GameInfo attacker;
	public bool hit;
	public int damage;
	public int hp;
}
