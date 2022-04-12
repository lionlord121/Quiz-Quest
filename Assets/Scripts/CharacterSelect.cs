using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    public void SelectMage()
    {

    }
}
