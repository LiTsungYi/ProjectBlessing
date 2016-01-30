using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(ScrollRect))]
public class UGUIScrollMouseWheel : MonoBehaviour, IScrollHandler
{
	private RectTransform transRef;
	private ScrollRect scrollRef;
	private RectTransform contentRef;
	
	public float scrollSpeed = 100;
	
	[ReadOnly][SerializeField] private float minScroll = 0;
	[ReadOnly][SerializeField] private float maxScroll = 0;
	
	void Start()
	{
		Init();
	}
	
	public void Init()
	{
		transRef = GetComponent<RectTransform> ();
		scrollRef = GetComponent<ScrollRect> ();
		contentRef = scrollRef.content;
		maxScroll = contentRef.rect.height - transRef.rect.height;
		if(maxScroll < 0)
		{
			maxScroll = 0;
		}
	}
	
	public void OnScroll(PointerEventData eventData)
	{
		Vector2 ScrollDelta = eventData.scrollDelta;
		
		contentRef.anchoredPosition += new Vector2(0, -ScrollDelta.y * scrollSpeed);
		
		if(contentRef.anchoredPosition.y < minScroll)
		{
			contentRef.anchoredPosition = new Vector2(0, minScroll);
		}
		else if(contentRef.anchoredPosition.y > maxScroll)
		{
			contentRef.anchoredPosition = new Vector2(0, maxScroll);
		}
	}
}
