using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Game : MonoBehaviour
{
    public QuestionDatabase questionDatabase;

    private int level;
    private QuestionSet currentQuestionSet;
    private Question currentQuestion;
    private int currentQuestionIndex;
    private string currentCatagory ="";

    private float timeLeft = 15.0f;
    private float maxTimer = 15.0f;
    private bool timerActive = true;

    [Header("UI elements")]
    [SerializeField]
    private Image timerBarCurrent;
    [SerializeField]
    private Sprite incorrectAnswerSprite, correctAnswerSprite;
    [SerializeField]
    private Transform questionPanel;
    [SerializeField]
    private Transform answerPanel;
    [SerializeField]
    private GameObject timerBar;
    [SerializeField]
    private Transform defeatScreen, questionScreen, victoryScreen, catagorySelectScreen;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreStats, scorePercentage, scoreFinal;
    [SerializeField]
    private TMPro.TMP_Dropdown catagoryDropdown;

    [Header("Audio")]
    [SerializeField]
    private AudioClip correctSound;
    [SerializeField]
    private AudioClip incorrectSound;
    [SerializeField]
    private AudioClip defeatMusic;
    private AudioSource backgroundSource, soundSource;


    [Header("Players")]
    public PlayerController players;
    //public PlayerController[] players;
    public int playerCount = 1;
    private int correctAnswers = 0;
    private int totalQuestions = 1;

    [Header("Enemy")]
    public EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        backgroundSource = GetComponent<AudioSource>();
        soundSource = GetComponent<AudioSource>();
        level = PlayerPrefs.GetInt("level", 0);
        SetGamePrefs();
        SpawnPlayer();
        SpawnEnemy();
        questionDatabase.GetAPISession();
        if (questionDatabase.questionCatagories.Count == 0)
        {
            questionDatabase.SetCatagories();
            catagoryDropdown.AddOptions(questionDatabase.questionCatagories.Keys.ToList());
        }
        if (players.character == PlayerController.Character.Mage)
        {
            System.Random random = new System.Random();
            if (random.Next(1, 4) == 1)
            {
                players.PlayMageAbilitySound();
                // show catagory screen then load question set
                ShowCatagoryScreen();
            }
            else
            {
                LoadQuestionSet(enemy.health, currentCatagory);
                UseQuestionTemplate(currentQuestion.questionType);

            }
        } else
        {
            LoadQuestionSet(enemy.health, currentCatagory);
            UseQuestionTemplate(currentQuestion.questionType);

        }
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
        if (!players.dead && timerBar.activeSelf && timerActive)
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
        players.Initialize((PlayerController.Character)GamePrefs.characterOneId, 0);
    }

    void SpawnEnemy()
    {
        System.Random rnd = new System.Random();
        int enemyId = rnd.Next(0, Enum.GetValues(typeof(EnemyController.Enemy)).Length);
        enemy.Initialize(enemyId);
    }

    void LoadQuestionSet(int questionCount, string catagoryName)
    {
        currentQuestionSet = questionDatabase.GetQuestionSet(level, questionCount, catagoryName);
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
            // clears out the catagory when new set is loaded 
            currentCatagory = "";
            LoadQuestionSet(enemy.health, currentCatagory);
            currentQuestionIndex = 0;
            currentQuestion = currentQuestionSet.questions[currentQuestionIndex];
            UseQuestionTemplate(currentQuestion.questionType);
        }
    }

    private void ChangeVolume()
    {
        backgroundSource.volume = 1f;
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(3);
        ToggleTimer();
        if (players.dead)
        {
            // do score screen
            defeatScreen.gameObject.SetActive(true);
            questionScreen.gameObject.SetActive(false);
            scorePercentage.text = string.Format("Percent Correct: \n{0}%", Mathf.Floor((float)correctAnswers / (float)totalQuestions * 100));
            scoreStats.text = string.Format("Questions: {0}\nCorrect: {1}", totalQuestions, correctAnswers);
            scoreFinal.text = string.Format("Final Score: \n {0}", players.score);
            PlayDefeatedMusic();
        }
        else
        {
            ClearAnswers();
            NextQuestion();
        }
    }

    private void ShowCatagoryScreen()
    {
        questionScreen.gameObject.SetActive(false);
        catagorySelectScreen.gameObject.SetActive(true);
        timerActive = false;
    }

    public void CatagorySelected()
    {
        questionScreen.gameObject.SetActive(true);
        catagorySelectScreen.gameObject.SetActive(false);
        timerActive = GamePrefs.timerOn && true;
        currentCatagory = catagoryDropdown.options[catagoryDropdown.value].text;
        LoadQuestionSet(enemy.health, currentCatagory);
        UseQuestionTemplate(currentQuestion.questionType);
    }

    public void CheckAnswer(string answer)
    {
        TogglePlayerInput();
        backgroundSource.volume = 0.25f;
        Invoke("ChangeVolume", 2);
        HighlightCorrectAnswer(currentQuestion.correctAnswerKey);
        if (answer == currentQuestion.correctAnswerKey)
        {
            correctAnswers++;
            soundSource.PlayOneShot(correctSound);
            players.GainScore(1 + (int)timeLeft);
            enemy.TakeDamage(1);
            if (enemy.dead)
            {
                // start death animation for enemy

                // for now make a new enemy
                SpawnEnemy();
            }
        }
        else
        {
            if (answer != "")
            {
                HighlightIncorrectAnswer(answer);
            }
            soundSource.PlayOneShot(incorrectSound);
            players.TakeDamage(1);
        }

        StartCoroutine("WaitForAnimation");
        ToggleTimer();
    }

    public void ToggleTimer()
    {
        timerActive = !timerActive;
    }

    public void TogglePlayerInput() 
    {
        FindObjectsOfType<Button>().ToList().ForEach(x => x.interactable = false);
    }

    public void PlayDefeatedMusic()
    {
        backgroundSource.Stop();
        backgroundSource.loop = true;
        backgroundSource.PlayOneShot(defeatMusic);
    }

    public void HighlightCorrectAnswer(string answer)
    {
        // get the button with the correct answer
        try {
            AnswerButton correctAnswerBtn = FindObjectsOfType<AnswerButton>().Where(x => x.GetAnswer() == answer).ToArray()[0];
            correctAnswerBtn.GetComponent<Image>().sprite = correctAnswerSprite;
        } 
        catch (Exception e)
        {
            Debug.Log("uh oh");
        }

    }

    public void HighlightIncorrectAnswer(string answer)
    {
        // get the button with the incorrect answer
        AnswerButton incorrectAnswerBtn = FindObjectsOfType<AnswerButton>().Where(x => x.GetAnswer() == answer).ToArray()[0];
        incorrectAnswerBtn.GetComponent<Image>().sprite = incorrectAnswerSprite;
    }
}
