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
		Monster = monster;
	}
	
	private bool CalaulateHit( GameInfo defender )
	{
		var random = UnityEngine.Random.value;
		return ( random > defender.Avoid );
	}

	private int CalulateDamage( GameInfo attacker, GameInfo defender )
	{
		return Math.Max( attacker.Attack - defender.Defence, 1 );
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

		while ( Hero.Alive && Monster.Alive )
		{
			GameInfo attacker = null;
			GameInfo defender = null;
			
			if ( Hero.Cooldown < Monster.Cooldown  )
			{
				// Hero Attack!
				Debug.Log( "Hero Attack!" );
				
				attacker = Hero;
				defender = Monster;
			}
			else
			{
				// Monster Attack!
				Debug.Log( "Monster Attack!" );
				
				attacker = Monster;
				defender = Hero;
			}
			
			if ( !CalaulateHit( defender ) )
			{
				Debug.Log( string.Format( "{0} missed at {1}", attacker.Name, attacker.Cooldown ) );

				result.Add( new AttackResult() {
					time = attacker.Cooldown,
					attacker = attacker.Id,
					defender = defender.Id,
					hit = false,
					damage = 0,
				});
				UpdateCooldown( attacker );
				continue;
			}
			
			var damage = CalulateDamage( attacker, defender );
			UpdateDamage( defender, damage );
			result.Add( new AttackResult() {
				time = attacker.Cooldown,
				attacker = attacker.Id,
				defender = defender.Id,
				hit = true,
				damage = damage,
			});
			
			Debug.Log( string.Format( "{0} deal {1} damage to {2} at {3}", attacker.Name, damage, defender.Name, attacker.Cooldown ) );
			UpdateCooldown( attacker );
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

		Cooldown = Speed;
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

}




public class AttackResult
{
	public float time;
	public string attacker;
	public string defender;
	public bool hit;
	public int damage;
}

