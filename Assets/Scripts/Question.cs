using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public enum QuestionType { Text = 0, ImageWithCaption = 1, Audio = 2 };
    public QuestionType questionType;
    public string questionText;
    public Sprite questionImage;
    public AudioClip questionAudio;
    public string correctAnswerKey;
    public string[] answerChoices;
}
