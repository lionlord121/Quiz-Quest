using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public QuestionDatabase questionDatabase;

    private int level;
    private QuestionSet currentQuestionSet;
    private Question currentQuestion;
    private int currentQuestionIndex;
    [SerializeField]
    private Transform questionPanel;
    [SerializeField]
    private Transform answerPanel;
    [SerializeField]
    private GameObject timerBar;
    [SerializeField]
    private Image timerBarCurrent;
    private float timeLeft = 10.0f;
    private float maxTimer = 10.0f;

    [SerializeField]
    private Transform scoreScreen, questionScreen, victoryScreen;
    [SerializeField]
    private AudioClip correctSound, incorrectSound;
    private AudioSource backgroundSource, soundSource;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreStats, scorePercentage;

    [Header("Players")]
    public PlayerController players;
    public int playerCount = 1;

    private int correctAnswers = 0;
    private int totalQuestions = 1;

    // Start is called before the first frame update
    void Start()
    {
        backgroundSource = GetComponent<AudioSource>();
        soundSource = GetComponent<AudioSource>();
        level = PlayerPrefs.GetInt("level", 0);
        SetGamePrefs();
        SpawnPlayer();
        questionDatabase.getAPISession();
        questionDatabase.setCatagories();
        LoadQuestionSet();
        UseQuestionTemplate(currentQuestion.questionType);
    }

    private void SetGamePrefs()
    {
        if (!GamePrefs.timerOn)
        {
            timerBar.SetActive(false);
        }
    }

    private void Update()
    {
        if (!players.dead && timerBar.activeSelf)
        {
            timeLeft -= Time.deltaTime;
            timerBarCurrent.fillAmount = (float)timeLeft / maxTimer;
            if (timeLeft < 0)
            {
                // Send a blank answer
                CheckAnswer("");
            }
        }
    }

    void SpawnPlayer()
    {
        //playerPrefabPath = PhotonNetwork.LocalPlayer.CustomProperties["character"].ToString();
        //GameObject playerObject = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        // initialize player
        //playerObject.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);

        //playerCount
        players.Initialize();
    }

    void LoadQuestionSet()
    {
        currentQuestionSet = questionDatabase.GetQuestionSet(level);
        currentQuestion = currentQuestionSet.questions[0];
    }

    void ClearAnswers()
    {
        foreach(Transform buttons in answerPanel)
        {
            Destroy(buttons.gameObject);
        }
    }

    void UseQuestionTemplate(Question.QuestionType questionType)
    {
        for (int i = 0; i < questionPanel.childCount; i++)
        {
            questionPanel.GetChild(i).gameObject.SetActive(i == (int)questionType);
            if ( i == (int)questionType)
            {
                GameObject template = questionPanel.GetChild(i).gameObject;
                template.GetComponent<QuestionUI>().UpdateQuestionInfo(currentQuestion);
            }
        }
    }
    //public void NextQuestionSet()
    //{
    //    if(level < questionDatabase.questionSets.Length - 1)
    //    {
    //        correctAnswers = 0;
    //        currentQuestionIndex = 0;
    //        level++;
    //        PlayerPrefs.SetInt("level", level);
    //        scoreScreen.gameObject.SetActive(false);
    //        questionScreen.gameObject.SetActive(true);
    //        LoadQuestionSet();
    //        UseQuestionTemplate(currentQuestion.questionType);
    //    }
    //    else
    //    {
    //        LoadQuestionSet();
    //        // load start menu
    //        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    //    }
    //}

    public void ReturnToMainMenu()
    {
        // load start menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    void NextQuestion()
    {
        totalQuestions++;
        timeLeft = maxTimer;
        if (currentQuestionIndex < currentQuestionSet.questions.Count-1)
        {
            currentQuestionIndex++;
            currentQuestion = currentQuestionSet.questions[currentQuestionIndex];
            UseQuestionTemplate(currentQuestion.questionType);
        }
        else
        {
            LoadQuestionSet();
            currentQuestionIndex = 0;
            currentQuestion = currentQuestionSet.questions[currentQuestionIndex];
            UseQuestionTemplate(currentQuestion.questionType);
        }
    }

    private void ChangeVolume()
    {
        backgroundSource.volume = 1f;
    }

    public void CheckAnswer(string answer)
    {
        backgroundSource.volume = 0.25f;
        Invoke("ChangeVolume", 2);
        if (answer == currentQuestion.correctAnswerKey)
        {
            correctAnswers++;
            soundSource.PlayOneShot(correctSound);

            players.GainScore(1 + (int)timeLeft);
            Debug.Log("Correct");
        } else
        {
            soundSource.PlayOneShot(incorrectSound);
            players.TakeDamage(1);
        }

        if (players.dead)
        {
            // do score screen
            scoreScreen.gameObject.SetActive(true);
            questionScreen.gameObject.SetActive(false);
            scorePercentage.text = string.Format("Score: {0}%", Mathf.Floor((float)correctAnswers / (float)totalQuestions * 100));
            scoreStats.text = string.Format("Questions: {0}\nCorrect: {1}", totalQuestions, correctAnswers);
            backgroundSource.Stop();
        } else
        {
            ClearAnswers();
            NextQuestion();
        }
    }
}
