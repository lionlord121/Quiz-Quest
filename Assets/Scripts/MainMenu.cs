using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Toggle timerToggle;
    private AsyncOperation asyncOperation;

    void Start()
    {
        timerToggle.isOn = GamePrefs.timerOn;
        StartCoroutine(LoadScene());
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Question");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene("Character Select");
        GamePrefs.triviaMode = false;
    }

    public void TriviaMode()
    {
        PlayerPrefs.SetInt("level", 0);
        GamePrefs.characterOneId = PlayerController.Character.Mage;
        GamePrefs.timerOn = false;
        GamePrefs.triviaMode = true;
        asyncOperation.allowSceneActivation = true;
    }

    public void ToggleTimer(bool val)
    {
       GamePrefs.timerOn = val;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.2f);

        //Begin to load the Scene you specify
        asyncOperation = SceneManager.LoadSceneAsync("Question");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        //while (!asyncOperation.isDone)
        //{
        //    yield return null;
        //}
    }

}
