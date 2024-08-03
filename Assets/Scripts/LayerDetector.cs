using UnityEngine;

public class LayerDetector : MonoBehaviour
{
    [SerializeField] private int soilFrontLayer;

    private SpriteRenderer frontSoilSprite;
    private SpriteRenderer backSoilSprite;
    [SerializeField] private int soilBackLayer;

    private void Start()
    {
        Transform soilFrontTransform = transform.Find("SoilFront");
        if (soilFrontTransform != null)
        {
            frontSoilSprite = soilFrontTransform.GetComponent<SpriteRenderer>();
        }

        Transform soilBackTransform = transform.Find("SoilBack");
        if (soilBackTransform != null)
        {
            backSoilSprite = soilBackTransform.GetComponent<SpriteRenderer>();
        }

        if (frontSoilSprite != null)
        {
            frontSoilSprite.sortingOrder = 1;
        }

        if (backSoilSprite != null)
        {
            backSoilSprite.sortingOrder = 0;
        }

        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        frontSoilSprite.sortingOrder = soilFrontLayer;
        backSoilSprite.sortingOrder = soilFrontLayer + 2;
    }
}