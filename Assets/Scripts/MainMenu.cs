using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle timerToggle;

    void Start()
    {
        timerToggle.isOn = GamePrefs.timerOn;
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Question");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene("Character Select");
    }

    public void ToggleTimer(bool val)
    {
       GamePrefs.timerOn = val;
    }

}
