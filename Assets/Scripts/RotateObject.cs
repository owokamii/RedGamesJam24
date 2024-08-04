using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float squashAmount = 0.2f; // How much to squash/stretch
    [SerializeField] private float squashDuration = 0.5f; // Duration of one squash/stretch cycle

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(SquashAndStretch());
    }

    private IEnumerator SquashAndStretch()
    {
        while (true)
        {
            // Squash
            yield return StartCoroutine(AnimateScale(new Vector3(originalScale.x + squashAmount, originalScale.y - squashAmount, originalScale.z), squashDuration / 2));

            // Return to normal
            yield return StartCoroutine(AnimateScale(originalScale, squashDuration / 2));

            // Stretch
            yield return StartCoroutine(AnimateScale(new Vector3(originalScale.x - squashAmount, originalScale.y + squashAmount, originalScale.z), squashDuration / 2));

            // Return to normal
            yield return StartCoroutine(AnimateScale(originalScale, squashDuration / 2));
        }
    }

    private IEnumerator AnimateScale(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}