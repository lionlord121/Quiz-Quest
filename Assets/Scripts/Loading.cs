using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public string sceneToLoad;
    AsyncOperation loadingOperation;
    public Image loadingBar;
    public TMPro.TextMeshProUGUI percentLoaded;
    void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
    }
    void Update()
    {
        float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        loadingBar.fillAmount = progressValue;
    }
}
