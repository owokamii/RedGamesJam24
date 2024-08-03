using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public GameObject[] spawnPoints;
    public Vector3 spawnOffset;
    private bool[] hasSpawned;

    void Start()
    {
        hasSpawned = new bool[spawnPoints.Length];
        for (int i = 0; i < hasSpawned.Length; i++)
        {
            hasSpawned[i] = false;
        }
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            List<int> availablePoints = new List<int>();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (!hasSpawned[i])
                {
                    availablePoints.Add(i);
                }
            }

            if (availablePoints.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePoints.Count);
                int spawnIndex = availablePoints[randomIndex];

                int prefabIndex = Random.Range(0, prefabsToSpawn.Length);
                GameObject prefabToSpawn = prefabsToSpawn[prefabIndex];

                Vector3 spawnPosition = spawnPoints[spawnIndex].transform.position + spawnOffset;
                GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                spawnedObject.name = prefabToSpawn.name;

                hasSpawned[spawnIndex] = true;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResetSpawnPoint(int index)
    {
        //Debug.Log("ResetSpwanpoint");
        if (index >= 0 && index < hasSpawned.Length)
        {
            hasSpawned[index] = false;
            //Debug.Log("Reset spawn point at index: " + index);
        }
    }
}
