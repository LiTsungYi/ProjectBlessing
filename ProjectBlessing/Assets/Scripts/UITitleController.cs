using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UITitleController : MonoBehaviour 
{
	public RectTransform bgTrans;
	public Text nameText;
	
	public void SetText(string set)
	{
		nameText.text = set;
		Canvas.ForceUpdateCanvases();
	}
	
	public void SetIntroDef()
	{
		var pos = bgTrans.anchoredPosition;
		pos.x = bgTrans.rect.width;
		if(pos.x < 1920)
		{
			pos.x = 1920;
		}
		bgTrans.anchoredPosition = pos;
	}
	
	public void ShowIntro(float duration)
	{
//		bgTrans.DOAnchorPosX(-(bgTrans.anchoredPosition.x + 100f), duration)
//		bgTrans.do(-5f, 0.1f)
//			.SetRelative(true).SetLoops(100);
//			.SetEase(Ease.InOutSine);

		StartCoroutine(MoveTitle());
	}
	
	IEnumerator MoveTitle()
	{
		float targetX = -(bgTrans.anchoredPosition.x + 100f);
		while( bgTrans.anchoredPosition.x > targetX)
		{
			var pos = bgTrans.anchoredPosition;
			pos.x -= 12;
			bgTrans.anchoredPosition = pos;
			yield return new WaitForEndOfFrame();
		}
	}
}
