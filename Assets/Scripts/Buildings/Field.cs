using UnityEngine;

public class Field : MonoBehaviour
{
    Mill mill;
    bool hasFarmer = true;
    bool dayTime = true;

    [SerializeField] float harvestTimeOrigin;
    float harvestTime;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        harvestTime = harvestTimeOrigin;
    }

    private void Update()
    {
        //if (hasFarmer && gameManager.day)
        //{
        //    harvestTime -= Time.deltaTime;

        //    if (harvestTime < 0)
        //    {
        //        PopulationManager.Instance.UpdateFood(1);
        //        harvestTime = harvestTimeOrigin;
        //    }
        //}
    }
}
