using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSourceEffect = null;
    [SerializeField] private AudioSource audioSourceMusic = null;

    [SerializeField] private AudioClip themeSong = null;
    [SerializeField] private AudioClip playerWinSong = null;
    [SerializeField] private AudioClip battleSong = null;
    [SerializeField] private AudioClip buttonSound = null;

    private float initialVolume;

    protected override void Awake() {
        base.Awake();
        SceneManager.sceneLoaded += (scene, mode) => Initialize();
        
        initialVolume = audioSourceMusic.volume;
    }

    private void Start() {
        audioSourceMusic.volume = OpcoesManager.Instance.GetMusicVolume();
        OpcoesManager.Instance.OnMusicVolumeSliderChanged += (volume) => audioSourceMusic.volume = volume;

        audioSourceEffect.volume = OpcoesManager.Instance.GetSoundEffectVolume();
        OpcoesManager.Instance.OnSoundEffectVolumeSliderChanged += (volume) => audioSourceEffect.volume = volume;
    }

    private void Initialize() {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnStartBossEvent += () => ChangeMusic(battleSong);
        GameManager.Instance.OnEndBossEvent += () => {
            if (BossController.Instance.IsDefeated())
                ChangeMusic(playerWinSong);
            else
                ChangeMusic(themeSong);
        };
        GameManager.Instance.OnPlayerDieEvent += () => StopMusic();
        if (!audioSourceMusic.isPlaying && !BossController.Instance.IsDefeated()) StartCoroutine(StartMusicEnumerator());

        audioSourceMusic.volume = OpcoesManager.Instance.GetMusicVolume();
        OpcoesManager.Instance.OnMusicVolumeSliderChanged += (volume) => audioSourceMusic.volume = volume;

        audioSourceEffect.volume = OpcoesManager.Instance.GetSoundEffectVolume();
        OpcoesManager.Instance.OnSoundEffectVolumeSliderChanged += (volume) => audioSourceEffect.volume = volume;
    }

    public void PlaySound(AudioClip clip) {
        audioSourceEffect.clip = clip;
        audioSourceEffect.Play();
    }

    public void PlaySound(AudioClip[] clips) {
        audioSourceEffect.clip = clips[Random.Range(0, clips.Length)];
        audioSourceEffect.Play();
    }

    public void ChangeMusic(AudioClip newMusic) {
        if (audioSourceMusic == null) return;

        StartCoroutine(ChangeMusicEnumerator(newMusic));
    }

    public void StopMusic() {
        if (audioSourceMusic == null) return;
        StartCoroutine(StopMusicEnumerator());
    }

    private IEnumerator ChangeMusicEnumerator(AudioClip newMusic) {
        while(audioSourceMusic.volume > 0.01f) {
            audioSourceMusic.volume -= Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = 0f;

        audioSourceMusic.clip = newMusic;

        yield return new WaitForSeconds(0.6f);

        audioSourceMusic.Play();

        float volume = OpcoesManager.Instance.GetMusicVolume();

        while(audioSourceMusic.volume < volume) {
            audioSourceMusic.volume += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = volume;
    }

    private IEnumerator StopMusicEnumerator() {
        while(audioSourceMusic.volume > 0.01f) {
            audioSourceMusic.volume -= Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = 0.0f;
        audioSourceMusic.Stop();
    }

    private IEnumerator StartMusicEnumerator() {
        audioSourceMusic.clip = themeSong;
        audioSourceMusic.Play();

        float volume = OpcoesManager.Instance.GetMusicVolume();

        while(audioSourceMusic.volume < volume) {
            audioSourceMusic.volume += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = volume;
    }

    public void PauseMusic() {
        audioSourceMusic.Pause();
    }

    public void ResumeMusic() {
        if (!audioSourceMusic.isPlaying)
            audioSourceMusic.Play();
    }

    public void PlayButtonSound() {
        PlaySound(buttonSound);
    }
}
