using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawnLevel1;
    public GameObject[] prefabsToSpawnLevel2;
    public GameObject[] prefabsToSpawnLevel3;

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

                GameObject[] currentPrefabsToSpawn = GetPrefabsForCurrentLevel();
                if (currentPrefabsToSpawn.Length > 0)
                {
                    int prefabIndex = Random.Range(0, currentPrefabsToSpawn.Length);
                    GameObject prefabToSpawn = currentPrefabsToSpawn[prefabIndex];

                    if (spawnPoints[spawnIndex] != null)
                    {
                        Vector3 spawnPosition = spawnPoints[spawnIndex].transform.position + spawnOffset;
                        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                        spawnedObject.name = prefabToSpawn.name;

                        hasSpawned[spawnIndex] = true;
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    GameObject[] GetPrefabsForCurrentLevel()
    {
        int currentLevel = GameManager.Instance.GetCurrentLevel();
        switch (currentLevel)
        {
            case 1:
                return prefabsToSpawnLevel1;
            case 2:
                return prefabsToSpawnLevel2;
            case 3:
                return prefabsToSpawnLevel3;
            default:
                return prefabsToSpawnLevel1;
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
