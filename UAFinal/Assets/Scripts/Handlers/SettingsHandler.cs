using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsHandler : MonoBehaviour
{
    public event Action<float> OnSFXChange;
    public event Action<float> OnMusicChange;
    public event Action<bool> OnTutorialChange;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Toggle isTutorialOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 5);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 5);
        isTutorialOn.isOn = PlayerPrefs.GetInt("TutorialValue", 1) == 1 ? true : false;
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

    public void ChangeTutorialValue(bool value)
    {
        PlayerPrefs.SetInt("TutorialValue", value ? 1 : 0);
        OnTutorialChange?.Invoke(value);
    }
}
