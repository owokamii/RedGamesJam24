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
            Debug.Log("Helloworld");
            Pullable pullableComponent = other.GetComponent<Pullable>();
            GrowthStages growthStages = other.GetComponent<GrowthStages>();
            if (pullableComponent != null && growthStages != null && !growthStages.HasChangedState())
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
        GrowthStages growthStages = obj.GetComponent<GrowthStages>();

        while (true)
        {
            if (pullableComponent.isDestroyed)
            {
                yield break;
            }

            // 每次循环检查isBeingDragged和其他状态
            Debug.Log($"Loop Check - isBeingDragged: {pullableComponent.isBeingDragged}, isMoving: {pullableComponent.isMoving}, currentStage: {growthStages.GetCurrentStage()}");

            if (pullableComponent.isBeingDragged || pullableComponent.isMoving)
            {
                yield return null;
                continue;
            }

            if (pullableComponent.isDestroyed || pullableComponent.isMoving)
            {
                yield break;
            }

            if (obj != null)
            {
                int currentStage = growthStages.GetCurrentStage();
                Debug.Log($"Current Stage: {currentStage}, HasChangedState: {growthStages.HasChangedState()}, IsBeingDragged: {pullableComponent.isBeingDragged}");

                if (!growthStages.HasChangedState() && pullableComponent.isBeingDragged)
                {
                    Debug.Log("Entering first if condition");
                    pullableComponent.UpdateCurrentAnimationStage(0);
                    growthStages.SetChangedStateTrue();
                }
                else if (growthStages.HasChangedState() && pullableComponent.isBeingDragged)
                {
                    Debug.Log("Entering second if condition");
                    if (currentStage == 1)
                    {
                        Debug.Log("Entering inner if condition of second else if");
                        animator.speed = 0;
                        yield break;
                    }
                }
                else
                {
                    Debug.Log("Entering else condition");
                    animator.speed = 1;
                    pullableComponent.UpdateCurrentAnimationStage(currentStage);
                }

                if (currentStage == 2)
                {
                    yield return new WaitForSeconds(0.5f);
                    if (!pullableComponent.isDestroyed && !pullableComponent.isBeingDragged && !pullableComponent.isMoving)
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
            }
            yield return null;
        }
    }

    public void RestartCoroutine(GameObject obj, int startIndex)
    {
        Pullable pullableComponent = obj.GetComponent<Pullable>();
        GrowthStages growthStages = obj.GetComponent<GrowthStages>();
        if (pullableComponent != null && growthStages != null && pullableComponent.remainingDestroyTime > 0)
        {
            StartCoroutine(ResumeDestroyAfterDelay(obj, pullableComponent.remainingDestroyTime));
        }
        else
        {
            if (activeCoroutines.ContainsKey(obj))
            {
                Coroutine routine = activeCoroutines[obj];
                if (routine != null)
                {
                    StopCoroutine(routine);
                }
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
