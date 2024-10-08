using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource[] music;
    [SerializeField] private AudioSource[] sfx;
    //[SerializeField] int levelMusicToPlay;
    public AudioMixerGroup musicMixer, sfxMixer;
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
    
    public void PlayMusic(int musicToPlay)
    {
        music[musicToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay) 
    {
        sfx[sfxToPlay].Play();
    }

    public void PauseSFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Pause();
    }
}
