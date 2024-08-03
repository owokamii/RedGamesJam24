using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanger : MonoBehaviour
{
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
    private RandomSpawner randomSpawner;
    private bool IsColliderEnter = false;
    private LayerDetector layerDetector;

    void Start()
    {
        layerDetector = GetComponent<LayerDetector>();
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
        if (obj == null)
            return -1;

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
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Change the sorting order
                spriteRenderer.sortingOrder = layerDetector.GetSoilFrontLayer - 1;
            }
            Pullable pullableComponent = other.GetComponent<Pullable>();
            if (pullableComponent != null && !pullableComponent.hasChangedState)
            {
                if (!activeCoroutines.ContainsKey(other.gameObject))
                {
                    if (IsColliderEnter)
                    {
                        pullableComponent.SetCurrentAnimationStage(0);
                        IsColliderEnter = true;
                    }
                    Coroutine coroutine = StartCoroutine(ChangeAnimationStageOverTime(other.gameObject, 0));
                    activeCoroutines.Add(other.gameObject, coroutine);
                }
            }
        }
    }

    IEnumerator ChangeAnimationStageOverTime(GameObject obj, int startIndex)
    {
        Animator animator = obj.GetComponent<Animator>();
        Pullable pullableComponent = obj.GetComponent<Pullable>();

        for (int i = startIndex; i < pullableComponent.animationStages.Length; i++)
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
                animator.Play(pullableComponent.animationStages[i]);
                pullableComponent.UpdateCurrentAnimationStage(i);
                pullableComponent.hasChangedState = true;
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
                randomSpawner.ResetSpawnPoint(spawnPointIndex);
            }
        }
    }

    public void RestartCoroutine(GameObject obj, int startIndex)
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
                activeCoroutines[obj] = StartCoroutine(ChangeAnimationStageOverTime(obj, startIndex));
            }
            else
            {
                Coroutine coroutine = StartCoroutine(ChangeAnimationStageOverTime(obj, startIndex));
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
        IsColliderEnter = false;
    }
}
