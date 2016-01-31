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
	None,
	HeroTalk1,
	HeroTalk2,
	HeroTalk3,
	HeroAttack1,
	HeroAttack2,
	HeroAttack3,
	HeroHurt2,
	HeroHurt3,
	HeroDeath1,
	HeroDeath2,
	HeroDeath3,
	BatapongAttack1,
	BatapongAttack2,
	BatapongAttack3,
	BatapongHurt1,
	BatapongHurt2,
	BatapongHurt3,
	BatapongDeath1,
	BatapongDeath2,
	BatapongDeath3,
	PiguluAttack1,
	PiguluAttack2,
	PiguluAttack3,
	PiguluHurt1,
	PiguluHurt2,
	PiguluHurt3,
	PiguluDeath1,
	PiguluDeath2,
	PiguluDeath3,
	SirLovelotAttack1,
	SirLovelotAttack2,
	SirLovelotAttack3,
	SirLovelotAttack5,
	SirLovelotHurt1,
	SirLovelotHurt2,
	SirLovelotHurt3,
	SirLovelotHurt4,
	SirLovelotDeath1,
	SirLovelotDeath2,
	SirLovelotDeath3,
	SirLovelotDeath4,
}
