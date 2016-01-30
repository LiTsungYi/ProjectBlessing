using UnityEngine;
using System.Collections;

public class GameDataView : GoogleDataBase<GameData> 
{

}

[System.Serializable]
public class GameData
{
	public string id;
	public string name;
	public string image;
	
	public int hitPoint;
	public int attack;
	public int defence;
	public float speed;
	public float avoid;
	
	public string skill;
	public string note;
}