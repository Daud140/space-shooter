using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject[] Planets;
    readonly Queue<GameObject> availablePlanets = new Queue<GameObject>();

    void Start()
    {
        if (Planets != null)
        {
            foreach (var p in Planets)
            {
                if (p != null) availablePlanets.Enqueue(p);
            }
        }
        InvokeRepeating("MovePlanetDown", 0f, 20f);
    }

    void MovePlanetDown()
    {
        EnqueuePlanets();
        if (availablePlanets.Count == 0) return;

        GameObject aPlanet = availablePlanets.Dequeue();
        if (aPlanet != null)
        {
            aPlanet.GetComponent<Planet>().isMoving = true;
        }
    }

    void EnqueuePlanets()
    {
        if (Planets == null) return;
        foreach (GameObject aPlanet in Planets)
        {
            if (aPlanet == null) continue;
            Planet p = aPlanet.GetComponent<Planet>();
            if (aPlanet.transform.position.y < 0 && !p.isMoving)
            {
                p.ResetPosition();
                availablePlanets.Enqueue(aPlanet);
            }
        }
    }
}

