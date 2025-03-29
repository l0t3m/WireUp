using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public enum AudioNames
{
    Music,
    Click,
    Rotate,
    Start,
    Win,
    Lose
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] SettingsHandler settingsHandler;

    [SerializeField] AudioSource[] Audios;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (settingsHandler != null)
        {
            settingsHandler.OnSFXChange += ChangeSFX;
            settingsHandler.OnMusicChange += ChangeMusic;
        }
        ChangeSFX(PlayerPrefs.GetFloat("SFXVolume", 5));
        ChangeMusic(PlayerPrefs.GetFloat("MusicVolume", 5));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PlaySound(AudioNames.Click);
    }

    public void ChangeSFX(float volume)
    {
        mainMixer.SetFloat("SFXVol", volume == 0 ? -80 : Mathf.Log10(volume)*20);
    }

    public void ChangeMusic(float volume)
    {
        mainMixer.SetFloat("MusicVol", volume == 0 ? -80 : Mathf.Log10(volume)*20);
    }

    public void PlaySound(AudioNames name)
    {
        Audios[(int)name].Play();
    }
}
