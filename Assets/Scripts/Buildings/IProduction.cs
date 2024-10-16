using System.Collections.Generic;

public interface IProduction
{
    void GetResources();
    int CalculateResources(List<ResourceHandler> resourceList);
    void Work();
    int GetStorage();
    void SetStorage(int storage);
    ResourceManager.ResourceType GetResourceType();
    void AddWorkers(bool add);
    int GetWorkersCount();
    void RemoveWorker(Villager worker);
}
