using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

    public static HUDController Instance;

    [SerializeField] private TextMeshProUGUI stoneText, steelText, goldText, wormoniumText;

    private void Awake()
    {

        #region set instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        #endregion

    }

    private void Start()
    {
        InitializeHUD();
    }

    #region event subscription in OnEnable/Disable
    private void OnEnable()
    {
        ResourceManager.CollectResourcesEvent += UpdateResourceHUD;
    }

    private void OnDisable()
    {
        ResourceManager.CollectResourcesEvent -= UpdateResourceHUD;
    }
    #endregion

    private void UpdateResourceHUD(ResourceType type)
    {
        switch (type) 
        {

            case ResourceType.STONE:
                stoneText.text = "Stone: " + ResourceManager.Instance.stoneAmt;
                break;
            case ResourceType.STEEL:
                steelText.text = "Steel: " + ResourceManager.Instance.steelAmt;
                break;
            case ResourceType.GOLD:
                goldText.text = "Gold: " + ResourceManager.Instance.goldAmt;
                break;
            case ResourceType.WORMONIUM:
                wormoniumText.text = "Wormonium: " + ResourceManager.Instance.wormoniumAmt;
                break;

        }
    }

    public void InitializeHUD()
    {

        stoneText.text = "Stone: " + ResourceManager.Instance.stoneAmt;
        steelText.text = "Steel: " + ResourceManager.Instance.steelAmt;
        goldText.text = "Gold: " + ResourceManager.Instance.goldAmt;
        wormoniumText.text = "Wormonium: " + ResourceManager.Instance.wormoniumAmt;

    }

}
