using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public enum Enemy { 
        Assassin = 0, 
        Cultist = 1,
        Darkreaper = 2,
        Drow = 3,
        Frogman = 4,
        Ghost = 5,
        GiantSpider = 6,
        Gnoll = 7,
        Goblin = 8,
        Hobgoblin = 10,
        Icegolem = 11,
        Imp = 12,
        Lizardman = 13,
        Magmagolem = 14,
        Ogre = 15,
        Ruffian = 16,
        Scout = 17,
        SkelWar = 18,
        Slime = 19,
        Stonegolem = 20,
        Swashbuckler = 21,
        Troll = 22,
        Tyrant = 23,
        Wererat = 24,
        Zombie = 25,
    };

    [Header("Info")]
    public EnemyInfo enemyInfo;
    public int health = 3;
    public Enemy enemy = Enemy.Tyrant;
    public bool dead;
    public Animator anim;
    public Image enemyIcon;
    public string enemyName;

    [Header("Enemy Sprites")]
    public Sprite spriteAssassin;
    public Sprite spriteCultist;
    public Sprite spriteDarkreaper;
    public Sprite spriteDrow;
    public Sprite spriteFrogman;
    public Sprite spriteGhost;
    public Sprite spriteGiantSpider;
    public Sprite spriteGnoll;
    public Sprite spriteGoblin;
    public Sprite spriteHobgoblin;
    public Sprite spriteIcegolem;
    public Sprite spriteImp;
    public Sprite spriteLizardman;
    public Sprite spriteMagmagolem;
    public Sprite spriteOgre;
    public Sprite spriteRuffian;
    public Sprite spriteScout;
    public Sprite spriteSkelWar;
    public Sprite spriteSlime;
    public Sprite spriteStonegolem;
    public Sprite spriteSwashbuckler;
    public Sprite spriteTroll;
    public Sprite spriteTyrant;
    public Sprite spriteWererat;
    public Sprite spriteZombie;

    [Header("SFX")]
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip blockSound;
    private AudioSource source;

    public static EnemyController me;

    public void Initialize(int enemyId)
    {
        dead = false;
        enemy = (Enemy)enemyId;
        switch (enemy)
        {
            case Enemy.Assassin:
                health = 4;
                enemyIcon.sprite = spriteAssassin;
                enemyName = "Assassin";
                break;
            case Enemy.Cultist:
                health = 3;
                enemyIcon.sprite = spriteCultist;
                enemyName = "Cultist";
                break;
            case Enemy.Darkreaper:
                health = 5;
                enemyIcon.sprite = spriteDarkreaper;
                enemyName = "Darkreaper";
                break;
            case Enemy.Drow:
                health = 3;
                enemyIcon.sprite = spriteDrow;
                enemyName = "Drow Ranger";
                break;
            case Enemy.Frogman:
                health = 3;
                enemyIcon.sprite = spriteFrogman;
                enemyName = "Frogman";
                break;
            case Enemy.Ghost:
                health = 2;
                enemyIcon.sprite = spriteGhost;
                enemyName = "Ghost";
                break;
            case Enemy.GiantSpider:
                health = 2;
                enemyIcon.sprite = spriteGiantSpider;
                enemyName = "Giant Spider";
                break;
            case Enemy.Gnoll:
                health = 3;
                enemyIcon.sprite = spriteGnoll;
                enemyName = "Gnoll";
                break;
            case Enemy.Goblin:
                health = 3;
                enemyIcon.sprite = spriteGoblin;
                enemyName = "Goblin";
                break;
            case Enemy.Hobgoblin:
                health = 4;
                enemyIcon.sprite = spriteHobgoblin;
                enemyName = "Hobgolbin";
                break;
            case Enemy.Icegolem:
                health = 6;
                enemyIcon.sprite = spriteIcegolem;
                enemyName = "Ice Golem";
                break;
            case Enemy.Imp:
                health = 4;
                enemyIcon.sprite = spriteImp;
                enemyName = "Imp";
                break;
            case Enemy.Lizardman:
                health = 4;
                enemyIcon.sprite = spriteLizardman;
                enemyName = "Lizardman";
                break;
            case Enemy.Magmagolem:
                health = 7;
                enemyIcon.sprite = spriteMagmagolem;
                enemyName = "Magma Golem";
                break;
            case Enemy.Ogre:
                health = 8;
                enemyIcon.sprite = spriteOgre;
                enemyName = "Ogre";
                break;
            case Enemy.Ruffian:
                health = 3;
                enemyIcon.sprite = spriteRuffian;
                enemyName = "Ruffian";
                break;
            case Enemy.Scout:
                health = 2;
                enemyIcon.sprite = spriteScout;
                enemyName = "Scout";
                break;
            case Enemy.SkelWar:
                health = 2;
                enemyIcon.sprite = spriteSkelWar;
                enemyName = "Skeleton Warrior";
                break;
            case Enemy.Slime:
                health = 1;
                enemyIcon.sprite = spriteSlime;
                enemyName = "Slime";
                break;
            case Enemy.Stonegolem:
                health = 7;
                enemyIcon.sprite = spriteStonegolem;
                enemyName = "Stone Golem";
                break;
            case Enemy.Swashbuckler:
                health = 4;
                enemyIcon.sprite = spriteSwashbuckler;
                enemyName = "Swashbuckler";
                break;
            case Enemy.Troll:
                health = 7;
                enemyIcon.sprite = spriteTroll;
                enemyName = "Troll";
                break;
            case Enemy.Tyrant:
                health = 8;
                enemyIcon.sprite = spriteTyrant;
                enemyName = "Tyrant";
                break;
            case Enemy.Wererat:
                health = 2;
                enemyIcon.sprite = spriteWererat;
                enemyName = "Wererat";
                break;
            case Enemy.Zombie:
                health = 3;
                enemyIcon.sprite = spriteZombie;
                enemyName = "Zombie";
                break;
            default:
                health = 1;
                enemyIcon.sprite = spriteSlime;
                enemyName = "Slime";
                break;
        }
        enemyInfo.Initialize(health, 0);
        source = GetComponent<AudioSource>();
    }

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
        health -= damage;

        // update the health bar
        enemyInfo.SetHealth(health);

        if (health <= 0)
            Die();
        else
        {
            GetsHurt();
        }
    }

    void GetsHurt()
    {
        StartCoroutine(DamageFlash());

        IEnumerator DamageFlash()
        {
            enemyIcon.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            enemyIcon.color = Color.white;
        }
    }

    void Die()
    {
        dead = true;
    }
}
