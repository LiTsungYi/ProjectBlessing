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
	public string hitPoint;
	public string attack;
	public string defence;
	public string speed;
	public string avoid;
	public string skill;
	public string note;
}