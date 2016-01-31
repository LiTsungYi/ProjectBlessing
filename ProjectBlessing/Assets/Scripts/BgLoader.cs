using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BgLoader : MonoBehaviour
{
	public Transform frontBg;
	public Transform backBg;
	
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
