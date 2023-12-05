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

    [SerializeField] private float respawnTime;
    [SerializeField] private Transform transform;


    public bool instantSpawn = false;
    // Start is called before the first frame update
    private void Awake()
    {
        respawnTime = 20f;
    }
    void Start()
    {
        transform = GetComponent<Transform>();
        //SpawnEnemies();
    }
    private void FixedUpdate()
    {
        if(instantSpawn)
        {
            InstantSpawn();
        }
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

                switch (spawnMode)
                {
                    case (1):
                        {
                            Instantiate(enemyObjects[0], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                            break;
                        }
                    case (2):
                        {
                            Instantiate(enemyObjects[1], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                            break;
                        }
                    case (3):
                        {
                            Instantiate(enemyObjects[2], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                            break;
                        }
                    case (4):
                        {
                            int theSelection = Random.Range(0, 3);
                            Instantiate(enemyObjects[theSelection], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
            }
            hasSpawned = true;
            StartCoroutine(RefreshSpawn());
        }
    }
    void InstantSpawn()
    {
        instantSpawn = false;
        float newX = Random.Range(-radius, radius);
        //float newY = Random.Range(-radius, radius);
        float newZ = Random.Range(-radius, radius);
        switch (spawnMode)
        {
            case (1):
                {
                    Instantiate(enemyObjects[0], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                    break;
                }
            case (2):
                {
                    Instantiate(enemyObjects[1], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                    break;
                }
            case (3):
                {
                    Instantiate(enemyObjects[2], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                    break;
                }
            case (4):
                {
                    int theSelection = Random.Range(0, 3);
                    Instantiate(enemyObjects[theSelection], new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ), Quaternion.identity);
                    break;
                }
        }
    }

    IEnumerator RefreshSpawn()
    {
        yield return new WaitForSeconds(respawnTime);

        hasSpawned = false;
    }
}
