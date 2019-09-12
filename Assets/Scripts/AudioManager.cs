using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _clickButton;
    [SerializeField] AudioClip _clickSlot;
    [SerializeField] AudioClip _winYay;
    [SerializeField] AudioSource _sfxAudioSource;

    public void PlayClickButton()
    {
        _sfxAudioSource.PlayOneShot(_clickButton);
    }

    public void PlayClickSlot()
    {
        _sfxAudioSource.PlayOneShot(_clickSlot);
    }

    public void PlayWinYay()
    {
        _sfxAudioSource.PlayOneShot(_winYay);
    }
}
