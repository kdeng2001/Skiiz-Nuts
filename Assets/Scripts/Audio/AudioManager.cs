using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// AudioManager contains methods for playing different sounds, music, sfx from two arrays of AudioSources.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource[] music;
    [SerializeField] private AudioSource[] sfx;
    //[SerializeField] int levelMusicToPlay;
    public AudioMixerGroup musicMixer, sfxMixer;

    /// <summary>
    /// Singleton functionality, ensuring one instance of AudioManager, that is not destroyed between scenes.
    /// </summary>
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //private void Start()
    //{
    //    PlayMusic(levelMusicToPlay);
    //}
    
    /// <summary>
    /// Plays music at index musicToPlay from the music property.
    /// </summary>
    /// <param name="musicToPlay"> An index for accessing elements of music property. </param>
    public void PlayMusic(int musicToPlay)
    {
        music[musicToPlay].Play();
    }

    /// <summary>
    /// Plays sfx at index sfxToPlay from the sfx property.
    /// </summary>
    /// <param name="sfxToPlay"> An index for accessing elements of sfx property. </param>
    public void PlaySFX(int sfxToPlay) 
    {
        sfx[sfxToPlay].Play();
    }

    /// <summary>
    /// Pauses sfx at index sfxToPlay from sfx property.
    /// </summary>
    /// <param name="sfxToPlay"> An index for accessing elements of sfx property. </param>
    public void PauseSFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Pause();
    }
}
