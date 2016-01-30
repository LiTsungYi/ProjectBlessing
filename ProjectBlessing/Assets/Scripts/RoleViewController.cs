using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleViewController : MonoBehaviour 
{
	public Text hpText;
	public Text atkText;
	public Text defText;
	public Text avoidText;
	public Text speedText;
	
	void Awake()
	{
//		hpText = transform.FindGrandChild("hp").GetComponent<Text>();
//		atkText = transform.FindGrandChild("atk").GetComponent<Text>();
//		defText = transform.FindGrandChild("def").GetComponent<Text>();
//		avoidText = transform.FindGrandChild("avoid").GetComponent<Text>();
//		speedText = transform.FindGrandChild("speed").GetComponent<Text>();
	}

	public void Show(GameData mosterData)
	{
		hpText.text = string.Format("{0}", mosterData.hitPoint);
		atkText.text = string.Format("{0}", mosterData.attack);
		defText.text = string.Format("{0}", mosterData.defence);
		avoidText.text = string.Format("{0}%", mosterData.avoid);
		speedText.text = string.Format("{0:0.00}", mosterData.speed);
	}
}
