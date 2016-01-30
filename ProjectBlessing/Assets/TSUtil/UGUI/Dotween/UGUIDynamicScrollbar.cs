using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Scrollbar))]
public class UGUIDynamicScrollbar : MonoBehaviour 
{
	public ScrollRect scrollRect;
	
	public float duration = 0.5f;
	private CanvasGroup canvasGroup;
	
	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if(null == scrollRect)
		{
			Debug.LogError("UGUIDynamicScrollbar scrollRect null:" + name);
		}
	}
	
	public void UpdateScrollBar(bool isForceShowOff = false)
	{
		Canvas.ForceUpdateCanvases();
		RectTransform trans = scrollRect.GetComponent<RectTransform>();
		
		float targetAlpha = 0;
		if(	!isForceShowOff &&
			(scrollRect.content.offsetMax.y - scrollRect.content.offsetMin.y) > (trans.offsetMax.y - trans.offsetMin.y))
		{
			targetAlpha = 1;
		}
		
		if(canvasGroup.alpha != targetAlpha)
		{
			canvasGroup.DOFade(targetAlpha, duration);
		}
	}
}
