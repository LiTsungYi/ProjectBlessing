using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameRule 
{
	private GameInfo Hero = null;
	private GameInfo Monster = null;

	public GameRule( GameData hero, GameData monster )
	{
		Hero = new GameInfo( hero );
		Monster = new GameInfo( monster );
	}

	public IList<AttackResult> Attack()
	{
		if ( null == Hero || null == Monster )
		{
			return new List<AttackResult>();
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
				UpdateCooldown( attacker );
				continue;
			}
			
			var damage = CalulateDamage( attacker, defender );
			UpdateDamage( defender, damage );

			Debug.Log( string.Format( "{0} deal {1} damage to {2} at {3}", attacker.Name, damage, defender.Name, attacker.Cooldown ) );
			UpdateCooldown( attacker );
		}

		return new List<AttackResult>();
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
}

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
	public float Cooldown;

}




public class AttackResult
{
}



