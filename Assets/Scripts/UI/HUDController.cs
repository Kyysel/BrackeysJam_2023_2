using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

    public static HUDController Instance;
    private ResourceManager resourceManager;
    
    public Dictionary<string, TextMeshProUGUI> resourceTextDict;
    public GameObject resourcePanelPrefab;
    public GameObject resourcesPanelUI;

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
        resourceManager = ResourceManager.Instance;
        resourceTextDict = new Dictionary<string, TextMeshProUGUI>();
        // create the dictionnary based on ResourceManager's resourceTypes
        foreach (string name in ResourceManager.Instance.resourceTypes)
        {
            resourceTextDict.Add(name, null);
        }
        
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

    private void UpdateResourceHUD(string type)
    {
        // access the specific text UI and update it
        resourceTextDict[type].text = resourceManager.resourceDict[type].ToString();
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
    }

}
