using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    //[SerializeField] GameObject spitterToSpawn;
    //[SerializeField] GameObject ghoulToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;

    bool startSpawning;
    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numberToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")))
        {
            startSpawning = true;
        }

    }

    IEnumerator spawn()
    {
        isSpawning = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        int enemyIndex = Random.Range(0, enemiesToSpawn.Length);
        GameObject enemyToSpawn = enemiesToSpawn[enemyIndex];
        int spawnInt = Random.Range(0, spawnPos.Length);

        Instantiate(enemyToSpawn, spawnPos[spawnInt].position, spawnPos[spawnInt].rotation);


        isSpawning = false;
    }
}
