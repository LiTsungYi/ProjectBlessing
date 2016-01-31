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
	
	public int vit = 0;
	public int agi = 0;
	public int dex = 0;
	
	public string name = string.Empty;
	public int lv = 0;
	public int hitPoint;
	public int attack;
	public int defence;
	public float speed;
	public float avoid;
	
	public EnumStage stage;
}