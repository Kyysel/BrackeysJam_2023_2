using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    public Image icon;
    //public Building building;
    //public GameObject resourceCostPanel;
    public GameObject[] resourcePanels;

    private void Start()
    {

        InitializeBuildingImage();
        //Initialize cost icons
        foreach (GameObject go in resourcePanels)
        {
            TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();

            if (text.text == "0")
            {
                go.SetActive(false);
            }
        }

    }

    private void InitializeBuildingImage()
    {
        //calculate the size ratio of the icon
        float a = icon.preferredWidth, b = icon.preferredHeight;
        float greatestCommonDenominator = GCD(a, b);
        Vector2 ratio = new Vector2(a / greatestCommonDenominator, b / greatestCommonDenominator);

        //calculate the ratio'd height and width
        RectTransform rect = icon.GetComponent<RectTransform>();
        float newHeight = (rect.sizeDelta.x / ratio.x) * ratio.y;
        float newWidth = (rect.sizeDelta.y / ratio.y) * ratio.x;

        //change the Image RectTransform, keeping the biggest size always to 100
        rect.sizeDelta = a >= b ? new Vector2(rect.sizeDelta.x, newHeight) : new Vector2(newWidth, rect.sizeDelta.x);
    }

    //Returns the Greatest Common Denominator between two floats
    public static float GCD(float a, float b)
    {
        return b == 0 ? Math.Abs(a) : GCD(b, a % b);
    }

}
