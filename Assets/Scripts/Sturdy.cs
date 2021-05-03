using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
