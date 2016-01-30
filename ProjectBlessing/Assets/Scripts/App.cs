using UnityEngine;
using System.Collections;

public class App : Singleton<App>
{
	[ReadOnly] [SerializeField] private GameData _HeroInfo = null;
	public GameData heroInfo
	{
		get
		{
			if(null == _HeroInfo)
			{
				_HeroInfo = TSSaveUtil.Instance.LoadFile<GameData>("HeroInfo");
				if(null == _HeroInfo)
				{
					_HeroInfo = CreateNewRoleInfo(EnumRoleType.HERO);
				}			
			}
			return _HeroInfo;
		}
	}
	
	[ReadOnly] [SerializeField] private GameData _MonsterInfo = null;
	public GameData monsterInfo
	{
		get
		{
			if(null == _MonsterInfo)
			{
				_MonsterInfo = CreateNewRoleInfo(EnumRoleType.MOSTER);
			}
			return _MonsterInfo;
		}
		
		set
		{
			_MonsterInfo = value;
		}
	}
	
	[ReadOnly] [SerializeField] private GoogleDataController _googleDataCtrl = null;
	public GoogleDataController googleDataCtrl
	{
		get
		{
			if(null == _googleDataCtrl)
			{
				var googlePrefab = Resources.Load("GoogleDataManager") as GameObject;
				if(null == googlePrefab)
				{
					Debug.LogError("NO GoogleDataManager FUCK!!!");
					return null;
				}
				
				if(null == _googleDataCtrl)
				{
					_googleDataCtrl = TSUtil.Instantiate(googlePrefab, transform).GetComponent<GoogleDataController>();
				}
			}
			return _googleDataCtrl;
		}
	}
	
	[ReadOnly] [SerializeField] private GameSettingManager _gameSettingManager = null;
	public GameSettingManager gameSettingManager
	{
		get
		{
			var gameSettingPrefab = Resources.Load("GameSettingManager") as GameObject;
			if(null == gameSettingPrefab)
			{
				Debug.LogError("NO GameSettingManager Shit!!!");
				return null;
			}
			
			if(null == _gameSettingManager)
			{
				_gameSettingManager = TSUtil.Instantiate(gameSettingPrefab, transform).GetComponent<GameSettingManager>();
			}
			return _gameSettingManager;
		}
	}
	
	public GameData CreateNewRoleInfo(EnumRoleType roleType)
	{
		GameData role = new GameData();
		RoleGeneraSetting generalSetting = null;
		switch(roleType)
		{
		case EnumRoleType.HERO:
			generalSetting = gameSettingManager.heroDefSetting;
			break;
			
		case EnumRoleType.MOSTER:
			generalSetting = gameSettingManager.mosterDefSetting;
			break;
		}
		
		role.hitPoint = generalSetting.hpDef;
		role.attack = generalSetting.atkDef;
		role.defence = generalSetting.defDef;
		role.avoid = generalSetting.avoidDef;
		role.speed = generalSetting.speedDef;
		
		return role;
	}
	
	public void AddRoleValue(GameData roleInfo, EnumRoleValueType addType)
	{
		switch(addType)
		{
		case EnumRoleValueType.hp:
			roleInfo.vit++;
			roleInfo.hitPoint += roleInfo.vit;
			break;
			
		case EnumRoleValueType.atk:
			roleInfo.attack += Random.Range(1, roleInfo.lv);
			break;
			
		case EnumRoleValueType.def:
			roleInfo.defence += Random.Range(1, roleInfo.lv);
			break;
			
		case EnumRoleValueType.avoid:
			roleInfo.agi++;
			if(roleInfo.agi <= 25)
			{
				roleInfo.avoid += 2;
			}
			else
			{
				roleInfo.avoid += 1;
			}
			break;
			
		case EnumRoleValueType.speed:
			roleInfo.dex++;
			roleInfo.speed = 1 * Mathf.Pow( 0.9f, roleInfo.dex);
			break;
		}
	}
}

public enum EnumRoleType
{
	HERO,
	MOSTER,
}

public enum EnumRoleValueType
{
	hp,
	atk,
	def,
	avoid,
	speed,
	
	max,
}