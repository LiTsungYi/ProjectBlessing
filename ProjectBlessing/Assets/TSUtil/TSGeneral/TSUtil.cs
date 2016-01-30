using UnityEngine;
using System.Collections;
using System;

public class TSUtil
{
	static public GameObject Instantiate(GameObject createObj, Transform parent = null, bool worldPosStays = true, bool resetTrans = true)
	{
		if( null == createObj)
		{
			throw new Exception("TSUtil Instantiate createObj null");
		}

		GameObject obj = GameObject.Instantiate(createObj) as GameObject;
		Transform trans = obj.transform;

		if( null != parent)
		{
			trans.SetParent(parent, worldPosStays);
		}

		if(resetTrans)
		{
			trans.localPosition = Vector3.zero;
			trans.localRotation = Quaternion.identity;
			trans.localScale = Vector3.one;
		}
		return obj;
	}

	static public GameObject InstantiateForUGUI(GameObject createObj, Transform parent)
	{
		return Instantiate(createObj, parent, false, false);
	}
	
	static public IEnumerator WaitForFixedUpdate(Action callback)
	{
		yield return new WaitForFixedUpdate();
		if(null != callback)
		{
			callback();
		}
	}

	static public IEnumerator WaitForEndOfFrame(Action callback)
	{
		yield return new WaitForEndOfFrame();
		
		if(null != callback)
		{
			callback();
		}
	}

	static public IEnumerator WaitForSeconds(float sec, Action callback)
	{
		yield return new WaitForSeconds(sec);

		if(null != callback)
		{
			callback();
		}
	}
	
	/// <summary>
	/// Checks the internet.
	/// </summary>
	/// <returns><c>true</c>, if internet was checked, <c>false</c> otherwise.</returns>
	/// ReachableViaLocalAreaNetwork only Know WIFI connect , if WIFI no connect to Internet, it still return true
	static public bool CheckInternet()
	{
		switch(Application.internetReachability)
		{
		case NetworkReachability.NotReachable:
			Debug.Log( "Internet NotReachable");
			return false;
			
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			Debug.Log( "Internet ReachableViaCarrierDataNetwork" );
			return true;
			
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			Debug.Log( "Internet ReachableViaLocalAreaNetwork" );
			return true;
		}
		
		return false;
	}
}
