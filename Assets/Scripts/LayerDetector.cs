using UnityEngine;

public class LayerDetector : MonoBehaviour
{
    [SerializeField] private int soilFrontLayer;

    private SpriteRenderer frontSoilSprite;
    private SpriteRenderer backSoilSprite;
    private int soilBackLayer;

    public int GetSoilFrontLayer { get; set; }

    private void Start()
    {
        GetSoilFrontLayer = soilFrontLayer;

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

        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        frontSoilSprite.sortingOrder = soilFrontLayer;
        backSoilSprite.sortingOrder = soilFrontLayer - 2;
    }
}