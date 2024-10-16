using System.Collections.Generic;
using UnityEngine;

public class Villager
{
    private PopulationManager.VillagerGender gender;
    private PopulationManager.VillagerProfession profession;
    private House home;
    private Building work;
    private Army army;
    private string name = "I'm a Villager";
    private VillagerBody body;

    public Villager()
    {
        profession = PopulationManager.VillagerProfession.None;
        SetGender();
        //SetName();
        FindHome();
    }

    void SetGender()
    {
        int genderIndex = Random.Range(0, 2);
        switch (genderIndex)
        {
            case 0: gender = PopulationManager.VillagerGender.Male; break;
            case 1: gender = PopulationManager.VillagerGender.Female; break;
        }
    }
    void SetName()
    {
        List<string> names = new List<string>();

        if (gender == PopulationManager.VillagerGender.Male)
            names = PopulationManager.Instance.maleNames;
        else if (gender == PopulationManager.VillagerGender.Female)
            names = PopulationManager.Instance.femaleNames;

        name = names[Random.Range(0, names.Count)];
    }
    void FindHome()
    {
        List<House> houses = new List<House>();

        foreach (Building building in BuildingsManager.Instance.buildingsOnMap)
        {
            House house = null;

            if (building.GetBuildingType() == BuildingsManager.BuildingType.House)
            {
                house = building.GetComponent<House>();
            }
            else continue;

            if (house.villagers.Count < house.maxPopulation)
            {
                houses.Add(house);
            }
        }

        if (houses.Count == 0) return;

        House home = houses[Random.Range(0, houses.Count)];

        home.villagers.Add(this);
        this.home = home;
    }

    public PopulationManager.VillagerProfession GetProfession() { return profession; }
    public void SetProfession(PopulationManager.VillagerProfession profession) { this.profession = profession; }
    public Building GetWorkingPlace() { return work; }
    public void SetWorkingPlace(Building work) { this.work = work; }
    public string GetName() { return name; }
    public void SetName(string name) { this.name = name; }
    public bool IsHomeless()
    {
        if (home == null) return true;
        else return false;
    }
    public void SetHome(House home) { this.home = home; }
    public House GetHouse() { return home; }
    public void SetBody(VillagerBody body) { this.body = body; }
    public VillagerBody GetBody() { return body; }
    public void SetArmy(Army army) {  this.army = army; }
    public Army GetArmy() { return army; }
}
