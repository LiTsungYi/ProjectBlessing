using UnityEngine;
using System.Collections;

public class GameDataView : GoogleDataBase<GameData> 
{

}

[System.Serializable]
public class GameData
{
	public int a;
	public int b;
	public int c;
}