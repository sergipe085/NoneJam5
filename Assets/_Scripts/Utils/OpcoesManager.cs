using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpcoesManager : Singleton<OpcoesManager>
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectVolumeSlider;

    public event Action<float> OnMusicVolumeSliderChanged;
    public event Action<float> OnSoundEffectVolumeSliderChanged;

    private void Start() {
        Initialize();
    }

    private void Initialize() {
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
        SoundManager.Instance.audioSourceEffect.volume = GetSoundEffectVolume();
        SoundManager.Instance.audioSourceMusic.volume = GetMusicVolume();

        gameObject.SetActive(false);
    }

    private void SaveMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        OnMusicVolumeSliderChanged?.Invoke(volume);
        SoundManager.Instance.audioSourceMusic.volume = volume;
    }

    public float GetMusicVolume() {
        return PlayerPrefs.GetFloat("MusicVolume");
    }

    private void SaveSoundEffectVolume(float volume) {
        PlayerPrefs.SetFloat("SoundEffectVolume", volume);
        OnSoundEffectVolumeSliderChanged?.Invoke(volume);
        SoundManager.Instance.audioSourceEffect.volume = volume;
    }

    public float GetSoundEffectVolume() {
        return PlayerPrefs.GetFloat("SoundEffectVolume");
    }
}
