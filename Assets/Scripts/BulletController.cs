using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float travelSpeed = 0.5f;
    public float explosiveMultiplier = 1;
    public bool isEnemyBullet;

    public AudioSource impactSound;

    // Start is called before the first frame update
    void Start()
    {
        impactSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (isEnemyBullet)
            GetComponent<SpriteRenderer>().sprite = GameManager.enemyBulletTex;
        else
            GetComponent<SpriteRenderer>().sprite = GameManager.playerBulletTex;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * travelSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!isEnemyBullet && col.transform.tag == "Enemy")
        {
            impactSound.Play();
            col.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * explosiveMultiplier, col.contacts[0].point, ForceMode2D.Impulse);
            col.gameObject.GetComponent<EnemyController>().Die();
            gameObject.SetActive(false);
        }
        if(isEnemyBullet && col.transform.tag == "Player")
        {
            GameManager.player.GetComponent<PlayerController>().GotHit();
            gameObject.SetActive(false);
        }
    }
}
