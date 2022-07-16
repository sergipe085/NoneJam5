using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public void OnPlayClick() {
        GameSceneManager.Instance.LoadScene("GameScene");
    }

    public void OnNewGameClick() {
        PlayerPrefs.SetInt("bossCurrentLevel", 0);
        OnPlayClick();
    }

    public void OnExitClick() {
        Application.Quit();
    }
}
