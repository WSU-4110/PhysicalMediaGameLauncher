using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class ImageSelector : MonoBehaviour
{
    public Transform stockImageGrid;
    public GameObject imageButtonPrefab;
    public Button pickFromPcButton;
    public Button cancelButton;
    public LoginUIController uiController;

    private string[] stockImageNames = {
        "StockImage1", "StockImage2", "StockImage3", "StockImage4", "StockImage5",
        "StockImage6", "StockImage7", "StockImage8", "StockImage9", "StockImage10"
    };

    private void Start()
    {
        foreach (string imageName in stockImageNames)
        {
            Sprite sprite = Resources.Load<Sprite>("StockProfilePics/" + imageName);
            if (sprite != null)
            {
                GameObject btnObj = Instantiate(imageButtonPrefab, stockImageGrid);
                Image img = btnObj.transform.Find("Image").GetComponent<Image>();
                img.sprite = sprite;

                Button btn = btnObj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    uiController.OnImageSelectedFromStock(sprite);
                    gameObject.SetActive(false);
                });
            }
        }

        pickFromPcButton.onClick.AddListener(() =>
        {
            uiController.OnPickImageFromPC();
            gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}