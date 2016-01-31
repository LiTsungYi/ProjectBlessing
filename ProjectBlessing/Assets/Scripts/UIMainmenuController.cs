using UnityEngine;
using System.Collections;

public class UIMainmenuController : MonoBehaviour 
{
	public GameObject StartBtn;
	public GameObject CreditBtn;
	// Use this for initialization
	void Start ()
	{
		EventTriggerListener.Get(StartBtn).onClick = OnBtnClick;
		//EventTriggerListener.Get(CreditBtn).onClick = OnCreditBtnClick;
	}
	
	// Update is called once per frame
	void OnBtnClick (GameObject obj)
	{
		Application.LoadLevel("ritual");
	}
}
