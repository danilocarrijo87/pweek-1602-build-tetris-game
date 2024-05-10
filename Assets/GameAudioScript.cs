using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameAudioScript : MonoBehaviour
{
    [SerializeField] AudioSource MainAudioSource;
    [SerializeField] AudioSource MoveAudioSource;

    public AudioClip MainMenuSound;
    public AudioClip GameOverSound;
    public AudioClip GameSound;
    public AudioClip MovePiece;
    public AudioClip BlockPiece;
    public AudioClip DropPiece;
    public AudioClip RotatePiece;
    public AudioClip ClearLine;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayGameSound();
    }

    public void PlayGameSound()
    {
        MainAudioSource.clip = GameSound;
        MainAudioSource.Play();
    }

    public void PlayMainMenuSound()
    {
        MainAudioSource.clip = MainMenuSound;
        MainAudioSource.Play();
    }

    public void PlayMoveSound()
    {
        MoveAudioSource.clip = MovePiece;
        MoveAudioSource.Play();
    }

    public void PlayBlockSound()
    {
        MoveAudioSource.clip = BlockPiece;
        MoveAudioSource.Play();
    }

    public void PlayDropSound()
    {
        MoveAudioSource.clip = DropPiece;
        MoveAudioSource.Play();
    }

    public void PlayRotateSound()
    {
        MoveAudioSource.clip = RotatePiece;
        MoveAudioSource.Play();
    }

    public void PlayClearLineSound()
    {
        MoveAudioSource.clip = ClearLine;
        MoveAudioSource.Play();
    }
    
    public void PlayGameOverSound()
    {
        MainAudioSource.Stop();
        MoveAudioSource.clip = GameOverSound;
        MoveAudioSource.Play();

        Invoke( nameof(PlayMainMenuSound) , MoveAudioSource.clip.length );
    }
}
