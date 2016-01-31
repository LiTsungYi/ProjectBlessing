using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioController : MonoBehaviour 
{
	public AudioSource audioSource;
	public AudioClip[] auidoClips;
	public AudioSource sfxSource1;
	public AudioSource sfxSource2;
	public AudioClip[] sfxClips;
	private int lastSound = 0;
	
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
		if ( !sfxSource1.isPlaying )
		{
			lastSound = 1;
			sfxSource1.PlayOneShot(sfxClips[ ( int ) sfx ]);
		}
		else if ( !sfxSource2.isPlaying )
		{
			lastSound = 2;
			sfxSource2.PlayOneShot(sfxClips[ ( int ) sfx ]);
		}
		else
		{
			if ( 1 == lastSound )
			{
				sfxSource2.PlayOneShot(sfxClips[ ( int ) sfx ]);
			}
			else if ( 2 == lastSound )
			{
				sfxSource1.PlayOneShot(sfxClips[ ( int ) sfx ]);
			}
		}
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
