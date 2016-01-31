using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIResultsController : MonoBehaviour
{
	public Text resultsText;
	public Image winImg;
	public Image loseImg;
	
	public void Show(bool isWin, Action callback)
	{
		gameObject.SetActive(true);
		var duration = 1.5f;
		winImg.gameObject.SetActive(false);
		loseImg.gameObject.SetActive(false);
		if(isWin)	
		{
			App.Instance.audioCtrl.PlayBGM( EnumAudio.VICTORY );
			duration = 5.0f;
			resultsText.text = "WIN";
			winImg.gameObject.SetActive(true);
		}
		else
		{
			App.Instance.audioCtrl.PlayBGM( EnumAudio.FAIL );
			duration = 7.0f;
			resultsText.text = "LOSE";
			loseImg.gameObject.SetActive(true);
		}

		StartCoroutine(TSUtil.WaitForSeconds(duration, ()=>{
			if(null != callback)	callback();
		}));
	}
	
	public void ShowGameover()
	{
		gameObject.SetActive(true);
		resultsText.text = "FIN";
	}
}
