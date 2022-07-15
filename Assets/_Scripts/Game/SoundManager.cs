using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSourceEffect = null;
    [SerializeField] private AudioSource audioSourceMusic = null;

    [SerializeField] private AudioClip themeSong = null;
    [SerializeField] private AudioClip battleSong = null;

    private void Start() {
        GameManager.Instance.OnStartBossEvent += () => ChangeMusic(battleSong);
        GameManager.Instance.OnEndBossEvent += () => ChangeMusic(themeSong);
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
        StartCoroutine(ChangeMusicEnumerator(newMusic));
    }

    private IEnumerator ChangeMusicEnumerator(AudioClip newMusic) {
        float initialVolume = audioSourceMusic.volume;

        while(audioSourceMusic.volume > 0.01f) {
            audioSourceMusic.volume -= Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = 0f;

        audioSourceMusic.clip = newMusic;

        yield return new WaitForSeconds(0.6f);

        audioSourceMusic.Play();

        while(audioSourceMusic.volume < initialVolume) {
            audioSourceMusic.volume += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = initialVolume;
    }
}
