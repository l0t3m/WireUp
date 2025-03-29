using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsHandler : MonoBehaviour
{
    public event Action<float> OnSFXChange;
    public event Action<float> OnMusicChange;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 5);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        OnSFXChange?.Invoke(value);
    }

    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        OnMusicChange?.Invoke(value);
    }
}
