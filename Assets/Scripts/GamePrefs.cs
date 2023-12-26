using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePrefs
{
    [SerializeField]
    public static bool timerOn = true;
    [SerializeField]
    public static bool triviaMode = false;
    [SerializeField]
    public static float maxTimer = 15.0f;
    [SerializeField]
    public static PlayerController.Character characterOneId = PlayerController.Character.Knight;
    [SerializeField]
    public static PlayerController.Character characterTwoId = PlayerController.Character.Knight;
}
