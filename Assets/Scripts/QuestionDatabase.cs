using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Web;

[CreateAssetMenu(fileName = "Questions", menuName = "Create Questions", order = 100)]
public class QuestionDatabase : ScriptableObject
{
    public JSONNode jsonResult;
    public QuestionSet[] questionSets;
    public Dictionary<string, int> questionCatagories = new Dictionary<string, int>();
    private string token = "";

    public QuestionSet GetQuestionSet(int level)
    {
        return GetJSONData(3, 0);
        //foreach (QuestionSet questionSet in questionSets)
        //{
        //    if (questionSet.level == level)
        //    {
        //        return questionSet;
        //    }
        //}
        //return new QuestionSet();
    }

    public void getAPISession()
    {
        if(token == "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_token.php?command=request");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string json = reader.ReadToEnd();

            JSONNode data = JSON.Parse(json);

            token = data["token"].Value;
        }
    }

    public void setCatagories()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api_category.php");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());

        string json = reader.ReadToEnd();

        JSONNode data = JSON.Parse(json);
        foreach (var catagory in data["trivia_categories"])
        {
            questionCatagories.Add(catagory.Value["name"], catagory.Value["id"]);
        }
    }

    public QuestionSet GetJSONData(int questionAmount, int category)
    {
        HttpWebRequest request = category > 0 == true ? 
            (HttpWebRequest)WebRequest.Create(string.Format("https://opentdb.com/api.php?amount={0}&category={1}&type=multiple&token={2}", questionAmount, category, token))
            : (HttpWebRequest)WebRequest.Create(string.Format("https://opentdb.com/api.php?amount={0}&type=multiple&token={1}", questionAmount, token));


        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());

        string json = reader.ReadToEnd();


        JSONNode data = JSON.Parse(json);

        QuestionSet set = new QuestionSet();
        set.questions = new List<Question>();
        for (int i = 0; i < data["results"].Count; i++)
        {
            Result result = BuildResult(data["results"][i]);
            set.questions.Add(ResultToQuestion(result));
        }

        return set;
    }

    public Result BuildResult(JSONNode node)
    {
        Result normal = new Result();
        normal.category = node["category"].Value;
        normal.type = node["type"].Value;
        normal.difficulty = node["difficulty"].Value;
        normal.question = node["question"].Value;
        normal.correct_answer = node["correct_answer"].Value;
        normal.incorrect_answers = new string[] { node["incorrect_answers"][0].Value, node["incorrect_answers"][1].Value, node["incorrect_answers"][2].Value };
        return normal;
    }
    public Question ResultToQuestion(Result result)
    {
        Question question = new Question();
        question.questionType = Question.QuestionType.Text;
        question.questionText = HttpUtility.HtmlDecode(result.question);
        question.answerChoices = new string[] { 
            HttpUtility.HtmlDecode(result.correct_answer), 
            HttpUtility.HtmlDecode(result.incorrect_answers[0]),
            HttpUtility.HtmlDecode(result.incorrect_answers[1]),
            HttpUtility.HtmlDecode(result.incorrect_answers[2]) };

        question.correctAnswerKey = HttpUtility.HtmlDecode(result.correct_answer);
        return question;
    }
}

[System.Serializable]
public struct QuestionSet
{
    public int level;
    public List<Question> questions;
}

public class Result
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}