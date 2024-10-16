using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    MapCell thisCell;

    private void Start()
    {
        thisCell = GetComponent<Building>().GetCell();
    }
}
