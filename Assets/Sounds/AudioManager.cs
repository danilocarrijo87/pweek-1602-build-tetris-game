using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip Theme;
    public AudioClip MovePiece;
    public AudioClip SpinPiece;
    public AudioClip PlayerScore;
    public AudioClip StartMenu;
    public AudioClip GameOver;
    public AudioClip LateGameTheme;
    public AudioClip CantMove;
    public AudioClip DropPiece;

    private void Start()
    {
        MusicSource.clip = Theme;
        MusicSource.Play();
    }

    public void PlayMusicSource(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
