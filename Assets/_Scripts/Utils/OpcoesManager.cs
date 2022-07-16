using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpcoesManager : Singleton<OpcoesManager>
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;

    public event Action<float> OnMusicVolumeSliderChanged;
    public event Action<float> OnSoundEffectVolumeSliderChanged;

    protected override void Awake() {
        base.Awake();
        
        musicVolumeSlider.onValueChanged.AddListener(SaveMusicVolume);

        soundEffectVolumeSlider.onValueChanged.AddListener(SaveSoundEffectVolume);

        if (!PlayerPrefs.HasKey("SoundEffectVolume")) {
            SaveSoundEffectVolume(0.5f);
        }

        if (!PlayerPrefs.HasKey("MusicVolume")) {
            SaveMusicVolume(0.5f);
        }

        soundEffectVolumeSlider.value = GetSoundEffectVolume();
        musicVolumeSlider.value = GetMusicVolume();

        gameObject.SetActive(false);
    }

    private void SaveMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        OnMusicVolumeSliderChanged?.Invoke(volume);
    }

    public float GetMusicVolume() {
        return PlayerPrefs.GetFloat("MusicVolume");
    }

    private void SaveSoundEffectVolume(float volume) {
        PlayerPrefs.SetFloat("SoundEffectVolume", volume);
        OnSoundEffectVolumeSliderChanged?.Invoke(volume);
    }

    public float GetSoundEffectVolume() {
        return PlayerPrefs.GetFloat("SoundEffectVolume");
    }
}
