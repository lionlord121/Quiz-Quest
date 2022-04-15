using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private AudioClip selectSound;
    private AudioSource soundSource;
    public Canvas HeroSelecter;
    private AsyncOperation asyncOperation;
    
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        StartCoroutine(LoadScene());
    }

    public void SelectKnight()
    {
        GamePrefs.characterOneId = PlayerController.Character.Knight;
        asyncOperation.allowSceneActivation = true;
    }

    public void SelectMage()
    {
        GamePrefs.characterOneId = PlayerController.Character.Mage;
        asyncOperation.allowSceneActivation = true;
    }

    public void SelectAssassin()
    {
        GamePrefs.characterOneId = PlayerController.Character.Assassin;
        asyncOperation.allowSceneActivation = true;
    }

    public void SelectCleric()
    {
        GamePrefs.characterOneId = PlayerController.Character.Cleric;
        asyncOperation.allowSceneActivation = true;
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
