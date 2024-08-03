using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite highlightedDot;
    [SerializeField] private Sprite unhighlightedDot;

    [SerializeField] private Image giftImage;
    [SerializeField] private Sprite giftNoti;
    [SerializeField] private Image shopImage;
    [SerializeField] private Sprite shopNoti;

    private int currentSceneIndex = -1;

    public void SetCurrentScene(int index)
    {
        if (index < 0 || index >= scenes.Length) return;

        // Deactivate all scenes
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }

        // Activate the selected scene
        scenes[index].SetActive(true);

        // Update button sprites
        UpdateButtonSprites(index);
    }

    public void UpdateButtonSprites(int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            Image buttonImage = images[i].GetComponent<Image>();

            if (i == index)
            {
                buttonImage.sprite = highlightedDot;
            }
            else
            {
                buttonImage.sprite = unhighlightedDot;
            }
        }
    }

    public void ChangeGiftSprite()
    {
        Image buttonImage = giftImage.GetComponent<Image>();
        buttonImage.sprite = giftNoti;
    }

    public void ChangeShopNoti()
    {
        Image buttonImage = shopImage.GetComponent<Image>();
        buttonImage.sprite = shopNoti;
    }
}