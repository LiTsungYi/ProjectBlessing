using UnityEngine;
using System.Collections;

public class GameDataManager : GoogleDataBase<GameData> 
{

}

[System.Serializable]
public class GameData
{
	public int a;
	public int b;
	public int c;
}