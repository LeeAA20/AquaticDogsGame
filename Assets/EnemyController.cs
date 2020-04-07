using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int deathFlashes = 3;
    public float flashDelayLength = 0.2f;
    Renderer renderer;

    public AudioSource impactSound;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        impactSound = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        GetComponent<Renderer>().enabled = true;
        renderer = GetComponent<Renderer>();
        renderer.material.color *= new Vector4(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(renderer.enabled == true)
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(renderer.material.color.a, 1, 0.1f));
    }

    public void Die()
    {
        impactSound.Play();
        StartCoroutine(DeathSequence());
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
}
