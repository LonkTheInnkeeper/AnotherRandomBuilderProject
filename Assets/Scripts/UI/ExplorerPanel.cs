using System.Collections.Generic;
using UnityEngine;

public class ExplorerPanel : MonoBehaviour
{
    [SerializeField] Explorer explorer;

    [Space]
    [SerializeField] GameObject panel;

    [SerializeField] List<IBuildableUI> UIs = new List<IBuildableUI>();
    Building activeBuilding;

    private void Start()
    {
        IBuildableUI[] buildableUIs = panel.GetComponents<IBuildableUI>();
        UIs.AddRange(buildableUIs);
        panel.SetActive(false);
    }

    public void ToggleExplorerPanel(Building building)
    {
        if (building == null) 
        { 
            panel.SetActive(false); 
        }
        else
        {
            activeBuilding = building;
            panel.SetActive(true);
            SellectPanel();
        }
    }

    void SellectPanel()
    {
        foreach (var panel in UIs)
        {
            panel.TogglePanel(false);
        }

        foreach (var panel in UIs)
        {
            if (panel.GetBuildingType() == activeBuilding.GetBuildingType())
            {
                panel.TogglePanel(true);
                panel.SetBuilding(activeBuilding);
                break;
            }
        }
    }
}
