using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUiObject = null;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ChangeMenuStatus(!menuUiObject.activeSelf);
        }
    }

    public void ChangeMenuStatus(bool active) {
        Time.timeScale = active ? 0.0f : 1.0f;
        menuUiObject.SetActive(active);

        if (active) SoundManager.Instance.PauseMusic();
        else SoundManager.Instance.ResumeMusic();
    }
    
    public void MenuButtonClick() {
        Time.timeScale = 1.0f;
        GameSceneManager.Instance.LoadScene("MenuScene");
    }
}
