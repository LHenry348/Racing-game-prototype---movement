using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource menuMusic;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        menuMusic = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (menuMusic.isPlaying) return;
        menuMusic.Play();
    }

    public void StopMusic()
    {
        menuMusic.Stop();
    }
}
