using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float travelSpeed = 0.5f;
    public float explosiveMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * travelSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Enemy")
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * explosiveMultiplier, col.contacts[0].point, ForceMode2D.Impulse);
            col.gameObject.GetComponent<EnemyController>().Die();
            gameObject.SetActive(false);
        }
    }
}
