using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioController : MonoBehaviour 
{
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioSource audioNoLoopSource;
	public AudioClip[] auidoClips;
	
	[Header("Sfx")]
	public AudioSource sfxSource1;
	public AudioSource sfxSource2;
	public AudioSource sfxLoopSource;
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
		audioNoLoopSource.Stop();
	}

	public void PlayOnceBGM(EnumAudio audio)
	{
		audioSource.Stop();
		audioNoLoopSource.PlayOneShot( auidoClips[(int)audio] );
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

	public void PlayLoopSfx( EnumSfx sfx )
	{
		sfxLoopSource.clip = sfxClips[(int)sfx];
		sfxLoopSource.Play();
	}
	
	public void StopLoopSfx()
	{
		if ( sfxLoopSource.isPlaying )
		{
			sfxLoopSource.Stop();
		}
	}

	public static EnumSfx GetRandom( EnumSfx start, EnumSfx end )
	{
		var length = ( int ) end - ( int ) start + 1;
		if ( length <= 1 )
		{
			return start;
		}
		
		var random = ( int ) ( UnityEngine.Random.value * length );
		var sfxIndex = ( EnumSfx )( random + start );
		return sfxIndex;
	}
}

public enum EnumAudio
{
	MENU,
	INGAME,
	INGAME_INTRO,
	ICE_FOREST,
	SWAMP,
	GAY,
	VICTORY,
	FAIL,
	SPLASH_SCREEN,
}

public enum EnumSfx
{
	SwardTap,
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
	Blessing,
	HeroMove1,
	HeroMove2,
	HeroMove3,
	HeroMove4,
}
