using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BgLoader : MonoBehaviour
{
	public Transform frontBg;
	public Transform backBg;
	
	public SpriteRenderer frontSprite;
	public SpriteRenderer backSprite;
	
	public void Init(string monsterName)
	{
		string frontName = monsterName + "_front";
		string backName = monsterName + "_back";
		
		string frontPath = "Stages/" + frontName;
		Debug.Log("frontPath: " + frontPath);
		var sprite = Resources.Load<Sprite>(frontPath);
		frontSprite.sprite = sprite;
		backSprite.sprite = Resources.Load<Sprite>("Stages/" + backName);
	}
	
	public void SetX(float moveX)
	{
		backBg.position = new Vector3(moveX, backBg.position.y, backBg.position.z);
	}
	
	public void ShowMove(float moveX, float duration)
	{
		backBg.DOMoveX(0, duration)
			.SetEase(Ease.InOutSine);
	}
}