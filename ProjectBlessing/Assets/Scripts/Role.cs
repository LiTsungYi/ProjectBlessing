using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Role : MonoBehaviour
{
	[HideInInspector]
	public GameInfo gameInfo;

	public TextMesh hpText;
	
	private GameObject instanceRole = null;
	public void CreateRole(string roleName)
	{
		string path = "Roles/" + roleName;
		var prefab = Resources.Load<GameObject>(path);
		instanceRole = TSUtil.Instantiate(prefab, transform);
	}
}

