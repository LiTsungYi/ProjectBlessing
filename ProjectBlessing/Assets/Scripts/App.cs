using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : Singleton<App>
{
	public List<string> usedHonorName = new List<string>();
	string[] honorNames = new string[] {
		"Sword of QQQ", "Speer of QQQ", "Axe of QQQ", "Hammer of QQQ", "Flail of QQQ", "Fortress of QQQ", "Protector of QQQ", "Guardian of QQQ", "Savior of QQQ", "Shield of QQQ", "Slayer of QQQ", "Warrior of QQQ", "Assassin of QQQ", "Fighter of QQQ", "Soldier of QQQ", "QQQ of the Mist Mountain", "QQQ of the Deep Sea", "QQQ of the Crystal Lake", "QQQ of the Endless River", "QQQ of the Faraway Forest", "the Flame QQQ", "the Ice QQQ", "the Wind QQQ", "the Light of QQQ", "the Dark of QQQ"};
		
	string[] nickNames = new string[] {
		"Eldn", "Jyser", "Nybur", "Loos", "Dar'eno", "Honkelon", "Zhoath", "Sertherche", "Ingk", "Hior", "On'hone", "Narixu", "Urn'adu", "Honril", "Und'ghau", "Rod'gar", "Joull", "Torsamris", "Smyshn", "Atu", "Che'oughu", "Emxem", "Vedeso", "Mojit", "Imnver", "Skelyll", "Breang", "Eldok", "Ther'ryna", "Uwara", "Roint", "Tassul", "Epola", "Buronn", "Ranash", "Amoru", "Hinrine", "Peror", "Rynon", "Omaiph", "Ghaiald't", "Radkin", "Snelt", "Orade", "Inaden", "Cybice", "Queque", "Lihat", "Atr", "Enthghae", "Tiaight", "Warpol", "Habana", "Mash", "Enard", "Nyskellye", "Schoran", "Lorslor", "Essenth", "Swivend", "Ard'lor", "Oreng", "Oity", "Awtin", "Uskrilo", "Cleastor", "Elmineo", "Undiusk", "Ormash", "Aleawold", "Adid'o", "Daryl", "Veren", "Tonol", "Chrieckton", "Sulelmeng", "Tas'um'aesh", "Sameldu", "Ateati", "Adeno", "Quataiend", "Saylenth", "Estnt", "Alevelm", "Kobala", "Bumof", "Ataiu", "Draull", "Belyf", "Naltas", "Quedan", "Saltghnal", "Ulyeo", "Cluthray", "Toreng", "Mosshin", "Cuhez", "Equao", "Ingon", "Ohati", "Sunal", "Traranyer", "Dra-ranir", "Morlor", "Che'om", "Angeyt'a", "Iright", "Enddarler", "Eldryn", "Yeroesti", "Zuntser", "Danem", "Beir", "Awar", "Tastas", "Isoq", "Cayus", "Serkime", "Cheit"};
		
	string[] monsterNames = new string[]{
		"AAA", "BBB"
	};

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
	
	public string GetTheHonorName()
	{
		string outName = string.Empty;
		int idx = Random.Range(0, honorNames.Length);
		usedHonorName.Add(honorNames[idx]);
		
		if(heroInfo.name.Equals(string.Empty))
		{
			heroInfo.name += honorNames[idx].Replace("QQQ", GetNickName());
		}
		else
		{
			heroInfo.name += ", " + honorNames[idx].Replace("QQQ", GetNickName());
		}
		
		Debug.Log("GetTheHonorName: " + heroInfo.name);
		return heroInfo.name;
	}
	
	public string GetNickName()
	{
		int idx = Random.Range(0, nickNames.Length);
		return nickNames[idx];
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
		
		int monsterId = Random.Range(0, monsterNames.Length);
		role.id = monsterNames[monsterId];
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