using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Building building;
    MapCell cell;

    private void Start()
    {
        building = GetComponent<Building>();
        cell = building.GetCell();
    }

    private void Update()
    {
        if (GameManager.Instance.day)
        {
            cell.cost = 1;
        }

        if (!GameManager.Instance.day)
        {
            cell.cost = 10;
        }
    }
}
