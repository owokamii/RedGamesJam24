using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSquish : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 squishedScale = new Vector3(1.2f, 0.8f, 1.0f); // Scale when squished
    public float squishDuration = 0.1f; // Duration of the squish effect
    public float bounceBackDuration = 0.2f; // Duration of the bounce-back effect

    private Vector3 originalScale;
    private bool isSquished = false;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Squish the button when pressed
        if (!isSquished)
        {
            StopAllCoroutines();
            StartCoroutine(Squish());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Bounce back to the original scale when released
        if (isSquished)
        {
            StopAllCoroutines();
            StartCoroutine(BounceBack());
        }
    }

    private IEnumerator Squish()
    {
        isSquished = true;
        float elapsedTime = 0;

        while (elapsedTime < squishDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, squishedScale, elapsedTime / squishDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = squishedScale;
    }

    private IEnumerator BounceBack()
    {
        isSquished = false;
        float elapsedTime = 0;

        while (elapsedTime < bounceBackDuration)
        {
            transform.localScale = Vector3.Lerp(squishedScale, originalScale, elapsedTime / bounceBackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}