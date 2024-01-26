using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip[] footstepSounds; // Array of footstep sound clips
    [SerializeField] private AudioClip[] playerDeath;
    [SerializeField] private AudioClip[] EnemyDeath;
    [SerializeField] private AudioClip[] StompSounds;
    [SerializeField] private AudioClip[] CollectionSounds;
    [SerializeField] private AudioSource effectsAudioSource; // AudioSource for playing sound effects

    void Awake()
    {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: make it persistent across scenes
        
    }

    public void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            effectsAudioSource.PlayOneShot(clip);
        }
    }
    public void PlayPlayerDeathSound()
    {
        if (playerDeath.Length > 0)
        {
            AudioClip clip = playerDeath[Random.Range(0, playerDeath.Length)];
            effectsAudioSource.PlayOneShot(clip);
        }
    }
    //blah
    public void PlayEnemyDeathSound()
    {
        if (EnemyDeath.Length > 0)
        {
            AudioClip clip = EnemyDeath[Random.Range(0, EnemyDeath.Length)];
            effectsAudioSource.PlayOneShot(clip);
        }
    }
    public void PlayStompSound()
    {
        if (StompSounds.Length > 0)
        {
            AudioClip clip = StompSounds[Random.Range(0, StompSounds.Length)];
            effectsAudioSource.PlayOneShot(clip);
        }
    }


    public void PlaySoundEffect(AudioClip clip)
    {
        effectsAudioSource.PlayOneShot(clip);
    }




    public void PlayEnemyFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            effectsAudioSource.PlayOneShot(clip);
        }
    }

    
}
