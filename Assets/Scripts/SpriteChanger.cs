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
            if (!activeCoroutines.ContainsKey(other.gameObject))
            {
                Sprite[] spritesToChange = GetSpritesForObject(other.gameObject);
                if (spritesToChange != null)
                {
                    Pullable pullableComponent = other.GetComponent<Pullable>();
                    pullableComponent.SetCurrentSprites(spritesToChange, 0);
                    Coroutine coroutine = StartCoroutine(ChangeSpriteOverTime(other.gameObject, spritesToChange, 0));
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

    IEnumerator ChangeSpriteOverTime(GameObject obj, Sprite[] spritesToChange, int startIndex)
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

        Pullable pullableComponent = obj.GetComponent<Pullable>();
        if (pullableComponent == null)
        {
            yield break;
        }

        for (int i = startIndex; i < spritesToChange.Length; i++)
        {
            while (pullableComponent.isBeingDragged || pullableComponent.isMoving)
            {
                yield return null; // 等待直到拖拽结束或移动完成
            }

            yield return new WaitForSeconds(2);
            if(spriteRenderer != null)
            {
                spriteRenderer.sprite = spritesToChange[i];
                pullableComponent.currentSpriteIndex = i; // 更新当前的 Sprite 索引
            }
        }

        int spawnPointIndex = GetSpawnPointIndex(obj);
        //Debug.Log($"GetSpawnPointIndex returned {spawnPointIndex} for {obj.name}");
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            for (float timer = 1f; timer > 0; timer -= Time.deltaTime)
            {
                if (pullableComponent.isBeingDragged || pullableComponent.isMoving)
                {
                    yield return null;
                }
                yield return null;
            }

            if (!pullableComponent.isBeingDragged && !pullableComponent.isMoving)
            {
                pullableComponent.CheckPlantStatus();
                Destroy(obj);
                activeCoroutines.Remove(obj);
            }

            for (float timer = 2f; timer > 0; timer -= Time.deltaTime)
            {
                if (pullableComponent.isBeingDragged || pullableComponent.isMoving)
                {
                    yield return null;
                }
                yield return null;
            }

            if (!pullableComponent.isBeingDragged && !pullableComponent.isMoving)
            {
               // Debug.Log("Resetting spawn point at index: " + spawnPointIndex);
                randomSpawner.ResetSpawnPoint(spawnPointIndex);
            }
        }
    }

    public void RestartCoroutine(GameObject obj, Sprite[] spritesToChange, int startIndex)
    {
        if (activeCoroutines.ContainsKey(obj))
        {
            StopCoroutine(activeCoroutines[obj]);
            activeCoroutines[obj] = StartCoroutine(ChangeSpriteOverTime(obj, spritesToChange, startIndex));
        }
        else
        {
            Coroutine coroutine = StartCoroutine(ChangeSpriteOverTime(obj, spritesToChange, startIndex));
            activeCoroutines.Add(obj, coroutine);
        }
    }
}
