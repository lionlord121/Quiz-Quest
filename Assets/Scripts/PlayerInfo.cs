using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int currentHealth;
    //public int maxHealth;

    public Image healthIcon;
    public Image[] healthIcons;

    private int score;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreDisplay;

    public void Initialize(int healthVal, int scoreVal)
    {
        //maxHealth = healthVal;
        currentHealth = healthVal;
        score = scoreVal;

        SetHealth(healthVal);
    }

    public void SetHealth(int value)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            //if (i < currentHealth)
            //{
            //    full heart
            //}
            //else
            //{
            //    empty heart
            //}
            if (i < value)
            {
                healthIcons[i].enabled = true;
            }
            else
            {
                healthIcons[i].enabled = false;
            }
        }
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreDisplay.text = string.Format("Score: {0}", score);
    }
}
