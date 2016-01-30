using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioController : MonoBehaviour 
{
	public AudioSource audioSource;
	public AudioClip[] auidoClips;
	
	public void PlayBGM(EnumAudio audio)
	{
		audioSource.clip = auidoClips[(int)audio];
		audioSource.Play();
	}
}

public enum EnumAudio
{
	MENU,
	INGAME,
	INGAME_INTRO,
	ICE_FOREST,
}
