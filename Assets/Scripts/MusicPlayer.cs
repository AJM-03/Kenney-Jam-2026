using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    [SerializeField] private AudioSource[] musicSources;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private float fadeSpeed = 1f;
    [HideInInspector] public int currentIntensity;



    void Start()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        ChangeSong(musicClips);
        ChangeSongIntensity(1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (musicSources[0].isPlaying)
                musicSources[0].Stop();
            else
                ChangeSong(musicClips); 
        }
    }

    public void ChangeSongIntensity(int intensity)
    {
        currentIntensity = Mathf.RoundToInt(Mathf.Clamp(intensity, 0f, musicSources.Length - 1));
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].DOKill();
            musicSources[i].DOFade(0, fadeSpeed).SetEase(Ease.InSine);
        }

        musicSources[currentIntensity].DOKill();
        musicSources[currentIntensity].DOFade(1, fadeSpeed).SetEase(Ease.OutSine);
    }

    public void ChangeSong(AudioClip[] song)
    {
        currentIntensity = 0;
        for (int i = 0; i < song.Length; i++)
        {
            if (musicSources.Length == i) break;
            musicSources[i].Stop();
            musicSources[i].clip = song[i];
            musicSources[i].loop = true;
            musicSources[i].Play();
        }
    }
}
