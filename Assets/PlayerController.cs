using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 1;
    public float moveSpeed = 1;
    public float handDist = 1.5f;
    
    public GameObject hand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(xAxis, yAxis) * moveSpeed);
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
    }
}
