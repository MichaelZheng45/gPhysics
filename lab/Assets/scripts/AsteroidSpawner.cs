using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject[] asteroids;
    [SerializeField]
    float spawnTimer;
    [SerializeField]
    float spawnRadius;
    [SerializeField]
    float asteroidSpeed;
    [SerializeField]
    int NumberOfAsteroidsToSpawnPerTick;

    Vector3 playerPos;
    float spawnTimeReset;

    void Start()
    {
        spawnTimeReset = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.Find("ship").GetComponent<particle2D>().position;
        runTimer();
    }

    void runTimer()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            spawnAsteroids();
            spawnTimer = spawnTimeReset;
        }
    }

    void spawnAsteroids()
    {
        for (int i = 0; i < NumberOfAsteroidsToSpawnPerTick; i++)
        {
            float angle = Random.Range(0, 360);
            Vector2 spawnPos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            spawnPos *= spawnRadius;
            spawnPos += (Vector2)playerPos;
            Vector2 velocity = Vector2.MoveTowards(spawnPos, playerPos, 1f);
            GameObject asteroid = GameObject.Instantiate(chooseAsteroid(), spawnPos, Quaternion.identity);
            asteroid.GetComponent<particle2D>().setBase(spawnPos, 0);
            asteroid.GetComponent<particle2D>().AddForce(velocity * asteroidSpeed);
            
        }
    }

    GameObject chooseAsteroid()
    {
        int choice = Random.Range(0, 3);
        return asteroids[choice];
    }
}
