using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanger : MonoBehaviour
{
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
    private RandomSpawner randomSpawner;
    private bool IsColliderEnter = false;

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
            Pullable pullableComponent = other.GetComponent<Pullable>();
            GrowthStages growthStages = other.GetComponent<GrowthStages>();
            if (pullableComponent != null && growthStages != null)
            {
                if (!activeCoroutines.ContainsKey(other.gameObject))
                {
                    if (IsColliderEnter)
                    {
                        pullableComponent.SetCurrentAnimationStage(0);
                        IsColliderEnter = true;
                    }
                    Coroutine coroutine = StartCoroutine(CheckAndChangeAnimation(other.gameObject, 0));
                    activeCoroutines.Add(other.gameObject, coroutine);
                }
            }
        }
    }

    IEnumerator CheckAndChangeAnimation(GameObject obj, int startIndex)
    {
        while (obj != null)
        {
            yield return StartCoroutine(ChangeAnimationStageOverTime(obj, startIndex));
            yield return null;
        }
    }

    IEnumerator ChangeAnimationStageOverTime(GameObject obj, int startIndex)
    {
        if (obj == null) yield break;

        Animator animator = obj.GetComponent<Animator>();
        Pullable pullableComponent = obj.GetComponent<Pullable>();
        GrowthStages growthStages = obj.GetComponent<GrowthStages>();

        if (pullableComponent == null || growthStages == null || animator == null)
        {
            yield break;
        }

        if (pullableComponent.isDestroyed)
        {
            yield break;
        }

        if (pullableComponent.isBeingDragged || pullableComponent.isMoving)
        {
            yield return null;
        }

        if (pullableComponent.isDestroyed || pullableComponent.isMoving)
        {
            yield break;
        }

        int currentStage = growthStages.GetCurrentStage();

        if (!growthStages.hasChangedState && pullableComponent.isBeingDragged)
        {
            pullableComponent.UpdateCurrentAnimationStage(0);
            growthStages.SetChangedStateTrue();
        }
        else if (growthStages.hasChangedState && pullableComponent.isBeingDragged)
        {
            if (currentStage == 1)
            {
                animator.speed = 0;
            }
        }
        else
        {
            Debug.Log("PlayerByebye1");
            animator.speed = 1;
            pullableComponent.UpdateCurrentAnimationStage(currentStage);
        }

        if (!pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
        {
            Debug.Log("PlayerByebye");
            int currentStage1 = growthStages.GetCurrentStage();
            if (currentStage1 == 2)
            {
                yield return new WaitForSeconds(1.0f);

                int spawnPointIndex = GetSpawnPointIndex(obj);
                if (spawnPointIndex != -1 && randomSpawner != null)
                {
                    CheckingColliderEnter();
                    Destroy(obj);
                    activeCoroutines.Remove(obj);
                    randomSpawner.ResetSpawnPoint(spawnPointIndex);
                    yield break;
                }
            }
        }
    }

    public void RestartCoroutine(GameObject obj, int startIndex)
    {
        if (activeCoroutines.ContainsKey(obj))
        {
            Coroutine routine = activeCoroutines[obj];
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            activeCoroutines[obj] = StartCoroutine(CheckAndChangeAnimation(obj, startIndex));
        }
        else
        {
            Coroutine coroutine = StartCoroutine(CheckAndChangeAnimation(obj, startIndex));
            activeCoroutines.Add(obj, coroutine);
        }
    }

    IEnumerator ResumeDestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj == null) yield break;
        Pullable pullableComponent = obj.GetComponent<Pullable>();
        GrowthStages growthStages = obj.GetComponent<GrowthStages>();
        if (pullableComponent != null && growthStages != null && !pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
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
            Coroutine routine = activeCoroutines[obj];
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            activeCoroutines.Remove(obj);
        }
    }

    public void CheckingColliderEnter()
    {
        IsColliderEnter = false;
    }

    public void ResumeAnimation(Animator animator)
    {
        animator.speed = 1;
    }
}
