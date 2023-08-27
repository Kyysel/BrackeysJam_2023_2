using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HUDController : MonoBehaviour
{

    public static HUDController Instance;
    private ResourceManager resourceManager;
    
    public Dictionary<string, TextMeshProUGUI> resourceTextDict;
    public GameObject resourcePanelPrefab;
    public GameObject resourcesPanelUI;

    public GameObject buildingPanelUI;
    public GameObject buildingPanelPrefab;

    public GameObject topPanelHUD;
    public bool hudActive = false;
    [FormerlySerializedAs("_topPanelHUDTransform")] public Vector3 topPanelHUDTransform;

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
        topPanelHUDTransform = new Vector3(topPanelHUD.transform.localPosition.x, topPanelHUD.transform.localPosition.y);
        resourceManager = ResourceManager.Instance;
        resourceTextDict = new Dictionary<string, TextMeshProUGUI>();
        // create the dictionnary based on ResourceManager's resourceTypes
        foreach (string name in ResourceManager.Instance.resourceTypes)
        {
            resourceTextDict.Add(name, null);
        }
        
        InitializeHUD();
    }

    void OnToggleBuildMode(InputValue value)
    {
        hudActive = !hudActive;
        if (hudActive)
        {
            LeanTween.moveLocal(topPanelHUD, topPanelHUDTransform+Vector3.down*150, 0.2f);
        }
        else
        {
            LeanTween.moveLocal(topPanelHUD, topPanelHUDTransform, 0.2f);
        }
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

    private void UpdateResourceHUD(string type)
    {
        // access the specific text UI and update it
        resourceTextDict[type].text = resourceManager.resourceDict[type].ToString();
    }
    
    public void UpdateResourceHUD()
    {
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceTextDict[resource].text = resourceManager.resourceDict[resource].ToString();
        }
    }

    /**
     * Create the resource panels in the HUD
     */
    public void InitializeHUD()
    {
        for (int i=0; i<resourceManager.resourceTypes.Length; i++)
        {
            GameObject resourcePanel = Instantiate(resourcePanelPrefab, transform);
            resourcePanel.name =  resourceManager.resourceTypes[i] + "Panel";
            
            // create the resource panel
            ResourcePanel panel = resourcePanel.GetComponent<ResourcePanel>();
            panel.icon.sprite = resourceManager.resourceImages[i];
            panel.text.text = "0";
            
            // assign the text UI to the dictionary
            resourceTextDict[resourceManager.resourceTypes[i]] = panel.text;
            
            resourcePanel.transform.SetParent(resourcesPanelUI.transform, false);
        }

        foreach (Upgrade upgrade in UpgradeManager.Instance.upgrades)
        {
            GameObject buildingPanel = Instantiate(buildingPanelPrefab, transform);
            buildingPanel.name = upgrade.upgradeName + "Panel";
            
            //create the building panel
            BuildingPanel panel = buildingPanel.GetComponent<BuildingPanel>();
            panel.icon.sprite = upgrade.icon;
            //TODO add the list of resources need to build

            for (int i = 0; i < upgrade.costsArray.Count; i++)
            {
                panel.resourcePanels[i].SetActive(true);
                TextMeshProUGUI text = panel.resourcePanels[i].GetComponentInChildren<TextMeshProUGUI>();
                int cost = upgrade.costsArray[i];
                text.text = cost.ToString();
            }

            buildingPanel.transform.SetParent(buildingPanelUI.transform, false);
        }
        
    }

}
