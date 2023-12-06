using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private float radius = 10;
    [SerializeField] private bool randomize = false;
    [SerializeField] private GameObject[] enemyObjects;
    [SerializeField] private bool hasSpawned = false;
    [SerializeField] private int spawnMode;
    private Transform transform;

    [SerializeField] private float respawnTime;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        //SpawnEnemies();
    }
    private void FixedUpdate()
    {
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        if (!hasSpawned)
        {


            //GameObject newEnemy = enemyObject;
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                float newX = Random.Range(-radius, radius);
                //float newY = Random.Range(-radius, radius);
                float newZ = Random.Range(-radius, radius);
                if(spawnMode == 1) //spawns FIRST enemy in the enemyObjects list
                {
                    Instantiate(enemyObjects[0], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                }
                if (spawnMode == 2) //spawns SECOND enemy in the enemyObjects list
                {
                    Instantiate(enemyObjects[1], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                }
                if (spawnMode == 3) //spawns THIRD enemy in the enemyObjects list
                {
                    Instantiate(enemyObjects[2], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                }
                if (spawnMode == 4) //randomly spawns from all enemy types
                {
                    int theSelection = Random.Range(0, 3);
                    Instantiate(enemyObjects[theSelection], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                }
                
            }
            hasSpawned = true;
            StartCoroutine(RefreshSpawn());
        }
    }

    IEnumerator RefreshSpawn()
    {
        yield return new WaitForSeconds(respawnTime);

        hasSpawned = false;
    }
}
