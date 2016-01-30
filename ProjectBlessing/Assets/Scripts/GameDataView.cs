using UnityEngine;
using System.Collections;

public class GameDataView : GoogleDataBase<GameData> 
{

}

[System.Serializable]
public class GameData
{
	public string id;
	public string image;
	
	public string name;
	public int lv = 0;
	public int hitPoint;
	public int attack;
	public int defence;
	public float speed;
	public float avoid;
	
	public string skill;
	public string note;
}