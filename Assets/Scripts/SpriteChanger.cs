using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] plantSprites;

    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
    private RandomSpawner randomSpawner;

    void Start()
    {
        randomSpawner = FindObjectOfType<RandomSpawner>();
        if (randomSpawner == null)
        {
            Debug.LogError("RandomSpawner not found in the scene!");
        }
        else
        {
            Debug.Log("RandomSpawner found: " + randomSpawner.name);
        }
    }

    int GetSpawnPointIndex(GameObject obj)
    {
        Vector3 objPosition = obj.transform.position;

        for (int i = 0; i < randomSpawner.spawnPoints.Length; i++)
        {
            Vector3 spawnPointPosition = randomSpawner.spawnPoints[i].transform.position;
            float distance = Vector3.Distance(spawnPointPosition, objPosition);

            if (distance < 0.5f)
            {
                return i;
            }
        }
        return -1;
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("SpawnedObject"))
        {
            //Debug.Log("Trigger entered by: " + other.gameObject.name);
            if (!activeCoroutines.ContainsKey(other.gameObject))
            {
                Sprite[] spritesToChange = GetSpritesForObject(other.gameObject);
                if (spritesToChange != null)
                {
                    Coroutine coroutine = StartCoroutine(ChangeSpriteOverTime(other.gameObject, spritesToChange));
                    activeCoroutines.Add(other.gameObject, coroutine);
                }
            }
        }
    }

    Sprite[] GetSpritesForObject(GameObject obj)
    {
        if (obj.name.Contains("Plant"))
        {
            return plantSprites;
        }
        else
        {
            return null;
        }
    }

    IEnumerator ChangeSpriteOverTime(GameObject obj, Sprite[] spritesToChange)
    {
        if (spritesToChange == null)
        {
            yield break;
        }

        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            yield break;
        }

        for (int i = 0; i < spritesToChange.Length; i++)
        {
            yield return new WaitForSeconds(2);
            spriteRenderer.sprite = spritesToChange[i];
        }

        int spawnPointIndex = GetSpawnPointIndex(obj);
        Debug.Log($"GetSpawnPointIndex returned {spawnPointIndex} for {obj.name}");
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            Debug.Log("Resetting spawn point at index: " + spawnPointIndex);
            randomSpawner.ResetSpawnPoint(spawnPointIndex);
        }

        yield return new WaitForSeconds(1);
        Destroy(obj);

        activeCoroutines.Remove(obj);
    }
}
