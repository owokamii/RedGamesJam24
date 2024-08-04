using System.Collections;
using UnityEngine;

public class BounceImage : MonoBehaviour
{
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private float bounceScale = 1.2f;

    private Vector3 originalScale;
    private bool hasBounced = false;

    private void Start()
    {
        // Store the original scale of the image
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        // Reset the bounce flag
        hasBounced = false;

        // Start the bounce animation if the object is active
        if (!hasBounced)
        {
            PlayBounceAnimation();
            hasBounced = true;
        }
    }

    public void PlayBounceAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(BounceCoroutine());
    }

    private IEnumerator BounceCoroutine()
    {
        // Scale up
        float elapsedTime = 0f;
        while (elapsedTime < bounceDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * bounceScale, elapsedTime / (bounceDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = originalScale * bounceScale;

        // Scale back to original
        elapsedTime = 0f;
        while (elapsedTime < bounceDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale * bounceScale, originalScale, elapsedTime / (bounceDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the original scale
        transform.localScale = originalScale;
    }
}