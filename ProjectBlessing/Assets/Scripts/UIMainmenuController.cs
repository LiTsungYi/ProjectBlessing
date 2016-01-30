using UnityEngine;
using System.Collections;

public class UIMainmenuController : MonoBehaviour 
{
	public GameObject StartBtn;
	// Use this for initialization
	void Start ()
	{
		EventTriggerListener.Get(StartBtn).onClick = OnBtnClick;
	}
	
	// Update is called once per frame
	void OnBtnClick (GameObject obj)
	{
		Application.LoadLevel("ritual");
	}
}
