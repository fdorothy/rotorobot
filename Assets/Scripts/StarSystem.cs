using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour
{
    ParticleSystem system;

    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        Slow();
    }

    public void Fast()
    {
        var main = system.main;
        main.simulationSpeed = 3.0f;
    }

    public void Slow()
    {
        var main = system.main;
        main.simulationSpeed = 0.1f;
    }

    public void Stop()
    {
        var main = system.main;
        main.simulationSpeed = 0.0f;
    }
}
