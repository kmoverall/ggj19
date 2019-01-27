using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private List<Planet> Planets = new List<Planet>();
    public BoxCollider2D SpaceZone;
    public Planet PlanetPrefab;
    public Planet Earth;

    public float baseDiscoveryChance;
    public float discoveryDecay;
    public float popGrowthRate;
    public float popGrowthRateLinear;
    public float costPopBase;
    public float costPopExponent;
    public float costPerDistance;

    public static Planet Home;

    private void Start()
    {
        TimeManager.OnDayTick += DiscoverPlanets;
        TimeManager.OnDayTick += UpdatePlanets;
        Home = Earth;
        GeneratePlanet();
    }

    public void DiscoverPlanets()
    {
        float r = Random.Range(0f, 1f);
        if (r <= baseDiscoveryChance)
        {
            GeneratePlanet();
            baseDiscoveryChance -= discoveryDecay;
        }
    }
    
    public void UpdatePlanets()
    {
        foreach (var planet in Planets)
        {
            planet.Population *= popGrowthRate;
            planet.Population += popGrowthRateLinear;
            planet.RealEstateCost = Mathf.Pow(costPopBase, planet.Population * costPopExponent);
            planet.FlightCost = costPerDistance * Vector3.Distance(planet.transform.position, Earth.transform.position);
        }
    }

    private void GeneratePlanet()
    {
        var newPlanet = Instantiate(PlanetPrefab, SpaceZone.transform);
        Vector3 position = Vector3.zero;
        position.x = Random.Range(SpaceZone.bounds.min.x, SpaceZone.bounds.max.x);
        position.y = Random.Range(SpaceZone.bounds.min.y, SpaceZone.bounds.max.y);

        newPlanet.transform.position = position;
        newPlanet.Initialize();

        Planets.Add(newPlanet);
    }
}