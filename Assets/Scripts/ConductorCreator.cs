using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorCreator : MonoBehaviour
{
    public GameObject conductorPrefab;

    private void Start()
    {
        if (conductorPrefab == null)
        {
            throw new System.InvalidOperationException("You must add a conductor prefab to the ConductorCreator GameObject");
        }

        Conductor.CreateInstance(conductorPrefab);
    }
}
