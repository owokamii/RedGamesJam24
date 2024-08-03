using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 squashScale = new Vector3(1.2f, 0.8f, 1.0f); // Scale when squashed
    public Vector3 stretchScale = new Vector3(0.8f, 1.2f, 1.0f); // Scale when stretched
    public float duration = 0.5f; // Duration of one squash or stretch cycle

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        StartCoroutine(SquashAndStretchRoutine());
    }

    private IEnumerator SquashAndStretchRoutine()
    {
        while (true)
        {
            // Squash
            yield return ScaleTo(squashScale);

            // Stretch
            yield return ScaleTo(stretchScale);

            // Return to original scale
            yield return ScaleTo(originalScale);
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Easing function for smooth transition
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}