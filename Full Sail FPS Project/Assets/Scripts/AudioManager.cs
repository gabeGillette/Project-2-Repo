using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource;   // For background music
    [SerializeField] private AudioSource sfxSource;     // For sound effects
    [SerializeField] private bool isMusic3D = false;    // Toggle for 2D/3D music
    [SerializeField] private bool isSFX3D = false;      // Toggle for 2D/3D SFX

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play background music, either 2D or 3D depending on isMusic3D toggle
    public void PlayMusic(AudioClip music, Vector3 position = default)
    {
        musicSource.clip = music;
        musicSource.loop = true;

        if (isMusic3D)
        {
            musicSource.spatialBlend = 1.0f; // Set to 3D
            musicSource.transform.position = position; // Use the specified position
        }
        else
        {
            musicSource.spatialBlend = 0.0f; // Set to 2D
        }

        musicSource.Play();
    }

    // Stop background music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Play a sound effect, either 2D or 3D depending on isSFX3D toggle
    public void PlaySFX(AudioClip sfx, Vector3 position = default)
    {
        if (isSFX3D)
        {
            AudioSource.PlayClipAtPoint(sfx, position, sfxSource.volume); // Play at specified position
        }
        else
        {
            sfxSource.spatialBlend = 0.0f; // Set to 2D
            sfxSource.PlayOneShot(sfx);    // Play one-shot sound effect
        }
    }

    // Set volume for background music
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Set volume for sound effects
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
