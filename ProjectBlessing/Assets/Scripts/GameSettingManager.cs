using UnityEngine;
using System.Collections;

public class GameSettingManager : MonoBehaviour
{	
	public RoleGeneraSetting heroDefSetting;
	public RoleGeneraSetting mosterDefSetting;
}

[System.Serializable]
public class RoleGeneraSetting
{
	public int hpMin = 1;
	public int hpMax = 100;
	public int hpDef
	{
		get
		{
			return Random.Range(hpMin, hpMax);
		}
	}
	
	public int atkMin = 1;
	public int atkMax = 10;
	public int atkDef
	{
		get
		{
			return Random.Range(atkMin, atkMax);
		}
	}
	
	public int defMin = 1;
	public int defMax = 10;
	public int defDef
	{
		get
		{
			return Random.Range(defMin, defMax);
		}
	}
	
	public float avoidMin = 1;
	public float avoidMax = 10;
	public float avoidDef
	{
		get
		{
			return Random.Range(avoidMin, avoidMax);
		}
	}
	
	public float speedMin = 1;
	public float speedMax = 10;
	public float speedDef
	{
		get
		{
			return Random.Range(speedMin, speedMax);
		}
	}
}