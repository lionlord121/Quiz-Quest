using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public int currentHealth;

    public Image healthIcon;
    public Image[] healthIcons;

    public void Initialize(int healthVal, int scoreVal)
    {
        currentHealth = healthVal;

        SetHealth(healthVal);
    }

    public void SetHealth(int value)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
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

}
