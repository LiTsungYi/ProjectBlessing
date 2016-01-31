using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BgLoader : MonoBehaviour
{
	public Transform frontBg;
	public Transform backBg;
	
	public SpriteRenderer frontSprite;
	public SpriteRenderer backSprite;
	
	public void Init(EnumStage stage)
	{
		string frontName = stage.ToString() + "_front";
		string backName = stage.ToString() + "_back";
		
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

public enum EnumStage
{
	ICE_FOREST = 0,
	SWAMP,
	GAY,
}