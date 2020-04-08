using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Tooltip("In seconds")]
    public float fadeInDuration = 1;
    [Tooltip("The amount of times the sprite will flash before dying")]
    public int deathFlashes = 3;
    [Tooltip("The length of each flash")]
    public float flashDelayLength = 0.2f;

    public float handDist = 1.5f;
    [Tooltip("The speed that enemies move their weapons to face the player")]
    public float aimSpeed = 0.7f;
    [Tooltip("Time in seconds between enemy shots")]
    public float fireDelay = 3;

    public GameObject hand;
    GameObject player;

    public AudioSource impactSound;

    // Start is called before the first frame update
    void Start()
    {
        impactSound = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        GetComponent<Renderer>().enabled = true;
        StartCoroutine(FadeIn(GetComponent<SpriteRenderer>(), fadeInDuration));
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = GameManager.player.transform.position;
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        hand.transform.rotation = Quaternion.Lerp(hand.transform.rotation, Quaternion.Euler(0, 0, AngleDeg), aimSpeed);
        hand.transform.position = transform.position + hand.transform.TransformDirection(Vector2.right) * handDist;
    }

    public void Die()
    {
        impactSound.Play();
        StartCoroutine(DeathSequence());
    }

    public void Shoot()
    {
        GameObject bullet = GameManager.GetBullet();
        bullet.transform.position = hand.transform.position;
        bullet.transform.rotation = hand.transform.rotation;
        bullet.GetComponent<BulletController>().isEnemyBullet = true;
        bullet.SetActive(true);
    }

    //IEnumerators run in the background, and can continue to run while other proccesses (like update) execute. The "WaitForSeconds()" method is only useable within an IEnumerator
    public IEnumerator DeathSequence()
    {
        Renderer renderer = GetComponent<Renderer>();
        for(int i = 0; i < deathFlashes; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(flashDelayLength);
            renderer.enabled = true;
            yield return new WaitForSeconds(flashDelayLength);
        }
        GameManager.enemyPool.Add(gameObject);
        gameObject.SetActive(false);
    }

    IEnumerator FadeIn(SpriteRenderer myRenderer, float secondsDuration)
    {
        float counter = 0;
        //Get current color
        Color spriteColor = myRenderer.material.color;

        while (counter < secondsDuration)
        {
            counter += Time.deltaTime;
            //Fade from 0 to 1
            float alpha = Mathf.Lerp(0, 1, counter / secondsDuration);
            //Change alpha only
            myRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }

    IEnumerator Attack()
    {
        while(isActiveAndEnabled)
        {
            yield return new WaitForSeconds(fireDelay);
            Shoot();
        }
    }
}
