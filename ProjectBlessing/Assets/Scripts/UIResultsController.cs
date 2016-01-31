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
		var duration = 1.5f;
		if(isWin)	
		{
			App.Instance.audioCtrl.PlayBGM( EnumAudio.VICTORY );
			duration = 5.0f;
			resultsText.text = "WIN";
		}
		else
		{
			App.Instance.audioCtrl.PlayBGM( EnumAudio.FAIL );
			duration = 7.0f;
			resultsText.text = "LOSE";
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
