﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //VARIABLES
    //annoyingly static variables don't show up in the inspector so you have to change the values in code
    public Sprite enemyBulletTexture;
    public Sprite playerBulletTexture;
    public static Sprite enemyBulletTex;
    public static Sprite playerBulletTex;

    public Canvas UICanvas;
    public static Canvas UserInterface;
    
    public static int maxBullets = 100;
    static GameObject[] playerBullets = new GameObject[maxBullets];
    public GameObject playerBullet;

    static int bulletsFired = 0;

    public static int maxEnemies = 5;
    static GameObject[] enemies = new GameObject[maxEnemies];
    public static List<GameObject> enemyPool = new List<GameObject>();
    public GameObject enemy;
    public float enemySpawnDelay = 1f;
    public static GameObject player;

    public Transform[] enemySpawnPoints;

    //METHODS
    void Start()
    {
        UserInterface = UICanvas;
        enemyBulletTex = enemyBulletTexture;
        playerBulletTex = playerBulletTexture;

        player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < maxBullets; i++)
        {
            playerBullets[i] = GameObject.Instantiate(playerBullet, transform);
            playerBullets[i].SetActive(false);
        }
        for(int i = 0; i < maxEnemies; i++)
        {
            enemies[i] = GameObject.Instantiate(enemy, transform);
            enemies[i].SetActive(false);
            enemyPool.Add(enemies[i]);
        }

        StartCoroutine(SpawnEnemies());
    }

    public static void DrawHealthUI()
    {
        Transform healthBar = GameObject.FindGameObjectWithTag("HealthBar").transform;
        foreach (Transform child in healthBar)
            Destroy(child);
        for(int i = 0; i < player.GetComponent<PlayerController>().health; i++)
        {
            //draw heart images as children of the healthbar - only draw as many as the player has health, obviously
        }
    }

    public static GameObject GetBullet()
    {
        bulletsFired++;
        return playerBullets[(bulletsFired-1) % maxBullets];
    }

    void Update()
    {
        
    }

    public IEnumerator SpawnEnemies()
    {
        int spawns = 0;
        if (enemySpawnPoints.Length == 0)
        {
            Debug.LogError("GameManager: No spawn points created. Please add some transforms to the EnemySpawnPoints array!");
        }
        else
        {
            while (true)
            {
                int activeEnemies = maxEnemies - enemyPool.Count;
                if (enemyPool.Count > 0)
                {
                    if (activeEnemies > 0)
                        yield return new WaitForSeconds(enemySpawnDelay);
                    GameObject enemyInstance = enemyPool[0];
                    enemyPool.RemoveAt(0);
                    enemyInstance.transform.position = enemySpawnPoints[spawns % enemySpawnPoints.Length].position;
                    enemyInstance.transform.rotation = enemySpawnPoints[spawns % enemySpawnPoints.Length].rotation;
                    enemyInstance.SetActive(true);
                    spawns++;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
