using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Transform scoreScreen, questionScreen;
    [SerializeField]
    private AudioClip correctSound, incorrectSound;
    private AudioSource source;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreStats, scorePercentage;

    [Header("Players")]
    public PlayerController players;
    public int playerCount = 1;


    private int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        level = PlayerPrefs.GetInt("level", 0);
        SpawnPlayer();
        LoadQuestionSet();
        UseQuestionTemplate(currentQuestion.questionType);
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
    public void NextQuestionSet()
    {
        if(level < questionDatabase.questionSets.Length - 1)
        {
            correctAnswers = 0;
            currentQuestionIndex = 0;
            level++;
            PlayerPrefs.SetInt("level", level);
            scoreScreen.gameObject.SetActive(false);
            questionScreen.gameObject.SetActive(true);
            LoadQuestionSet();
            UseQuestionTemplate(currentQuestion.questionType);
        }
        else
        {
            // load start menu
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        }
    }

    void NextQuestion()
    {
        if (currentQuestionIndex < currentQuestionSet.questions.Count-1)
        {
            currentQuestionIndex++;
            currentQuestion = currentQuestionSet.questions[currentQuestionIndex];
            UseQuestionTemplate(currentQuestion.questionType);
        }
        else
        {
            // do score screen
            scoreScreen.gameObject.SetActive(true);
            questionScreen.gameObject.SetActive(false);
            scorePercentage.text = string.Format("Score: {0}%", (float)correctAnswers / (float)currentQuestionSet.questions.Count * 100);
            scoreStats.text = string.Format("Questions: {0}\nCorrect: {1}", currentQuestionSet.questions.Count, correctAnswers);
        }
    }

    public void CheckAnswer(string answer)
    {
        if (answer == currentQuestion.correctAnswerKey)
        {
            correctAnswers++;
            source.PlayOneShot(correctSound);
            Debug.Log("Correct");
        } else
        {
            source.PlayOneShot(incorrectSound);
            players.TakeDamage(1);
        }

        // next question
        ClearAnswers();
        NextQuestion();
        // reset answer options
    }
}
