using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [SerializeField] private Image transitionImage = null;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        DOTween.Init();
    }

    public void LoadScene(string scene) {
        transitionImage.DOFade(1.0f, 1.0f).From(0.0f).OnComplete(() => {
            StartCoroutine(LoadSceneAsync(scene));
        });

        //StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(string scene) {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone) {
            Debug.Log("Progress: " + ao.progress * 100 + "%");

            if (ao.progress >= 0.9f) {
                ao.allowSceneActivation = true;
                transitionImage.DOFade(0.0f, 1.0f).From(1.0f);
            }

            yield return null;
        }
    }
}
