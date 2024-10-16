using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform ghost;

    [Header("Materials")]
    [SerializeField] Material transparentGreen;
    [SerializeField] Material transparentRed;

    [Space]
    [SerializeField] Transform visual;
    MeshFilter visualMesh;
    MeshRenderer visualMaterial;

    Vector3 position;

    GameManager gameMan;
    MapManager mapMan;
    Building activeBuilding;
    ResourceManager resourceMan;

    BuildingsManager buildingsMan;
    IResource activeResource;

    GameManager.GameState gameState;

    private void Start()
    {
        gameMan = GameManager.Instance;
        mapMan = MapManager.Instance;
        buildingsMan = BuildingsManager.Instance;
        resourceMan = ResourceManager.Instance;

        visualMesh = visual.GetComponent<MeshFilter>();
        visualMaterial = visual.GetComponent<MeshRenderer>();

        UpdateVisual();
    }

    void Update()
    {
        gameState = gameMan.GetGameState();

        if ((gameState == GameManager.GameState.Exploring || 
             gameState == GameManager.GameState.Navigating) &&
            (buildingsMan.activeBuilding == BuildingsManager.BuildingType.None ||
             resourceMan.activeResource == ResourceManager.ResourceType.None)
            )
        {
            visual.gameObject.SetActive(false);
            return;
        }

        if (!visual.gameObject.activeInHierarchy)
            visual.gameObject.SetActive(true);

        if (gameState == GameManager.GameState.Constructing)
            visual.rotation = Quaternion.Euler(new Vector3(0, buildingsMan.rotationIndex * 60, 0));

        mapMan.SetMapCell();

        if (mapMan.activeCell != null)
        {

            position = mapMan.activeCell.GetCoordinates();
            ghost.position = new Vector3(position.x, 0.3f, position.z);

            CheckOccupied();
        }
    }

    public void UpdateVisual()
    {
        if (gameMan.GetGameState() == GameManager.GameState.Constructing)
        {
            activeBuilding = buildingsMan.GetActiveBuilding();
            visualMesh.mesh = activeBuilding.GetVisual();
        }
        else if (gameMan.GetGameState() == GameManager.GameState.Editing)
        {
            activeResource = resourceMan.GetActiveResource().GetComponent<IResource>();

            if (activeResource != null)
            {
                visual.gameObject.SetActive(true);
                visualMesh.mesh = activeResource.GetVisual();
            }
            else
                visual.gameObject.SetActive(false);
        }
    }

    void CheckOccupied()
    {
        MapCell cell = mapMan.activeCell;

        // Building state
        if (gameMan.GetGameState() == GameManager.GameState.Constructing && activeBuilding != null)
        {
            foreach (MapCell _cell in GetBuildingSize(cell))
            {
                if (_cell.occupied == true)
                {
                    ChangeOccupiedMaterial(true);
                    return;
                }
            }

            // Check general building occupation
            if (activeBuilding != null && activeBuilding.GetBuildingType() != BuildingsManager.BuildingType.Mine)
            {
                ChangeOccupiedMaterial(cell == null || cell.occupied);
            }

            // Check occupation for Mine
            else if (activeBuilding != null && activeBuilding.GetBuildingType() == BuildingsManager.BuildingType.Mine)
            {
                ChangeOccupiedMaterial(cell == null ||
                                       cell.GetResource() == null ||
                                       cell.GetBuilding() != null ||
                                       cell.GetResource().GetResourceType() != ResourceManager.ResourceType.Ore);
            }
        }

        // Editing state
        else if (gameMan.GetGameState() == GameManager.GameState.Editing)
        {
            // Check general resource occupation
            if (activeResource != null && activeResource.GetResourceType() != ResourceManager.ResourceType.Field)
            {
                ChangeOccupiedMaterial(cell == null || cell.GetResource() != null || cell.occupied);
            }

            // Check occupation for field
            else if (activeResource != null && activeResource.GetResourceType() == ResourceManager.ResourceType.Field)
            {
                bool millNeighbor = false;

                foreach (MapCell neighbor in cell.GetNeighbors())
                {
                    if (neighbor.GetBuilding() != null && neighbor.GetBuilding().GetBuildingType() == BuildingsManager.BuildingType.Mill)
                    {
                        millNeighbor = true;
                        break;
                    }
                }

                ChangeOccupiedMaterial(cell == null || cell.occupied || !millNeighbor);
            }
        }
    }

    void ChangeOccupiedMaterial(bool occupied)
    {
        if (occupied)
        {
            visualMaterial.material = transparentRed;
        }
        else
        {
            visualMaterial.material = transparentGreen;
        }
    }

    List<MapCell> GetBuildingSize(MapCell activeCell)
    {
        List<MapCell> building = new List<MapCell>();
        int size = activeBuilding.GetSize();

        building.Add(activeCell);

        if (size > 1)
        {
            List<MapCell> neighbors = activeCell.GetNeighbors();

            for (int i = 0; i < size - 1; i++)
            {
                int index = i + buildingsMan.rotationIndex;

                if (index > neighbors.Count - 1)
                {
                    index -= neighbors.Count;
                }

                building.Add(neighbors[index]);
            }
        }

        return building;
    }
}
