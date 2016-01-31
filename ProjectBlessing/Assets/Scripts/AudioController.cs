using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioController : MonoBehaviour 
{
	public AudioSource audioSource;
	public AudioClip[] auidoClips;
	public AudioSource sfxSource;
	public AudioClip[] sfxClips;
	
	public void PlayBGM(EnumAudio audio, float fadeValue, float fadeTime )
	{
		if ( audioSource.isPlaying )
		{
			var preValue = audioSource.volume;
			audioSource.DOFade( fadeValue, fadeTime ).OnComplete(
				() => { PlayBGM( audio ); audioSource.DOFade( preValue, fadeTime ); } );
		}
		else
		{
			PlayBGM(audio);
		}
	}

	public void PlayBGM(EnumAudio audio)
	{
		audioSource.clip = auidoClips[(int)audio];
		audioSource.Play();
	}

	public void PlaySfx(EnumSfx sfx )
	{
		sfxSource.PlayOneShot(sfxClips[ ( int ) sfx ]);
	}
}

public enum EnumAudio
{
	MENU,
	INGAME,
	INGAME_INTRO,
	ICE_FOREST,
}

public enum EnumSfx
{
	PowerUp,
	HeroAtk,
	HeroHurt,
	HeroDie,
	MonsterAtk,
	MonsterHurt,
	MonsterDie,
}
