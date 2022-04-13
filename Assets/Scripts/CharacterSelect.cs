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
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void SelectKnight()
    {
        GamePrefs.characterOneId = PlayerController.Character.Knight;
        SceneManager.LoadScene("Question");
    }

    public void SelectMage()
    {
        GamePrefs.characterOneId = PlayerController.Character.Mage;
        SceneManager.LoadScene("Question");
    }
}
