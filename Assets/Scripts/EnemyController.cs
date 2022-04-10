using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Enemy { Tyrant = 0 };

    [Header("Info")]
    public EnemyInfo enemyInfo;
    public int health = 3;
    public Enemy enemy = Enemy.Tyrant;
    public bool dead;
    public Animator anim;

    [Header("SFX")]
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip blockSound;
    private AudioSource source;

    public static EnemyController me;


    public void Initialize()
    {
        if (enemy == Enemy.Tyrant)
        {
            health = 3;
        }
        enemyInfo.Initialize(health, 0);
        source = GetComponent<AudioSource>();
    }

    //private void Update() {
    //    playerInfo.Initialize(health, 0);
    //}

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
            getsHurt();
        }
    }

    void getsHurt()
    {
        //StartCoroutine(DamageFlash());

        //IEnumerator DamageFlash()
        //{
        //sr.color = Color.red;
        //yield return new WaitForSeconds(0.05f);
        //sr.color = Color.white;
        //}
    }

    void Die()
    {
        dead = true;
        //rig.isKinematic = true;

        //transform.position = new Vector3(0, 99, 0);

        //Vector3 spawnPos = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)].position;

        //StartCoroutine(Spawn(spawnPos, GameManager.instance.respawnTime));
    }
}
