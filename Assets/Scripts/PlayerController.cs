﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum Character { Knight = 0, Mage = 1, Cleric = 2 , Assassin = 3 };

    [Header("Info")]
    public int numCorrect = 0;
    public PlayerInfo playerInfo;
    public int health = 6;
    public int maxHealth = 6;
    public int score;
    public Character character = Character.Knight;
    public Image characterSprite;
    public bool dead;
    public Animator anim;
    private int scoreForHealth;

    [Header("Character Sprites")]
    public Sprite knightSprite;
    public Sprite mageSprite;
    public Sprite clericSprite;
    public Sprite assassinSprite;


    [Header("SFX")]
    public AudioClip attackSound;
    public AudioClip hurtSound, blockSound, healSound;
    private AudioSource source;

    // local player
    public static PlayerController me;

    public void Initialize(Character character, int currentHp)
    {
        GetCharacterSelection(character);
        playerInfo.Initialize(health, 0);
        source = GetComponent<AudioSource>();
    }

    public void InitializeCurrent(Character character, int currentHp)
    {
        health = currentHp;
        GetCharacterSelection(character);
        playerInfo.Initialize(health, 0);
        source = GetComponent<AudioSource>();
    }

    private void GetCharacterSelection(Character character)
    {
        this.character = character;
        switch (character)
        {
            case Character.Knight:
                characterSprite.sprite = knightSprite;
                characterSprite.rectTransform.sizeDelta = new Vector2(235, 223);

                break;
            case Character.Mage:
                characterSprite.sprite = mageSprite;
                characterSprite.rectTransform.sizeDelta = new Vector2(171, 223);

                break;
            case Character.Cleric:
                characterSprite.sprite = clericSprite;
                characterSprite.rectTransform.sizeDelta = new Vector2(91, 228);
                break;
            case Character.Assassin:
                characterSprite.sprite = assassinSprite;
                characterSprite.rectTransform.sizeDelta = new Vector2(176, 222);
                break;
            default:
                characterSprite.sprite = knightSprite;
                characterSprite.rectTransform.sizeDelta = new Vector2(235, 223);
                break;
        }
    }
    //private void Update() {
    //    playerInfo.Initialize(health, 0);
    //}

    //void Move()
    //{
    //    // get horizontal and vertical inputs
    //    float x = Input.GetAxis("Horizontal");
    //    float y = Input.GetAxis("Vertical");

    //    // apply that to our velocity
    //    rig.velocity = new Vector2(x, y) * moveSpeed;

    //    // applies correct moving animation
    //    photonView.RPC("MovingAnimation", RpcTarget.All, x, y);
    //}

    void MovingAnimation(float x, float y)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            if (x == 0 && y == 0)
                anim.SetBool("moving", false);
            else
            {
                anim.SetBool("moving", true);
                if (x > 0)
                    anim.SetInteger("direction", 2);
                if (x < 0)
                    anim.SetInteger("direction", 4);
                if (y > 0)
                    anim.SetInteger("direction", 1);
                if (y < 0)
                    anim.SetInteger("direction", 3);
            }
        }
    }

    // melee attacks towards the mouse
    void Attack()
    {
    }

    void AttackAnimation()
    {
        anim.SetTrigger("attacking");
        source.PlayOneShot(attackSound);
    }

    public void TakeDamage(int damage)
    {
        if(character == Character.Knight)
        {
            System.Random random = new System.Random();
            if (random.Next(1, 5) == 1)
            {
                source.PlayOneShot(blockSound);
                damage = 0;
            }
        }
        health -= damage;

        if (damage != 0)
        {
            // update the health bar
            playerInfo.SetHealth(health);

            if (health <= 0)
                Die();
            else
            {
                GetsHurt();
            }
        }
    }

    void GetsHurt()
    {
        StartCoroutine(DamageFlash());

        IEnumerator DamageFlash()
        {
            characterSprite.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            characterSprite.color = Color.white;
        }
    }

    void Die()
    {
        dead = true;
        //rig.isKinematic = true;

        //transform.position = new Vector3(0, 99, 0);

        //Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;

        //StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }

    IEnumerator Spawn (Vector3 spawnPos, float timeToSpawn)
    {
        yield return new WaitForSeconds(timeToSpawn);

        dead = false;
        transform.position = spawnPos;
        //health = maxHp;

        // update health bar
    }

    void Heal (int amountToHeal)
    {
        health = Mathf.Clamp(health + amountToHeal, 0, maxHealth);
        source.PlayOneShot(healSound);
        // update the health bar
        playerInfo.SetHealth(health);
    }

    public void GainScore(int scoreToGive)
    {
        // update the current score
        score += scoreToGive;
        scoreForHealth += scoreToGive;
        // heal at every 50 points
        if (scoreForHealth >= 50)
        {
            scoreForHealth -= 50;
            Heal(1);
        }
        // update the ui
        playerInfo.UpdateScore(scoreToGive);
    }
}
