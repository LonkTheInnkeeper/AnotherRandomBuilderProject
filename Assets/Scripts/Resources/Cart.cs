using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Cart : MonoBehaviour
{
    List<MapCell> path = new List<MapCell>();

    MapCell startCell;
    MapCell endCell;

    Building building;
    Barn barn;

    int resources;
    ResourceManager.ResourceType resourceType;

    public float speed;
    int sequence = 0;

    private void Update()
    {
        if (path.Count > 0 && transform.position != path.Last().GetCoordinates())
        {
            sequence = PFTools.Navigate(transform, path, speed, sequence);
        }

        else if (transform.position == endCell.GetCoordinates())
            GatherGoods();

        else if (transform.position == startCell.GetCoordinates())
        {
            EmptyCart();
        }
    }

    public void GoToBuilding(List<MapCell> path, Building building, Barn barn)
    {
        this.path = path;
        this.building = building;
        this.barn = barn;

        startCell = path.First();
        endCell = path.Last();
    }

    void GatherGoods()
    {
        sequence = 0;
        path.Reverse();
        IProduction production = building.GetComponent<IProduction>();

        resources = production.GetStorage();
        production.SetStorage(0);

        resourceType = production.GetResourceType();
        building.exported = false;

        if (path.Count == 1)
        {
            EmptyCart();
        }
    }

    void EmptyCart()
    {
        barn.EmptyCart(resources, resourceType);
        resources = 0;
        resourceType = ResourceManager.ResourceType.None;
        Destroy(gameObject);
    }
}
