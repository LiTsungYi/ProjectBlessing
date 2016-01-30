using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIResultsController : MonoBehaviour
{
	public Text resultsText;
	public Image resultImg;
	
	public void Show(bool isWin, Action callback)
	{
		gameObject.SetActive(true);
		if(isWin)	
		{
			resultsText.text = "WIN";
		}
		else
		{
			resultsText.text = "LOSE";
		}
		StartCoroutine(TSUtil.WaitForSeconds(1.5f, ()=>{
			if(null != callback)	callback();
		}));
	}
	
	public void ShowGameover()
	{
		gameObject.SetActive(true);
		resultsText.text = "FIN";
	}
}
