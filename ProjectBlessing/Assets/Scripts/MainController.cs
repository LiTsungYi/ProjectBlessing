using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		GameController.Instance.Init();
		App.Instance.ResetGameInfo();
		App.Instance.audioCtrl.PlayBGM( EnumAudio.SPLASH_SCREEN );
	}
}
