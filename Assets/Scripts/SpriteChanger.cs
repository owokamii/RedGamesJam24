using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] plantSprites;

    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
    private RandomSpawner randomSpawner;
    private bool IscolliderEnter = false;

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

            if (distance < 0.6f)
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
                    if (pullableComponent != null)
                    {
                        if (IscolliderEnter)
                        {
                            pullableComponent.SetCurrentSprites(spritesToChange, 0);
                            IscolliderEnter = true;
                        }
                        Coroutine coroutine = StartCoroutine(ChangeSpriteOverTime(other.gameObject, spritesToChange, 0));
                        activeCoroutines.Add(other.gameObject, coroutine);
                    }
                }
            }
        }
    }

    //如果有新object加进来这里
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
            if (pullableComponent.isDestroyed)
            {
                yield break;
            }

            while (pullableComponent.isBeingDragged || pullableComponent.isMoving)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2);
            if (pullableComponent.isDestroyed || pullableComponent.isMoving)
            {
                yield break;
            }

            if (obj != null)
            {
                spriteRenderer.sprite = spritesToChange[i];
                pullableComponent.UpdateCurrentSpriteIndex(i);
            }
        }

        int spawnPointIndex = GetSpawnPointIndex(obj);
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            for (float timer = 1f; timer > 0; timer -= Time.deltaTime)
            {
                if (pullableComponent.isDestroyed || pullableComponent.isBeingDragged || pullableComponent.isMoving)
                {
                    pullableComponent.remainingDestroyTime = timer;
                    yield break;
                }
                yield return null;
            }

            if (!pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
            {
                CheckingColliderEnter();
                Destroy(obj);
                activeCoroutines.Remove(obj);
            }

            for (float timer = 2f; timer > 0; timer -= Time.deltaTime)
            {
                if (pullableComponent.isDestroyed || pullableComponent.isBeingDragged || pullableComponent.isMoving)
                {
                    pullableComponent.remainingDestroyTime = timer;
                    yield break;
                }
                yield return null;
            }

            if (!pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
            {
                Debug.Log("Resetting spawn point at index: " + spawnPointIndex);
                randomSpawner.ResetSpawnPoint(spawnPointIndex);
            }
        }
    }

    public void RestartCoroutine(GameObject obj, Sprite[] spritesToChange, int startIndex)
    {
        Pullable pullableComponent = obj.GetComponent<Pullable>();
        if (pullableComponent != null && pullableComponent.remainingDestroyTime > 0)
        {
            StartCoroutine(ResumeDestroyAfterDelay(obj, pullableComponent.remainingDestroyTime));
        }
        else
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

    IEnumerator ResumeDestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj == null) yield break;
        Pullable pullableComponent = obj.GetComponent<Pullable>();
        if (pullableComponent != null && !pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
        {
            int spawnPointIndex = GetSpawnPointIndex(obj);
            if (spawnPointIndex != -1 && randomSpawner != null)
            {
                CheckingColliderEnter();
                Destroy(obj);
                activeCoroutines.Remove(obj);
                randomSpawner.ResetSpawnPoint(spawnPointIndex);
            }
        }
    }

    public void StopCoroutineForObject(GameObject obj)
    {
        if (activeCoroutines.ContainsKey(obj))
        {
            StopCoroutine(activeCoroutines[obj]);
            activeCoroutines.Remove(obj);
        }
    }

    public void CheckingColliderEnter()
    {
        IscolliderEnter = false;
    }
}
