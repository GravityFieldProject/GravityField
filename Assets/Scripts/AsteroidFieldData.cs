using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldData : MonoBehaviour
{
    [SerializeField] public double UA = 149597828677.28f;
    [SerializeField] public double GRAVITATIONAL_CONSTANT = 6.67430f * Mathf.Pow(10, -11);
    [SerializeField] public int step = 3600;
    [SerializeField] public bool showTrails = false;
}
