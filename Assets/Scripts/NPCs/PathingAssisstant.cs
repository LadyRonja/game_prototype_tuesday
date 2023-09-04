using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingAssisstant : MonoBehaviour
{
    public static PathingAssisstant Instance;
    [SerializeField] private List<Transform> gawkPoints;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Transform GetRandomPoint()
    {
        return gawkPoints[UnityEngine.Random.Range(0, gawkPoints.Count)];
    }
}
