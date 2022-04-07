using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    private Game game;
    private string answer;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => game.CheckAnswer(answer));
    }
    public void SetAnswerButton(string answer)
    {
        this.answer = answer;
        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = answer;
    }

    public string GetAnswer()
    {
        return this.answer;
    }
}
