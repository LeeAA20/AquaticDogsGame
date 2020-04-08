using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Setting this to true makes bullets push the player back when they're fired")]
    public bool enableBulletRecoil = true;
    [Tooltip("Basic movement is just a simple transform.translate. Set this to false to use a physics based movement system")]
    public bool useBasicMovement = false;

    public float turnSpeed = 1;
    public float moveSpeed = 1;
    public float handDist = 1.5f;

    public float recoilForce = 0.5f;

    public int health = 5;

    public GameObject hand;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        if (useBasicMovement)
        {
            transform.Translate(new Vector2(xAxis, yAxis) * moveSpeed);
        }
        else
        {
            rb.AddForce(new Vector2(xAxis, yAxis) * moveSpeed);
            if (xAxis == 0 && yAxis == 0)
                rb.AddForce(rb.velocity * -moveSpeed);
        }

        Vector2 mousePos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(100);

        //Make the cursor follow the mouse but stay locked in a ring around the player
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        hand.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
        hand.transform.position = transform.position + hand.transform.TransformDirection(Vector2.right) * handDist;

        if (Input.GetButtonDown("Fire1"))
            shoot();
    }

    public void shoot()
    {
        GameObject bullet = GameManager.GetBullet();
        bullet.transform.position = hand.transform.position;
        bullet.transform.rotation = hand.transform.rotation;
        bullet.SetActive(true);
        if(enableBulletRecoil)
            rb.AddForce(-bullet.transform.right * recoilForce, ForceMode2D.Impulse);
    }

    public void GotHit()
    {
        health--;
        if (health <= 0)
            GameOver();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        UnityEditor.EditorApplication.isPaused = true;
    }
}
